using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.Dto;
using MyApp.Api.Models;

namespace MyApp.Api.EndPoints;

public static class CartEndPoints
{
    public static async Task<CartDetailsDto?> GetDetails(Cart existingCart, AppDbContex dbContext)
    {
        ICollection<CartItemDetailsDto> products = new List<CartItemDetailsDto>();
        decimal totalCost = 0;

        foreach (var item in existingCart.CartItems)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
            if (product is null) return null;

            totalCost += product.Price * item.Quantity;
            products.Add(new CartItemDetailsDto(
                product.Id, product.Name, product.Price, product.Description, product.Image, item.Quantity, item.DeliveryOptionId));
        }

        return new CartDetailsDto(existingCart.Id, totalCost, products);
    }

    public static void MapCartEndPoints(this WebApplication app)
    {

        app.MapGet("/cart", async (AppDbContex dbContext, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var existingCart = await dbContext.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (existingCart is null)
                return Results.NotFound("You do not have a cart yet.");

            var cartDetails = await GetDetails(existingCart, dbContext);
            if (cartDetails is null)
                return Results.NotFound("A product in your cart no longer exists.");

            return Results.Ok(cartDetails);
        }).RequireAuthorization();



        app.MapPost("/cart", async (CreateCartDto cart, AppDbContex dbContext, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var existingUserCart = await dbContext.Cart.FirstOrDefaultAsync(c => c.UserID == userId);
            if (existingUserCart is not null)
                return Results.Conflict("You already have a cart. Use PUT /cart to update it.");

            if (!cart.items.Any())
                return Results.BadRequest("Cart must have at least one item.");

            var duplicateProduct = cart.items.GroupBy(i => i.ProductId).FirstOrDefault(g => g.Count() > 1);
            if (duplicateProduct is not null)
                return Results.BadRequest($"Product {duplicateProduct.Key} appears more than once. Adjust the quantity instead.");

            Cart newCart = new() { UserID = userId };

            foreach (var item in cart.items)
            {
                if (item.Quantity <= 0)
                    return Results.BadRequest($"Quantity must be at least 1 for product {item.ProductId}.");
                if (item.Quantity > 50)
                    return Results.BadRequest($"Quantity cannot exceed 50 for product {item.ProductId}.");

                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product is null)
                    return Results.NotFound($"Product {item.ProductId} does not exist.");

                var deliveryOption = await dbContext.DeliveryOptions.FindAsync(item.DeliveryOptionId);
                if (deliveryOption is null)
                    return Results.BadRequest($"Delivery option {item.DeliveryOptionId} is invalid.");

                newCart.CartItems.Add(new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    DeliveryOptionId = item.DeliveryOptionId
                });
            }

            dbContext.Cart.Add(newCart);
            await dbContext.SaveChangesAsync();

            var cartDetails = await GetDetails(newCart, dbContext);
            if (cartDetails is null)
                return Results.NotFound("A product in your cart no longer exists.");

            return Results.Created($"/cart/{newCart.Id}", cartDetails);
        }).RequireAuthorization();



        app.MapPut("/cart", async (UpdateCartDto update, HttpContext context, AppDbContex dbContext) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var existingCart = await dbContext.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (existingCart is null)
                return Results.NotFound("You do not have a cart yet.");

            if (!update.items.Any())
                return Results.BadRequest("Cart must have at least one item.");

            var duplicateProduct = update.items.GroupBy(i => i.ProductId).FirstOrDefault(g => g.Count() > 1);
            if (duplicateProduct is not null)
                return Results.BadRequest($"Product {duplicateProduct.Key} appears more than once. Adjust the quantity instead.");

            foreach (var item in update.items)
            {
                if (item.Quantity <= 0)
                    return Results.BadRequest($"Quantity must be at least 1 for product {item.ProductId}.");
                if (item.Quantity > 50)
                    return Results.BadRequest($"Quantity cannot exceed 50 for product {item.ProductId}.");

                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product is null)
                    return Results.NotFound($"Product {item.ProductId} does not exist.");

                var deliveryOption = await dbContext.DeliveryOptions.FindAsync(item.DeliveryOptionId);
                if (deliveryOption is null)
                    return Results.BadRequest($"Delivery option {item.DeliveryOptionId} is invalid.");
            }

            dbContext.CartItem.RemoveRange(existingCart.CartItems);

            foreach (var item in update.items)
            {
                existingCart.CartItems.Add(new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    DeliveryOptionId = item.DeliveryOptionId
                });
            }

            await dbContext.SaveChangesAsync();

            var cartDetails = await GetDetails(existingCart, dbContext);
            if (cartDetails is null)
                return Results.NotFound("A product in your cart no longer exists.");

            return Results.Ok(cartDetails);
        }).RequireAuthorization();



        app.MapDelete("/cart", async (HttpContext context, AppDbContex dbContext) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var existingCart = await dbContext.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (existingCart is null)
                return Results.NotFound("You do not have a cart.");

            dbContext.CartItem.RemoveRange(existingCart.CartItems);
            dbContext.Cart.Remove(existingCart);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization();
    }
}