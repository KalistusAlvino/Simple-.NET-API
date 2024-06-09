using CustomerServices.DAL;
using CustomerServices.DTO;
using CustomerServices.Models;
using CustomerServices.Services;
using DAL.Interface;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomer, CustomerDapper>();
builder.Services.AddHttpClient<IWalletServices, WalletService>()
.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/user", (ICustomer cus) =>
{
    List<CustomerDTO> customerDTOs = new List<CustomerDTO>();
    var customer = cus.GetAll();
    foreach (var cust in customer)
    {
        customerDTOs.Add(new CustomerDTO
        {
            username = cust.username,
            password = cust.password,
            fullname = cust.fullname
        });
    }
    return Results.Ok(customerDTOs);
});

app.MapGet("/api/user/{username}", (ICustomer cus, string username) =>
{
    List<CustomerDTO> customerDTOs = new List<CustomerDTO>();
    var customers = cus.GetByUsername(username);
    foreach (var cust in customers)
    {
        customerDTOs.Add(new CustomerDTO
        {
            username = cust.username,
            password = cust.password,
            fullname = cust.fullname
        });
    }
    return Results.Ok(customerDTOs);
});

app.MapPost("/api/register", async (ICustomer iCust, CustomerDTO customerDTO, IWalletServices iWalletService) =>
{
    try
    {
        Customer customer = new Customer
        {
            username = customerDTO.username,
            password = customerDTO.password,
            fullname = customerDTO.fullname
        };
        iCust.Register(customer);
        var createWalletDTO = new CreateWalletDTO
        {
            username = customer.username,
            saldo = 0
        };
        await iWalletService.PostWalletWhenRegister(createWalletDTO);
        return Results.Created($"/api/wallet/{customer.username}", customer);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.Run();
