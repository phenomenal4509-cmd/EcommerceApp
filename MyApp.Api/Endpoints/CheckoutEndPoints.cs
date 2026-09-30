using MyApp.Api.Data;
using MyApp.Api.Dto;
using MyApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MyApp.Api.EndPoints;

public static class CheckOutEndPoints
{
    public static void MapCheckOutEndPoints(this WebApplication app)
    {
        app.MapPost("/checkout", async (HttpContext context, AppDbContex dbContext) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;


            var cart = await dbContext.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (cart is null || !cart.CartItems.Any())
                return Results.BadRequest("Your cart is empty.");

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                const string initialStatus = "Pending";
                decimal totalCost = 0;

                
                Order order = new()
                {
                    UserId      = userId,
                    OrderDate   = DateOnly.FromDateTime(DateTime.Now),
                    CurrentStatus = initialStatus
                };

                
                foreach (var cartItem in cart.CartItems)
                {
                    var product = await dbContext.Products.FindAsync(cartItem.ProductId);
                    if (product is null)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest($"Product {cartItem.ProductId} is no longer available.");
                    }

                    totalCost += product.Price * cartItem.Quantity;
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity  = cartItem.Quantity,
                        UnitPrice = product.Price
                    });
                }

                order.TotalCost = totalCost;
                dbContext.Orders.Add(order);
                await dbContext.SaveChangesAsync();

                
                Tracking tracking = new()
                {
                    OrderId         = order.Id,
                    Status          = initialStatus,
                    StatusChangedAt = DateTimeOffset.UtcNow
                };
                dbContext.Tracking.Add(tracking);

                
                dbContext.CartItem.RemoveRange(cart.CartItems);
                dbContext.Cart.Remove(cart);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                
                var orderDetails = new OrderDetailsDto(
                    order.Id,
                    order.TotalCost,
                    order.OrderDate,
                    order.CurrentStatus,
                    order.OrderItems.Select(i => new OrderItemDetailsDto(
                        i.ProductId, i.Quantity, i.UnitPrice)).ToList(),
                    [new TrackingHistoryDto(tracking.Status, tracking.StatusChangedAt)]
                );

                return Results.Created($"/orders/{order.Id}", orderDetails);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }).RequireAuthorization();
    }
}