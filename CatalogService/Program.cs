using CatalogService;
using CatalogService.DAL;
using CatalogService.DAL.Interfaces;
using CatalogService.DTO;
using CatalogService.Models;
using CatalogServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategory, CategoryDapper>();
builder.Services.AddScoped<IProduct, ProductDapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/categories", (ICategory categoryDal) =>
{
    List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    var categories = categoryDal.GetAll();
    foreach (var category in categories)
    {
        categoriesDto.Add(new CategoryDTO
        {
            CategoryName = category.CategoryName
        });
    }
    return Results.Ok(categoriesDto);
});

app.MapGet("/api/categories/{id}", (ICategory categoryDal, int id) =>
{
    CategoryDTO categoryDto = new CategoryDTO();
    var category = categoryDal.GetById(id);
    if (category == null)
    {
        return Results.NotFound();
    }
    categoryDto.CategoryName = category.CategoryName;
    return Results.Ok(categoryDto);
});

app.MapGet("/api/categories/search/{name}", (ICategory categoryDal, string name) =>
{
    List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    var categories = categoryDal.GetByName(name);
    foreach (var category in categories)
    {
        categoriesDto.Add(new CategoryDTO
        {
            CategoryName = category.CategoryName
        });
    }
    return Results.Ok(categoriesDto);
});

app.MapPost("/api/categories", (ICategory categoryDal, CategoryCreateDTO categoryCreateDto) =>
{
    try
    {
        Category category = new Category
        {
            CategoryName = categoryCreateDto.CategoryName
        };
        categoryDal.Insert(category);

        //return 201 Created
        return Results.Created($"/api/categories/{category.CategoryID}", category);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/categories", (ICategory categoryDal, CategoryUpdateDTO categoryUpdateDto) =>
{
    try
    {
        var category = new Category
        {
            CategoryID = categoryUpdateDto.CategoryID,
            CategoryName = categoryUpdateDto.CategoryName
        };
        categoryDal.Update(category);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/api/categories/{id}", (ICategory categoryDal, int id) =>
{
    try
    {
        categoryDal.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/products", (IProduct productDapper) =>
{
    List<ProductDTO> productDTO = new List<ProductDTO>();
    var products = productDapper.GetAll();
    foreach (var product in products)
    {
        productDTO.Add(new ProductDTO
        {
            Name = product.Name,
            CategoryName = product.CategoryName,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity

        });
    }
    return Results.Ok(productDTO);
});

app.MapGet("/api/products/{id}", (IProduct productDapper, int id) =>
{
    ProductDTO productDTO = new ProductDTO();
    var product = productDapper.GetById(id);
    if (product == null)
    {
        return Results.NotFound();
    }
    productDTO.Name = product.Name;
    productDTO.CategoryName = product.CategoryName;
    productDTO.Description = product.Description;
    productDTO.Price = product.Price;
    productDTO.Quantity = product.Quantity;
    return Results.Ok(productDTO);
});
app.MapGet("/api/productName/{name}", (IProduct productDapper, string name) =>
{
    List<ProductDTO> productDTO = new List<ProductDTO>();
    var products = productDapper.GetByName(name);
    foreach (var product in products)
    {
        productDTO.Add(new ProductDTO
        {
            Name = product.Name,
            CategoryName = product.CategoryName,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity

        });
    }
    return Results.Ok(productDTO);
});

app.MapPost("/api/products", (IProduct productDapper, ProductCreateDTO productCreateDTO) =>
{
    try
    {
        Product product = new Product
        {
            CategoryID = productCreateDTO.CategoryID,
            Name = productCreateDTO.Name,
            Description = productCreateDTO.Description,
            Price = productCreateDTO.Price,
            Quantity = productCreateDTO.Quantity
        };
        productDapper.Insert(product);

        //return 201 Created
        return Results.Created($"/api/products/{product.ProductID}", product);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/products", (IProduct productDapper, ProductUpdateDTO productUpdateDTO) =>
{
    try
    {
        var product = new Product
        {
            ProductID = productUpdateDTO.ProductID,
            CategoryID = productUpdateDTO.CategoryID,
            Name = productUpdateDTO.Name,
            Description = productUpdateDTO.Description,
            Price = productUpdateDTO.Price,
            Quantity = productUpdateDTO.Quantity
        };
        productDapper.Update(product);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/api/products/{id}", (IProduct productDapper, int id) =>
{
    try
    {
        productDapper.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

//update product quantity after order
app.MapPut("/api/products/updatestock", (IProduct productDapper, ProductUpdateStockDTO productUpdateStockDTO) =>
{
    try
    {
        productDapper.UpdateStockAfterOrder(productUpdateStockDTO);
        return Results.Ok();
    }
    catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.Run();