using MyApp.Api.Data;
using MyApp.Api.Models;
using MyApp.Api.Dto;
namespace MyApp.Api.EndPoints;
public static class ProductEndPoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products", (AppDbContex dbContext) =>
        {
            return dbContext.Products.ToList() ;
        });
        app.MapGet("/products/{id}",(int id ,AppDbContex dbContext) =>
        {
            var exisitingGame = dbContext.Products.Find(id);

            if (exisitingGame is null)
            {
                return Results.NotFound();
            }
            ProductDto product= new(exisitingGame.Name,exisitingGame.Price,exisitingGame.Description);
            return Results.Ok(product);
        });
        app.MapPost("/products", (CreateProductDto newProduct,AppDbContex dbContext) =>
        {
            Product product = new()
            {
                Name=newProduct.Name,
                Price=newProduct.Price,
                Description=newProduct.Description

            };
            
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            ProductDetailsDto productDetails= new (product.Id,product.Name,product.Price,product.Description);
            
            return Results.Created($"/products/{product.Id}",productDetails);
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapPut("/products/{id}", (int id ,UpdateProductDto product,AppDbContex dbContext) =>
        {
            var exisitingGame = dbContext.Products.Find(id);

            if (exisitingGame is null)
            {
                return Results.NotFound();
            }
            exisitingGame.Name=product.Name;
            exisitingGame.Price=product.Price;
            exisitingGame.Description=product.Description;
            dbContext.SaveChanges();
            return Results.NoContent();
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapDelete("/products/{id}",(int id,AppDbContex dbContext) =>
        {
            var exisitingGame = dbContext.Products.Find(id);
            if (exisitingGame is null)
            {
                return Results.NotFound();
            }
            dbContext.Products.Remove(exisitingGame);
            dbContext.SaveChanges();   
            return Results.NoContent();         
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });

    }
}