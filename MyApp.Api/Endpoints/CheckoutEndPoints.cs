using MyApp.Api.Data;
using MyApp.Api.Dto;
using MyApp.Api.Models;
using MyApp.Api.Services;
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
                var (paymentSummary, pricingError) = await CartPricing.BuildPaymentSummaryAsync(cart, dbContext);
                if (pricingError is not null)
                {
                    await transaction.RollbackAsync();
                    return Results.BadRequest(pricingError);
                }

                static decimal FromCents(int cents) => cents / 100m;

                Order order = new()
                {
                    UserId = userId,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    CurrentStatus = initialStatus,
                    ProductSubtotal = FromCents(paymentSummary!.ProductCostCents),
                    ShippingCost = FromCents(paymentSummary.ShippingCostCents),
                    TaxAmount = FromCents(paymentSummary.TaxCents),
                    TotalCost = FromCents(paymentSummary.TotalCostCents)
                };

                foreach (var cartItem in cart.CartItems)
                {
                    var product = await dbContext.Products.FindAsync(cartItem.ProductId);
                    if (product is null)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest($"Product {cartItem.ProductId} is no longer available.");
                    }

                    var deliveryOption = await dbContext.DeliveryOptions.FindAsync(cartItem.DeliveryOptionId);
                    if (deliveryOption is null)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest($"Delivery option {cartItem.DeliveryOptionId} is invalid.");
                    }

                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = cartItem.Quantity,
                        UnitPrice = product.Price,
                        DeliveryOptionId = deliveryOption.Id,
                        EstimatedDeliveryAt = DateTimeOffset.UtcNow.AddDays(deliveryOption.DeliveryDays)
                    });
                }

                dbContext.Orders.Add(order);
                await dbContext.SaveChangesAsync();

                Tracking tracking = new()
                {
                    OrderId = order.Id,
                    Status = initialStatus,
                    StatusChangedAt = DateTimeOffset.UtcNow
                };
                dbContext.Tracking.Add(tracking);

                dbContext.CartItem.RemoveRange(cart.CartItems);
                dbContext.Cart.Remove(cart);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                var orderDetails = await BuildOrderDetailsAsync(order.Id, dbContext);
                return Results.Created($"/orders/{order.Id}", orderDetails);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }).RequireAuthorization();
    }

    internal static async Task<OrderDetailsDto?> BuildOrderDetailsAsync(int orderId, AppDbContex dbContext)
    {
        var order = await dbContext.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.TrackingHistory)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
            return null;

        var itemDetails = new List<OrderItemDetailsDto>();
        foreach (var item in order.OrderItems)
        {
            var product = await dbContext.Products.FindAsync(item.ProductId);
            itemDetails.Add(new OrderItemDetailsDto(
                item.ProductId,
                item.Quantity,
                item.UnitPrice,
                product?.Name ?? "Unknown product",
                product?.Image ?? "",
                item.DeliveryOptionId,
                item.EstimatedDeliveryAt));
        }

        var trackingHistory = order.TrackingHistory
            .Select(t => new TrackingHistoryDto(t.Status, t.StatusChangedAt))
            .ToList();

        return new OrderDetailsDto(
            order.Id,
            order.TotalCost,
            order.OrderDate,
            order.CurrentStatus,
            itemDetails,
            trackingHistory);
    }
}
