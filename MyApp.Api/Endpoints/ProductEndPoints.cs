using MyApp.Api.Data;
using MyApp.Api.Models;
using MyApp.Api.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
namespace MyApp.Api.EndPoints;
public static class ProductEndPoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products", async (AppDbContex dbContext) =>
        {
            var result= await dbContext.Products.OrderBy(p => p.Id).ToListAsync();
            var products= result.Select(p=>new ProductDto(p.Name,p.Price,p.Description,p.Version)).ToList();
            return Results.Ok(products);
        });
        app.MapGet("/products/{id}", async (int id ,AppDbContex dbContext) =>
        {
            var exisitingProduct = await dbContext.Products.FindAsync(id);

            if (exisitingProduct is null)
            {
                return Results.NotFound();
            }
            ProductDto product= new(exisitingProduct.Name,exisitingProduct.Price,exisitingProduct.Description,exisitingProduct.Version);
            return Results.Ok(product);
        });
        app.MapPost("/products", async (CreateProductDto newProduct, AppDbContex dbContext) =>
        {
            Product product = new()
            {
                Name=newProduct.Name,
                Price=newProduct.Price,
                Description=newProduct.Description,
            };  
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            ProductDetailsDto productDetails = new(product.Id, product.Name, product.Price, product.Description,product.Version);
            return Results.Created($"/products/{product.Id}", productDetails);
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapPut("/products/{id}", async (int id ,UpdateProductDto product,AppDbContex dbContext) =>
        {
            try { 
            var exisitingProduct = await dbContext.Products.FindAsync(id);
            if (exisitingProduct is null)
            {
                return Results.NotFound();
            }
            dbContext.Entry(exisitingProduct).Property(p=>p.Version).OriginalValue=product.Version;
            exisitingProduct.Name=product.Name;
            exisitingProduct.Price=product.Price;
            exisitingProduct.Description=product.Description;
            exisitingProduct.Version=Guid.NewGuid();
            await dbContext.SaveChangesAsync();
            return Results.NoContent();}
            catch (DbUpdateConcurrencyException)
            {
                return Results.Conflict("The product was changed by another user. Please reload it and try again.");
            }
            
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });
        app.MapDelete("/products/{id}", async (int id, AppDbContex dbContext,[FromBody]DeleteProductDto request) =>
        {
            try
            {
            var exisitingProduct = await dbContext.Products.FindAsync(id);
            if (exisitingProduct is null)
            {
                return Results.NotFound();
            }
            dbContext.Entry(exisitingProduct).Property(p=>p.Version).OriginalValue=request.Version;
            dbContext.Products.Remove(exisitingProduct);
            await dbContext.SaveChangesAsync();   
            return Results.NoContent();        
            }
            catch (DbUpdateConcurrencyException)
            {
                return Results.Conflict("The product was changed by another user. Please reload it and try again.");
            }      
        }).RequireAuthorization((policy) =>
        {
            policy.RequireRole("Admin");
        });

    }
}