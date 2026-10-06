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
        app.MapGet("/products", async (
            string? search,
            string? name,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy,
            string? sortDirection,
            int? page,
            int? pageSize,
            AppDbContex dbContext) =>
        {
            if (minPrice is < 0 || maxPrice is < 0)
                return Results.BadRequest("minPrice and maxPrice cannot be negative.");

            if (minPrice is not null && maxPrice is not null && minPrice.Value > maxPrice.Value)
                return Results.BadRequest("minPrice cannot be greater than maxPrice.");

            var products = dbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                products = products.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.Description.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var nameTerm = name.Trim().ToLower();
                products = products.Where(p => p.Name.ToLower().Contains(nameTerm));
            }

            if (minPrice is not null)
            {
                var lowerBound = minPrice.Value;
                products = products.Where(p => p.Price >= lowerBound);
            }

            if (maxPrice is not null)
            {
                var upperBound = maxPrice.Value;
                products = products.Where(p => p.Price <= upperBound);
            }

            var direction = sortDirection?.Trim();
            var descending = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(direction, "descending", StringComparison.OrdinalIgnoreCase);

            var sort = sortBy?.Trim().ToLowerInvariant();
            products = sort switch
            {
                "name" => descending
                    ? products.OrderByDescending(p => p.Name).ThenBy(p => p.Id)
                    : products.OrderBy(p => p.Name).ThenBy(p => p.Id),
                "price" => descending
                    ? products.OrderByDescending(p => p.Price).ThenBy(p => p.Id)
                    : products.OrderBy(p => p.Price).ThenBy(p => p.Id),
                _ => descending
                    ? products.OrderByDescending(p => p.Id)
                    : products.OrderBy(p => p.Id)
            };

            var currentPage = page is > 0 ? page.Value : 1;
            var size = pageSize is > 0 and <= 100 ? pageSize.Value : 10;

            var totalItems = await products.CountAsync();
            var result = await products
                .Skip((currentPage - 1) * size)
                .Take(size)
                .ToListAsync();

            var items = result.Select(p =>
                new ProductDetailsDto(p.Id, p.Name, p.Price, p.Description, p.Image, p.Version)).ToList();

            return Results.Ok(new PagedResult<ProductDetailsDto>(
                items,
                currentPage,
                size,
                totalItems,
                (int)Math.Ceiling(totalItems / (double)size)));
        });
        app.MapGet("/products/{id}", async (int id ,AppDbContex dbContext) =>
        {
            var exisitingProduct = await dbContext.Products.FindAsync(id);

            if (exisitingProduct is null)
            {
                return Results.NotFound();
            }
            ProductDetailsDto product = new(
                exisitingProduct.Id,
                exisitingProduct.Name,
                exisitingProduct.Price,
                exisitingProduct.Description,
                exisitingProduct.Image,
                exisitingProduct.Version);
            return Results.Ok(product);
        });
        app.MapPost("/products", async (CreateProductDto newProduct, AppDbContex dbContext) =>
        {
            Product product = new()
            {
                Name=newProduct.Name,
                Price=newProduct.Price,
                Description=newProduct.Description,
                Image=newProduct.Image,
            };  
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            ProductDetailsDto productDetails = new(product.Id, product.Name, product.Price, product.Description, product.Image, product.Version);
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
            exisitingProduct.Image=product.Image;
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
