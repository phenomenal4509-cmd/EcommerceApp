using MyApp.Api.Data;
using MyApp.Api.Models;
using MyApp.Api.Dto;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MyApp.Api.EndPoints;

public static class OrderEndPoints
{
    public static void MapOrderEndPoints(this WebApplication app)
    {
        app.MapGet("/orders", async (HttpContext context, AppDbContex dbContext) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var orders = await dbContext.Orders
                .Include(o => o.OrderItems).Include(o => o.TrackingHistory)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            var details = new List<OrderDetailsDto>();
            foreach (var order in orders)
                details.Add(await MapOrderDetailsAsync(order, dbContext));

            return Results.Ok(details);
        }).RequireAuthorization();

        app.MapGet("/orders/{id}", async (int id, HttpContext context, AppDbContex dbContext) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var order = await dbContext.Orders
                .Include(o => o.OrderItems).Include(o => o.TrackingHistory)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == id);

            if (order is null)
                return Results.NotFound("No order with this Id exists.");

            return Results.Ok(await MapOrderDetailsAsync(order, dbContext));
        }).RequireAuthorization();

        app.MapPut("/orders/{id}", async (int id, AppDbContex dbContext, UpdateOrderDto updateOrder, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var existingOrder = await dbContext.Orders
                .Include(o => o.OrderItems).Include(o => o.TrackingHistory)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == id);

            if (existingOrder is null)
                return Results.NotFound("No such order exists.");

            if (existingOrder.CurrentStatus is "Shipped" or "Delivered" or "Cancelled")
                return Results.Conflict($"Cannot edit an order that is already '{existingOrder.CurrentStatus}'.");

            if (!updateOrder.items.Any())
                return Results.BadRequest("An order must have at least one item.");

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                dbContext.OrderItems.RemoveRange(existingOrder.OrderItems);
                decimal productSubtotal = 0;

                foreach (var item in updateOrder.items)
                {
                    var existingProduct = await dbContext.Products.FindAsync(item.productId);
                    if (existingProduct is null)
                    {
                        await transaction.RollbackAsync();
                        return Results.NotFound($"Product {item.productId} is not available.");
                    }
                    if (item.Quantity <= 0)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest($"Quantity must be at least 1 for product {existingProduct.Id}.");
                    }
                    if (item.Quantity >= 50)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest($"Quantity cannot exceed 49 for product {existingProduct.Id}.");
                    }

                    productSubtotal += existingProduct.Price * item.Quantity;
                    existingOrder.OrderItems.Add(new OrderItem
                    {
                        ProductId = existingProduct.Id,
                        Quantity = item.Quantity,
                        UnitPrice = existingProduct.Price,
                        DeliveryOptionId = 1,
                        EstimatedDeliveryAt = DateTimeOffset.UtcNow.AddDays(7)
                    });
                }

                existingOrder.ProductSubtotal = productSubtotal;
                existingOrder.ShippingCost = 0;
                existingOrder.TaxAmount = Math.Round(productSubtotal * 0.1m, 2, MidpointRounding.AwayFromZero);
                existingOrder.TotalCost = existingOrder.ProductSubtotal + existingOrder.TaxAmount;
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return Results.Ok(await MapOrderDetailsAsync(existingOrder, dbContext));
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }).RequireAuthorization();

        app.MapDelete("/orders/{id}", async (int id, HttpContext context, AppDbContex dbContext) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var existingOrder = await dbContext.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == id);

            if (existingOrder is null)
                return Results.NotFound("No such order exists.");

            if (existingOrder.CurrentStatus is "Shipped" or "Delivered")
                return Results.Conflict($"Cannot delete an order that is already '{existingOrder.CurrentStatus}'.");

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                dbContext.Orders.Remove(existingOrder);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return Results.NoContent();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }).RequireAuthorization();
    }

    private static async Task<OrderDetailsDto> MapOrderDetailsAsync(Order order, AppDbContex dbContext)
    {
        var itemDetails = new List<OrderItemDetailsDto>();
        foreach (var item in order.OrderItems)
        {
            var product = await dbContext.Products.FindAsync(item.ProductId);
            itemDetails.Add(new OrderItemDetailsDto(
                item.ProductId,
                item.Quantity,
                item.UnitPrice,
                product?.Name ?? "Unknown product",
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
