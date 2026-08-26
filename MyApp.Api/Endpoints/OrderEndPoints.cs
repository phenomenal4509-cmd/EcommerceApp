using MyApp.Api.Data;
using MyApp.Api.Models;
using MyApp.Api.Dto;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Api.EndPoints;

public static class OrderEndPoints
{
    public static void MapOrderEndPoints(this WebApplication app)
    {

        app.MapGet("/order/{id}", (int id, AppDbContex DbContext) =>
        {
            var order = DbContext.Orders.Include(o => o.OrderItems).Include(o => o.TrackingHistory).FirstOrDefault(o => o.Id == id);

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
        });
        app.MapPost("/order", (CreateOrderDto newOrder, AppDbContex DbContext) =>
        {
            decimal totalCost = 0;
            string status="Pending";
            Order order = new()
            {
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
            DbContext.SaveChanges(); // Step 1: Insert Order + OrderItems → order.Id is now the real DB Id

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
        });


        app.MapPut("/order/{id}", (int id, AppDbContex DbContext, UpdateOrderDto updateOrder) =>
        {
            var existingOrder = DbContext.Orders.Include(o => o.OrderItems).Include(o => o.TrackingHistory).FirstOrDefault(o => o.Id == id);

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
        });


        app.MapDelete("/order/{id}", (int id, AppDbContex dbContext) =>
        {
            var existingOrder = dbContext.Orders.FirstOrDefault(o => o.Id == id);

            if (existingOrder is null)
            {
                return Results.NotFound("No such Order exists");
            }

            dbContext.Orders.Remove(existingOrder);
            dbContext.SaveChanges();

            return Results.NoContent();
        });
    }
}