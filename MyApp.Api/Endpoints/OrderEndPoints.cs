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
            var order = DbContext.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
            if(order is null)
            {
                return Results.NotFound("No Order with this Id exists");
            }
            var itemDetails = order.OrderItems.Select(item =>
            new OrderItemDetailsDto(
            item.ProductId,
            item.Quantity,
            item.UnitPrice
            )
            ).ToList();
            OrderDetailsDto orderDetails = new(
            order.Id,
            order.TotalCost,
            order.OrderDate,
            itemDetails
        );
            return Results.Ok(orderDetails);
        });

        app.MapPost("/order", (CreateOrderDto newOrder, AppDbContex DbContext) =>
        {
            decimal totalCost = 0;
            
            Order order = new()
            {
                OrderDate = DateOnly.FromDateTime(DateTime.Now)
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
            var itemDetails = order.OrderItems.Select(item =>
            new OrderItemDetailsDto(
            item.ProductId,
            item.Quantity,
            item.UnitPrice
            )
            ).ToList();
            OrderDetailsDto orderDetails = new(
            order.Id,
            order.TotalCost,
            order.OrderDate,
            itemDetails
        );

            return Results.Created($"/order/{order.Id}", orderDetails);
        });
        app.MapPut("/order/{id}", (int id , AppDbContex DbContext,UpdateOrderDto updateOrder) =>
        {
            var existingOrder = DbContext.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == id);
            if(existingOrder is null)
            {
                return Results.NotFound("No such Order exists");
            }
            DbContext.OrderItems.RemoveRange(existingOrder.OrderItems);
            decimal totalCost=0;
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
            existingOrder.TotalCost=totalCost;
            DbContext.SaveChanges();
            var itemDetails = existingOrder.OrderItems.Select(item =>
            new OrderItemDetailsDto(
            item.ProductId,
            item.Quantity,
            item.UnitPrice
            )
            ).ToList();
            OrderDetailsDto orderDetails = new(
            existingOrder.Id,
            existingOrder.TotalCost,
            existingOrder.OrderDate,
            itemDetails
        );
        return Results.Ok(orderDetails);
        });
        app.MapDelete("/order/{id}",(int id,AppDbContex DbContext) =>
        {
            var existingOrder= DbContext.Orders.FirstOrDefault(o=>o.Id==id);
            if(existingOrder is null)
            {
                return Results.NotFound("No such Order exists");
            }
            DbContext.Orders.Remove(existingOrder);
            DbContext.SaveChanges();
            return Results.NoContent();
        });

    }
}