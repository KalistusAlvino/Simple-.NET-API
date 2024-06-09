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

app.MapGet("/api/wallet", (IWallet walletDapper) =>
{
    List<WalletDTO> walletDTO = new List<WalletDTO>();
    var wallet = walletDapper.GetAll();
    foreach (var wal in wallet)
    {
        walletDTO.Add(new WalletDTO
        {
            username = wal.username,
            saldo = wal.saldo
        });
    }
    return Results.Ok(walletDTO);
});
app.MapGet("/api/wallet/{username}", (IWallet walletDapper, string username) =>
{
    List<WalletDTO> walletDTO = new List<WalletDTO>();
    var wallet = walletDapper.GetByUsername(username);
    foreach (var wal in wallet)
    {
        walletDTO.Add(new WalletDTO
        {
            username = wal.username,
            saldo = wal.saldo
        });
    }
    return Results.Ok(walletDTO);
});

app.MapPost("/api/wallet", (IWallet walletDapper, WalletCreateDTO walletCreateDTO) =>
{
    try
    {
        Wallet wallet = new Wallet
        {
            username = walletCreateDTO.username,
            saldo = walletCreateDTO.saldo
        };
        walletDapper.Insert(wallet);

        //return 201 Created
        return Results.Created($"/api/wallet/{wallet.username}", wallet);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/wallet/bayar", (IWallet walletDapper, WalletUpdateSaldoDTO walletUpdateSaldoDTO) =>
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
app.MapPut("/api/wallet/topup", (IWallet walletDapper, WalletUpdateSaldoDTO walletUpdateSaldoDTO) =>
{
    try
    {
        walletDapper.UpdateSaldoAfterTopUp(walletUpdateSaldoDTO);
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
