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
                .ToListAsync();

            var details = orders.Select(order => new OrderDetailsDto(
                order.Id,
                order.TotalCost,
                order.OrderDate,
                order.CurrentStatus,
                order.OrderItems.Select(item => new OrderItemDetailsDto(
                    item.ProductId, item.Quantity, item.UnitPrice)).ToList(),
                order.TrackingHistory.Select(t => new TrackingHistoryDto(
                    t.Status, t.StatusChangedAt)).ToList()
            )).ToList();

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

            var itemDetails = order.OrderItems
                .Select(item => new OrderItemDetailsDto(item.ProductId, item.Quantity, item.UnitPrice))
                .ToList();

            var trackingHistory = order.TrackingHistory
                .Select(t => new TrackingHistoryDto(t.Status, t.StatusChangedAt))
                .ToList();

            return Results.Ok(new OrderDetailsDto(
                order.Id, order.TotalCost, order.OrderDate,
                order.CurrentStatus, itemDetails, trackingHistory));
        }).RequireAuthorization();



        app.MapPost("/orders", async (CreateOrderDto newOrder, AppDbContex dbContext, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (!newOrder.items.Any())
                return Results.BadRequest("An order must have at least one item.");

            decimal totalCost = 0;
            const string status = "Pending";

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                Order order = new()
                {
                    UserId = userId,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    CurrentStatus = status
                };

                foreach (var item in newOrder.items)
                {
                    var existingProduct = await dbContext.Products.FindAsync(item.ProductId);
                    if (existingProduct is null)
                    {
                        await transaction.RollbackAsync();
                        return Results.NotFound($"Product {item.ProductId} is not available.");
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

                    totalCost += existingProduct.Price * item.Quantity;
                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = existingProduct.Id,
                        Quantity = item.Quantity,
                        UnitPrice = existingProduct.Price
                    });
                }

                order.TotalCost = totalCost;
                dbContext.Orders.Add(order);
                await dbContext.SaveChangesAsync();

                Tracking tracking = new()
                {
                    OrderId = order.Id,
                    Status = status,
                    StatusChangedAt = DateTimeOffset.UtcNow
                };
                dbContext.Tracking.Add(tracking);
                await dbContext.SaveChangesAsync();

                var orderDetails = new OrderDetailsDto(
                    order.Id, order.TotalCost, order.OrderDate, order.CurrentStatus,
                    order.OrderItems.Select(i => new OrderItemDetailsDto(i.ProductId, i.Quantity, i.UnitPrice)).ToList(),
                    [new TrackingHistoryDto(tracking.Status, tracking.StatusChangedAt)]
                );

                await transaction.CommitAsync();
                return Results.Created($"/orders/{order.Id}", orderDetails);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
                decimal totalCost = 0;

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

                    totalCost += existingProduct.Price * item.Quantity;
                    existingOrder.OrderItems.Add(new OrderItem
                    {
                        ProductId = existingProduct.Id,
                        Quantity = item.Quantity,
                        UnitPrice = existingProduct.Price
                    });
                }

                existingOrder.TotalCost = totalCost;
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return Results.Ok(new OrderDetailsDto(
                    existingOrder.Id, existingOrder.TotalCost, existingOrder.OrderDate,
                    existingOrder.CurrentStatus,
                    existingOrder.OrderItems.Select(i => new OrderItemDetailsDto(i.ProductId, i.Quantity, i.UnitPrice)).ToList(),
                    existingOrder.TrackingHistory.Select(t => new TrackingHistoryDto(t.Status, t.StatusChangedAt)).ToList()
                ));
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
}
