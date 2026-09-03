using MyApp.Api.Data;
using MyApp.Api.Models;
using MyApp.Api.Dto;
using Microsoft.EntityFrameworkCore;
namespace MyApp.Api.EndPoints;
public static class ProductEndPoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products", async (AppDbContex dbContext) =>
        {
            return await dbContext.Products.ToListAsync();
        });
        app.MapGet("/products/{id}", async (int id ,AppDbContex dbContext) =>
        {
            var exisitingProduct = await dbContext.Products.FindAsync(id);

            if (exisitingProduct is null)
            {
                return Results.NotFound();
            }
            ProductDto product= new(exisitingProduct.Name,exisitingProduct.Price,exisitingProduct.Description);
            return Results.Ok(product);
        });
        app.MapPost("/products", async (CreateProductDto newProduct, AppDbContex dbContext) =>
        {
            Product product = new()
            {
                Name=newProduct.Name,
                Price=newProduct.Price,
                Description=newProduct.Description

            };
            
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            ProductDetailsDto productDetails = new(product.Id, product.Name, product.Price, product.Description);
            
            return Results.Created($"/products/{product.Id}", productDetails);
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapPut("/products/{id}", async (int id ,UpdateProductDto product,AppDbContex dbContext) =>
        {
            var exisitingProduct = await dbContext.Products.FindAsync(id);

            if (exisitingProduct is null)
            {
                return Results.NotFound();
            }
            exisitingProduct.Name=product.Name;
            exisitingProduct.Price=product.Price;
            exisitingProduct.Description=product.Description;
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapDelete("/products/{id}", async (int id, AppDbContex dbContext) =>
        {
            var exisitingProduct = await dbContext.Products.FindAsync(id);
            if (exisitingProduct is null)
            {
                return Results.NotFound();
            }
            dbContext.Products.Remove(exisitingProduct);
            await dbContext.SaveChangesAsync();   
            return Results.NoContent();         
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });

    }
}