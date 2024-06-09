using Polly;
using ShippingServices.DAL;
using ShippingServices.DAL.Interface;
using ShippingServices.DTO;
using ShippingServices.Models;
using ShippingServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IShipping, ShippingDapper>();

builder.Services.AddHttpClient<IWalletServices, WalletServices>()
.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
builder.Services.AddHttpClient<IOrderHeaderServices, OrderHeaderServices>()
.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/shipping", (IShipping shipping) =>
{
    List<ShippingDTO> shippingDTOs = new List<ShippingDTO>();
    var shippings = shipping.GetAll();
    foreach (var ship in shippings)
    {
        shippingDTOs.Add(new ShippingDTO
        {
            shippingid = ship.shippingid,
            shippingvendor = ship.shippingvendor,
            shippingdate = ship.shippingdate,
            shippingstatus = ship.shippingstatus,
            orderHeaderId = ship.orderHeaderId,
            beratbarang = ship.beratbarang,
            biayashipping = ship.biayashipping,
        });
    }
    return Results.Ok(shippingDTOs);
});

app.MapGet("/api/shipping/{id}", (IShipping shipping, int id) =>
{
    ShippingDTO shippingDTOs = new ShippingDTO();
    var ship = shipping.GetById(id);
    if (ship == null)
    {
        return Results.NotFound();
    }
    shippingDTOs.shippingid = ship.shippingid;
    shippingDTOs.shippingvendor = ship.shippingvendor;
    shippingDTOs.shippingdate = ship.shippingdate;
    shippingDTOs.shippingstatus = ship.shippingstatus;
    shippingDTOs.orderHeaderId = ship.orderHeaderId;
    shippingDTOs.beratbarang = ship.beratbarang;
    shippingDTOs.biayashipping = ship.beratbarang;
    return Results.Ok(shippingDTOs);
});

app.MapPost("/api/shipping", async (IShipping shipping, ShippingDTO shippingDTO, IOrderHeaderServices orderHeaderServices,IWalletServices walletServices, string paymentid, string paymentwallet) =>
{
    try
    {
       
        Shipping ship = new Shipping
        {
            shippingid = shippingDTO.shippingid,
            shippingvendor = shippingDTO.shippingvendor,
            shippingdate = shippingDTO.shippingdate,
            shippingstatus = shippingDTO.shippingstatus,
            orderHeaderId = shippingDTO.orderHeaderId,
            beratbarang = shippingDTO.beratbarang,
            biayashipping = shippingDTO.biayashipping,
        };
        shipping.Insert(ship);
        var order = await orderHeaderServices.GetOrderHeaderById(ship.orderHeaderId);
        if (order == null)
        {
            return Results.BadRequest("OrderHeader not found");
        }
        var userWallet = await walletServices.GetUserWalletByUsername(order.username);
        var walletUpdateSaldoDTO = new WalletUpdateSaldoDTO
        {
            username = userWallet.username,
            paymentid = paymentid,
            paymentwallet = paymentwallet,
            saldo = ship.biayashipping
        };
        //return 201 Created
        await walletServices.UpdateSaldoWallet(walletUpdateSaldoDTO);
        return Results.Created($"/api/categories/{ship.shippingid}", ship);
        
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/updatestatus", (IShipping iShipping, ShippingUpdateStatusDTO shippingDTO) =>
{
    try
    {
        var shipping = new Shipping
        {
            shippingid = shippingDTO.shippingid,
            shippingstatus = shippingDTO.shippingstatus
        
        };
        iShipping.UpdateStatus(shipping);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
