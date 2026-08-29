using MyApp.Api.Data;
using MyApp.Api.Models;
using MyApp.Api.Dto;
namespace MyApp.Api.EndPoints;
public static class ProductEndPoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/product", (AppDbContex dbContext) =>
        {
            return dbContext.Products.ToList() ;
        });
        app.MapGet("/product/{id}",(int id ,AppDbContex dbContext) =>
        {
            var exisitingGame = dbContext.Products.Find(id);

            if (exisitingGame is null)
            {
                return Results.NotFound();
            }
            ProductDto product= new(exisitingGame.Name,exisitingGame.Price,exisitingGame.Description);
            return Results.Ok(product);
        });
        app.MapPost("/product", (CreateProductDto newProduct,AppDbContex dbContext) =>
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
            
            return Results.Created($"/product/{product.Id}",productDetails);
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapPut("/product/{id}", (int id ,UpdateProductDto product,AppDbContex dbContext) =>
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
        app.MapDelete("/product/{id}",(int id,AppDbContex dbContext) =>
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