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
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Results.BadRequest("Can not process UserId");
            }

            var orders = await dbContext.Orders
                .Include(o => o.OrderItems).Include(o => o.TrackingHistory)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            if (orders.Count == 0)
            {
                return Results.Ok(new List<OrderDetailsDto>());
            }

            var details = new List<OrderDetailsDto>();

            foreach (var order in orders)
            {
                var itemDetails = order.OrderItems
                    .Select(item => new OrderItemDetailsDto(
                        item.ProductId,
                        item.Quantity,
                        item.UnitPrice))
                    .ToList();

                var trackingHistory = order.TrackingHistory
                    .Select(t => new TrackingHistoryDto(
                        t.Status,
                        t.StatusChangedAt))
                    .ToList();

                OrderDetailsDto orderDetails = new(
                    order.Id,
                    order.TotalCost,
                    order.OrderDate,
                    order.CurrentStatus,
                    itemDetails,
                    trackingHistory
                );

                details.Add(orderDetails);
            }

            return Results.Ok(details);
        }).RequireAuthorization();


        app.MapGet("/orders/{id}", async (int id, HttpContext context, AppDbContex DbContext) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var order = await DbContext.Orders
                .Include(o => o.OrderItems).Include(o => o.TrackingHistory)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == id);

            if (order is null)
            {
                return Results.NotFound("No Order with this Id exists");
            }

            var itemDetails = order.OrderItems
                .Select(item => new OrderItemDetailsDto(
                    item.ProductId,
                    item.Quantity,
                    item.UnitPrice))
                .ToList();

            var trackingHistory = order.TrackingHistory
                .Select(t => new TrackingHistoryDto(
                    t.Status,
                    t.StatusChangedAt))
                .ToList();

            OrderDetailsDto orderDetails = new(
                order.Id,
                order.TotalCost,
                order.OrderDate,
                order.CurrentStatus,
                itemDetails,
                trackingHistory
            );

            return Results.Ok(orderDetails);
        }).RequireAuthorization();


        app.MapPost("/orders", async (CreateOrderDto newOrder, AppDbContex DbContext, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            if (!newOrder.items.Any())
            {
                return Results.BadRequest("An order must have at least one item.");
            }

            decimal totalCost = 0;
            string status = "Pending";

            using var transaction = await DbContext.Database.BeginTransactionAsync();

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
                    var existingProduct = await DbContext.Products.FindAsync(item.ProductId);

                    if (existingProduct is null)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest("This Product is not available!");
                    }
                    if (item.Quantity <= 0)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest($"Quantity must be at least 1 for product {existingProduct.Id}.");
                    }
                    if (item.Quantity >= 50)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest($"Quantity exceeded for the item {existingProduct.Id}");
                    }
                    decimal cost = existingProduct.Price * item.Quantity;
                    totalCost += cost;


                    OrderItem orderItem = new()
                    {
                        ProductId = existingProduct.Id,
                        Quantity = item.Quantity,
                        UnitPrice = existingProduct.Price
                    };

                    order.OrderItems.Add(orderItem);
                }

                order.TotalCost = totalCost;

                DbContext.Orders.Add(order);
                await DbContext.SaveChangesAsync();

                Tracking tracking = new()
                {
                    OrderId = order.Id,
                    Status = status,
                    StatusChangedAt = DateTimeOffset.UtcNow
                };

                DbContext.Tracking.Add(tracking);
                await DbContext.SaveChangesAsync();

                var itemDetails = order.OrderItems
                    .Select(item => new OrderItemDetailsDto(
                        item.ProductId,
                        item.Quantity,
                        item.UnitPrice))
                    .ToList();

                var trackingHistory = new List<TrackingHistoryDto>
                {
                    new TrackingHistoryDto(
                        tracking.Status,
                        tracking.StatusChangedAt)
                };

                OrderDetailsDto orderDetails = new(
                    order.Id,
                    order.TotalCost,
                    order.OrderDate,
                    order.CurrentStatus,
                    itemDetails,
                    trackingHistory
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


        app.MapPut("/orders/{id}", async (int id, AppDbContex DbContext, UpdateOrderDto updateOrder, HttpContext context) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }
            var existingOrder = await DbContext.Orders
                .Include(o => o.OrderItems).Include(o => o.TrackingHistory)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == id);

            if (existingOrder is null)
            {
                return Results.NotFound("No such Order exists");
            }

            // Only allow edits when order is still Pending or Processing
            if (existingOrder.CurrentStatus == "Shipped" ||
                existingOrder.CurrentStatus == "Delivered" ||
                existingOrder.CurrentStatus == "Cancelled")
            {
                return Results.BadRequest($"Cannot edit an order that is already '{existingOrder.CurrentStatus}'.");
            }

            if (!updateOrder.items.Any())
            {
                return Results.BadRequest("An order must have at least one item.");
            }


            using var transaction = await DbContext.Database.BeginTransactionAsync();

            try
            {
                DbContext.OrderItems.RemoveRange(existingOrder.OrderItems);

                decimal totalCost = 0;

                foreach (var item in updateOrder.items)
                {
                    var existingProduct = await DbContext.Products.FindAsync(item.productId);

                    if (existingProduct is null)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest("This Product is not available!");
                    }
                    if (item.Quantity <= 0)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest($"Quantity must be at least 1 for product {existingProduct.Id}.");
                    }
                    if (item.Quantity >= 20)
                    {
                        await transaction.RollbackAsync();
                        return Results.BadRequest($"Quantity exceeded for the item {existingProduct.Id}");
                    }
                    decimal cost = existingProduct.Price * item.Quantity;
                    totalCost += cost;

                    OrderItem orderItem = new()
                    {
                        ProductId = existingProduct.Id,
                        Quantity = item.Quantity,
                        UnitPrice = existingProduct.Price
                    };

                    existingOrder.OrderItems.Add(orderItem);
                }

                existingOrder.TotalCost = totalCost;

                await DbContext.SaveChangesAsync();

                var itemDetails = existingOrder.OrderItems
                    .Select(item => new OrderItemDetailsDto(
                        item.ProductId,
                        item.Quantity,
                        item.UnitPrice))
                    .ToList();

                var trackingHistory = existingOrder.TrackingHistory
                    .Select(t => new TrackingHistoryDto(
                        t.Status,
                        t.StatusChangedAt))
                    .ToList();

                OrderDetailsDto orderDetails = new(
                    existingOrder.Id,
                    existingOrder.TotalCost,
                    existingOrder.OrderDate,
                    existingOrder.CurrentStatus,
                    itemDetails,
                    trackingHistory
                );

                await transaction.CommitAsync();

                return Results.Ok(orderDetails);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }).RequireAuthorization();


        app.MapDelete("/orders/{id}", async (int id, HttpContext context, AppDbContex dbContext) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingOrder = await dbContext.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == id);

            if (existingOrder is null)
            {
                return Results.NotFound("No such Order exists");
            }
            if (existingOrder.CurrentStatus == "Shipped" ||
                existingOrder.CurrentStatus == "Delivered")
            {
                return Results.BadRequest($"Cannot delete an order that is already '{existingOrder.CurrentStatus}'.");
            }
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
