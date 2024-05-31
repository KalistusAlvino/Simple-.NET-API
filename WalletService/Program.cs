using WalletService.DAL.Interface;
using WalletService.DTO;
using WalletService.Interface;
using WalletService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IWallet, WalletDapper>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/wallet/{id}", (IWallet walletDapper, int id) =>
{
    WalletDTO walletDTO = new WalletDTO();
    var wallet = walletDapper.GetById(id);
    if (wallet == null)
    {
        return Results.NotFound();
    }
    walletDTO.username = wallet.username;
    walletDTO.password = wallet.password;
    walletDTO.fullname = wallet.fullname;
    walletDTO.saldo = wallet.saldo;
    return Results.Ok(walletDTO);
});

app.MapPost("/api/wallet", (IWallet walletDapper, WalletCreateDTO walletCreateDTO) =>
{
    try
    {
        Wallet wallet = new Wallet
        {
            username = walletCreateDTO.username,
            password = walletCreateDTO.password,
            fullname = walletCreateDTO.fullname,
            saldo = walletCreateDTO.saldo
        };
        walletDapper.Insert(wallet);

        //return 201 Created
        return Results.Created($"/api/wallet/{wallet.userId}", wallet);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/wallet/updatesaldo", (IWallet walletDapper, WalletUpdateSaldoDTO walletUpdateSaldoDTO) =>
{
    try
    {
        walletDapper.UpdateSaldoAfterOrder(walletUpdateSaldoDTO);
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
