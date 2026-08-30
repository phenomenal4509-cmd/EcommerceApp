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
        app.MapGet("/order",(HttpContext context, AppDbContex dbContext) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Results.BadRequest("Can not process UserId");
            }
            var orders = dbContext.Orders.Include(o=>o.OrderItems).Include(o=>o.TrackingHistory).Where(o=> o.UserId==userId).ToList();
            if(orders.Count == 0)
            {
                return Results.Ok(new List<OrderDetailsDto>());
            }
            var details = new List<OrderDetailsDto>();
            foreach(var order in orders)
            {
                var itemDetails = order.OrderItems
                .Select(item => new OrderItemDetailsDto(item.ProductId, item.Quantity, item.UnitPrice))
                .ToList();

                var trackingHistory = order.TrackingHistory
                .Select(t => new TrackingHistoryDto(t.Status, t.StatusChangedAt))
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
        app.MapGet("/order/{id}", (int id,HttpContext context, AppDbContex DbContext) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
            var order = DbContext.Orders.Include(o => o.OrderItems).Include(o => o.TrackingHistory).FirstOrDefault(o => o.UserId==userId && o.Id==id);
            if (order is null)
            {
                return Results.NotFound("No Order with this Id exists");
            }

            var itemDetails = order.OrderItems
                .Select(item => new OrderItemDetailsDto(item.ProductId, item.Quantity, item.UnitPrice))
                .ToList();

            var trackingHistory = order.TrackingHistory
                .Select(t => new TrackingHistoryDto(t.Status, t.StatusChangedAt))
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
        app.MapPost("/order", (CreateOrderDto newOrder, AppDbContex DbContext,HttpContext context) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }
            decimal totalCost = 0;
            string status="Pending";
            Order order = new()
            {
                UserId=userId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                CurrentStatus = status
            };

            foreach (var item in newOrder.items)
            {
                var existingProduct = DbContext.Products.Find(item.ProductId);

                if (existingProduct is null)
                {
                    return Results.BadRequest("This Product is not available!");
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
            DbContext.SaveChanges(); 

            Tracking tracking = new()
            {
                OrderId = order.Id, 
                Status = status,
                StatusChangedAt = DateTimeOffset.UtcNow
            };
            DbContext.Tracking.Add(tracking);
            DbContext.SaveChanges(); 

            var itemDetails = order.OrderItems
                .Select(item => new OrderItemDetailsDto(item.ProductId, item.Quantity, item.UnitPrice))
                .ToList();

            var trackingHistory = new List<TrackingHistoryDto>
            {
                new TrackingHistoryDto(tracking.Status, tracking.StatusChangedAt)
            };

            OrderDetailsDto orderDetails = new(
                order.Id,
                order.TotalCost,
                order.OrderDate,
                order.CurrentStatus,
                itemDetails,
                trackingHistory
            );

            return Results.Created($"/order/{order.Id}", orderDetails);
        }).RequireAuthorization();


        app.MapPut("/order/{id}", (int id, AppDbContex DbContext, UpdateOrderDto updateOrder,HttpContext context) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                {
                    return Results.Unauthorized();
                }
            var existingOrder = DbContext.Orders.Include(o => o.OrderItems).Include(o => o.TrackingHistory).FirstOrDefault(o => o.UserId==userId&&o.Id==id);

            if (existingOrder is null)
            {
                return Results.NotFound("No such Order exists");
            }

            DbContext.OrderItems.RemoveRange(existingOrder.OrderItems);

            decimal totalCost = 0;

            foreach (var item in updateOrder.items)
            {
                var existingProduct = DbContext.Products.Find(item.productId);

                if (existingProduct is null)
                {
                    return Results.BadRequest("This Product is not available!");
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

            DbContext.SaveChanges();

            var itemDetails = existingOrder.OrderItems
                .Select(item => new OrderItemDetailsDto(item.ProductId, item.Quantity, item.UnitPrice))
                .ToList();

            var trackingHistory = existingOrder.TrackingHistory
                .Select(t => new TrackingHistoryDto(t.Status, t.StatusChangedAt))
                .ToList();

            OrderDetailsDto orderDetails = new(
                existingOrder.Id,
                existingOrder.TotalCost,
                existingOrder.OrderDate,
                existingOrder.CurrentStatus,
                itemDetails,
                trackingHistory
            );

            return Results.Ok(orderDetails);
        }).RequireAuthorization();


        app.MapDelete("/order/{id}", (int id ,HttpContext context, AppDbContex dbContext) =>
        {
            var userId=context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingOrder = dbContext.Orders.FirstOrDefault(o => o.UserId == userId&& o.Id==id);

            if (existingOrder is null)
            {
                return Results.NotFound("No such Order exists");
            }

            dbContext.Orders.Remove(existingOrder);
            dbContext.SaveChanges();

            return Results.NoContent();
        }).RequireAuthorization();
    }
}