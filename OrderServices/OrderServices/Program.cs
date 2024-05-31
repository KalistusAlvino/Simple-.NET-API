
using OrderServices.DAL;
using OrderServices.DAL.Interface;
using OrderServices.DTO;
using OrderServices.Models;
using OrderServices.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomer, CustomerDAL>();
builder.Services.AddScoped<IOrderHeader, OrderHeaderDAL>();
builder.Services.AddScoped<IOrderDetail, OrderDetailDAL>();


//register httpClient product

builder.Services.AddHttpClient<IProductService, ProductService>()
.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));
builder.Services.AddHttpClient<IWalletService, WalletService>()
.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Customers Service
app.MapGet("api/customers", (ICustomer CustomerDAL) =>
{
    var customer = CustomerDAL.GetAll();
    return Results.Ok(customer);
});

app.MapPost("api/customers", (ICustomer CustomerDAL, Customer customer) =>
{
    try{
        var cust = CustomerDAL.Insert(customer);
        return Results.Created($"/api/customers/{cust.CustomerId}", cust);
    }
    catch(Exception ex){
        return Results.BadRequest(ex.Message);
    }
});
//OrderHeaders Service
app.MapGet("api/orderHeaders", (IOrderHeader OrderHeaderDAL) =>
{
    var orderHeader = OrderHeaderDAL.GetAll();
    return Results.Ok(orderHeader);
});

app.MapPost("api/orderHeaders", (IOrderHeader OrderHeaderDAL, OrderHeader orderHeader) =>
{
    try{
        var orderHead = OrderHeaderDAL.Insert(orderHeader);
        return Results.Created($"/api/orderHeaders/{orderHead.OrderHeaderId}", orderHead);
    }
    catch(Exception ex){
        return Results.BadRequest(ex.Message);
    }
});
//OrderDetail Service
app.MapGet("/orderDetails", (IOrderDetail orderDetail) =>
{
    return Results.Ok(orderDetail.GetAll());
});
app.MapGet("/orderDetails/{id}", (IOrderDetail orderDetail, int id) =>
{
    var order = orderDetail.GetById(id);
    if(order == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(order);
});
app.MapGet("/orderHeader/{id}", (IOrderHeader orderHeader, int id) =>
{
    var header = orderHeader.GetById(id);
    if(header == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(header);
});

app.MapPost("/orderDetails", async (IOrderDetail orderDetail, IProductService productService, OrderDetail obj, IWalletService walletService, IOrderHeader IorderHeader) =>
{
    try
    {
        var products = await productService.GetProductById(obj.ProductId);
        var orderHeader = IorderHeader.GetById(obj.OrderHeaderId);
        Console.WriteLine($"Searching for wallet with userId: {orderHeader.userId}"); // Output userId for debugging purposes
        var userWallet = await walletService.GetUserWalletById(orderHeader.userId);
        if(products == null && userWallet == null)
        {
            return Results.BadRequest("Product or UserWallet not found");
        }
        if(products.quantity < obj.Quantity)
        {
            return Results.BadRequest("Stock not enough");
        }
        
        obj.Price = products.price * obj.Quantity;
        if (userWallet.saldo < obj.Price)
        {
            return Results.BadRequest("Saldo tidak mencukupi");
        }
        var order = orderDetail.Insert(obj);
        var productUpdateStockDTO = new ProductUpdateStockDTO
        {
            ProductId = obj.ProductId,
            Quantity =  obj.Quantity
        };
        var updateWalletStockDTO = new WalletUpdateSaldoDTO
        {
            userId = orderHeader.userId,
            saldo = obj.Price
        };
        Console.WriteLine($"Update Wallet: {updateWalletStockDTO.userId},{updateWalletStockDTO.saldo}"); // Output userId for debugging purposes
        await productService.UpdateProductStock(productUpdateStockDTO);
        await walletService.UpdateSaldoWallet(updateWalletStockDTO);

        return Results.Created($"/orderDetails/{order.OrderDetailId}", order);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/orderDetails", async (IOrderDetail orderDetail, IProductService productService, OrderDetail obj) =>
{
    try
    {
        var products = await productService.GetProductById(obj.ProductId);
        if (products == null)
        {
            return Results.BadRequest("Product not found");
        }
        if (products.quantity < obj.Quantity)
        {
            return Results.BadRequest("Stock not enough");
        }

        obj.Price = products.price * obj.Quantity;

        var updatedOrder = orderDetail.Update(obj);
        if (updatedOrder == null)
        {
            return Results.NotFound("Failed to update order");
        }

        return Results.Ok(updatedOrder);
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
