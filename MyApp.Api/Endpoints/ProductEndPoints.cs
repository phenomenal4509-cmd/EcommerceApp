using MyApp.Api.Data;
using MyApp.Api.Models;
using MyApp.Api.Dto;
namespace MyApp.Api.EndPoints;
public static class ProductEndPoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/product", () =>
        {
            return SeedData.Products ;
        });
        app.MapPost("/product", (CreateProductDto product) =>
        {
            int Id = SeedData.Products.Count+1;
            Product newProduct = new()
            {
                Id = Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
            SeedData.Products.Add(newProduct);
            return Results.Created($"/prducts/{Id}",newProduct);
        });
        app.MapPut("/product/{id}", (int id ,UpdateProductDto product) =>
        {
            var index = SeedData.Products.FindIndex(p => p.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            SeedData.Products[index] = new Product
            {
                Id = id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
            return Results.NoContent();
        });
        app.MapDelete("/product/{id}",(int id) =>
        {
            var index = SeedData.Products.FindIndex(p => p.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }
            SeedData.Products.RemoveAt(index);
            return Results.NoContent();
        });
        
    }
}