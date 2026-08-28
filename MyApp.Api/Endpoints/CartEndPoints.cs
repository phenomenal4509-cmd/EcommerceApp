using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.Dto;
using MyApp.Api.Models;

namespace MyApp.Api.EndPoints;

public static class CartEndPoints
{
    public static CartDetailsDto? GetDetails(
        Cart existingCart,
        AppDbContex dbContext)
    {
        ICollection<CartItemDetailsDto> products = new List<CartItemDetailsDto>();
        decimal totalCost = 0;

        foreach (var item in existingCart.CartItems)
        {
            var product = dbContext.Products
                .FirstOrDefault(p => p.Id == item.ProductId);

            if (product is null)
            {
                return null;
            }

            decimal cost = product.Price * item.Quantity;
            totalCost += cost;

            CartItemDetailsDto newItem = new(
                product.Id,
                product.Name,
                product.Price,
                product.Description,
                item.Quantity
            );

            products.Add(newItem);
        }

        return new CartDetailsDto(existingCart.Id,totalCost,products);
    }

    public static void MapCartEndPoints(this WebApplication app)
    {
        app.MapGet("/cart", (AppDbContex dbContext,HttpContext context) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
            var existingCart = dbContext.Cart
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserID == userId);

            if (existingCart is null)
            {
                return Results.NotFound("No such cart exists");
            }

            var cartDetails = GetDetails(existingCart, dbContext);

            if (cartDetails is null)
            {
                return Results.NotFound("A product in this cart no longer exists");
            }

            return Results.Ok(cartDetails);
        }).RequireAuthorization();

        app.MapPost("/cart", (CreateCartDto cart, AppDbContex dbContext,HttpContext context) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
            Cart newCart = new();
            newCart.UserID=userId;
            foreach (var item in cart.items)
            {
                var product = dbContext.Products.FirstOrDefault(p => p.Id == item.ProductId);

                if (product is null)
                {
                    return Results.NotFound("No such product exists");
                }

                CartItem newItem = new()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                newCart.CartItems.Add(newItem);
            }

            dbContext.Cart.Add(newCart);
            dbContext.SaveChanges();

            var cartDetails = GetDetails(newCart, dbContext);

            if (cartDetails is null)
            {
                return Results.NotFound("A product in this cart no longer exists");
            }

            return Results.Created($"/cart/{newCart.Id}", cartDetails);
        }).RequireAuthorization();

        app.MapPut("/cart",(UpdateCartDto update, HttpContext context, AppDbContex dbContext) =>
            {
                var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
                var existingCart = dbContext.Cart.Include(c => c.CartItems).FirstOrDefault(c => c.UserID == userId);

                if (existingCart is null)
                {
                    return Results.NotFound("No such cart exists");
                }

                foreach (var item in update.items)
                {
                    var product = dbContext.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product is null)
                    {
                        return Results.NotFound($"Product with Id {item.ProductId} does not exist");
                    }
                }

                dbContext.CartItem.RemoveRange(existingCart.CartItems);

                foreach (var item in update.items)
                {
                    CartItem newItem = new()
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    };

                    existingCart.CartItems.Add(newItem);
                }

                dbContext.SaveChanges();

                var cartDetails = GetDetails(existingCart, dbContext);

                if (cartDetails is null)
                {
                    return Results.NotFound("A product in this cart no longer exists");
                }

                return Results.Ok(cartDetails);
            }).RequireAuthorization();

        app.MapDelete("/cart", (HttpContext context, AppDbContex dbContext) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
            var existingCart = dbContext.Cart.Include(c => c.CartItems).FirstOrDefault(c => c.UserID == userId);
            if (existingCart is null)
            {
                return Results.NotFound("No such cart exists");
            }
            dbContext.CartItem.RemoveRange(existingCart.CartItems);
            dbContext.Cart.Remove(existingCart);

            dbContext.SaveChanges();

            return Results.NoContent();
        }).RequireAuthorization();
    }
}