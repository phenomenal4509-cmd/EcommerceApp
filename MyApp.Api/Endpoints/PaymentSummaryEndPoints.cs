using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.Services;

namespace MyApp.Api.EndPoints;

public static class PaymentSummaryEndPoints
{
    public static void MapPaymentSummaryEndPoints(this WebApplication app)
    {
        app.MapGet("/payment-summary", async (HttpContext context, AppDbContex dbContext) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var cart = await dbContext.Cart
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserID == userId);

            if (cart is null || !cart.CartItems.Any())
                return Results.Ok(new MyApp.Api.Dto.PaymentSummaryDto(0, 0, 0, 0, 0, 0));

            var (summary, error) = await CartPricing.BuildPaymentSummaryAsync(cart, dbContext);
            if (error is not null)
                return Results.BadRequest(error);

            return Results.Ok(summary);
        }).RequireAuthorization();
    }
}
