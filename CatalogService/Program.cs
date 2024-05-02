using CatalogService.DAL;
using CatalogService.DAL.Interfaces;
using CatalogService.Models;
using CatalogServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategory, CategoryDAL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/categories", (ICategory categoryDAL) =>
{
    var categories = categoryDAL.GetAll();
    return Results.Ok(categories);
});

app.MapGet("/api/categories/{id}", (ICategory categoryDAL,int id) =>
{
   var category = categoryDAL.GetById(id);
   if (category == null)
   {
        return Results.NotFound();
   }
   return Results.Ok(category);
});
app.MapGet("/api/categories/search/{name}", (ICategory categoryDAL,string name) =>
{
   var category = categoryDAL.GetByName(name);
   if (category == null)
   {
        return Results.NotFound();
   }
   return Results.Ok(category);
});

app.MapPost("api/categories",(ICategory CategoryDAL, Category category) =>
{
    try
    {
        CategoryDAL.Insert(category);
        return Results.Created($"/api/categories/{category.CategoryID}",category);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("api/categories",(ICategory CategoryDAL, Category category) =>
{
    try
    {
        CategoryDAL.Update(category);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("api/categories/{id}",(ICategory CategoryDAL, int id) =>
{
    try
    {
        CategoryDAL.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


//Contoh penggunaan query string parameter
// /api/products/getbyname?name=Apple
// app.MapGet("api/products/getbyname",(HttpRequest request) => 
// {
//     var name = request.Query["name"].ToString();
//     var results = products.Where(p => p.Name.ToLower().Contains(name.ToLower()));
//     return Results.Ok(name);
// });

// app.MapGet("api/products/getbyname", (string name) => 
// {
//     var result = products.Where(p => p.Name.ToLower().Contains(name.ToLower()));
//     return Results.Ok(result);
// });
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
