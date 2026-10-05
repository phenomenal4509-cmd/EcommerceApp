using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record OrderItemDetailsDto(
    int productId,
    [Range(1, int.MaxValue)] int Quantity,
    [Range(0.01, double.MaxValue)][Required] decimal UnitPrice,
    string ProductName,
    string ProductImage,
    int DeliveryOptionId,
    DateTimeOffset EstimatedDeliveryAt
);
