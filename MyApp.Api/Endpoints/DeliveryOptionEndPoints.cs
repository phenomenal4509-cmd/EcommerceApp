using MyApp.Api.Data;
using MyApp.Api.Dto;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Api.EndPoints;

public static class DeliveryOptionEndPoints
{
    public static void MapDeliveryOptionEndPoints(this WebApplication app)
    {
        app.MapGet("/delivery-options", async (AppDbContex dbContext) =>
        {
            var options = await dbContext.DeliveryOptions.OrderBy(o => o.Id).ToListAsync();
            var now = DateTimeOffset.UtcNow;

            var result = options.Select(o => new DeliveryOptionDto(
                o.Id,
                o.DeliveryDays,
                o.Price,
                now.AddDays(o.DeliveryDays).ToUnixTimeMilliseconds()
            )).ToList();

            return Results.Ok(result);
        });
    }
}
