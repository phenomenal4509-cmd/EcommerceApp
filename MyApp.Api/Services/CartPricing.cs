using Microsoft.EntityFrameworkCore;
using MyApp.Api.Data;
using MyApp.Api.Dto;
using MyApp.Api.Models;

namespace MyApp.Api.Services;

public static class CartPricing
{
    public static async Task<(PaymentSummaryDto? Summary, string? Error)> BuildPaymentSummaryAsync(
        Cart cart,
        AppDbContex dbContext)
    {
        var totalItems = 0;
        decimal productCost = 0;
        decimal shippingCost = 0;

        foreach (var item in cart.CartItems)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
            if (product is null)
                return (null, $"Product {item.ProductId} does not exist.");

            var deliveryOption = await dbContext.DeliveryOptions.FindAsync(item.DeliveryOptionId);
            if (deliveryOption is null)
                return (null, $"Delivery option {item.DeliveryOptionId} is invalid.");

            totalItems += item.Quantity;
            productCost += product.Price * item.Quantity;
            shippingCost += deliveryOption.Price;
        }

        var totalBeforeTax = productCost + shippingCost;
        var tax = Math.Round(totalBeforeTax * 0.1m, 2, MidpointRounding.AwayFromZero);
        var total = totalBeforeTax + tax;

        static int ToCents(decimal amount) => (int)Math.Round(amount * 100, MidpointRounding.AwayFromZero);

        return (new PaymentSummaryDto(
            totalItems,
            ToCents(productCost),
            ToCents(shippingCost),
            ToCents(totalBeforeTax),
            ToCents(tax),
            ToCents(total)), null);
    }
}
