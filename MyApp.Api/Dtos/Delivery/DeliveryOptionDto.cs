namespace MyApp.Api.Dto;

public record DeliveryOptionDto(
    int Id,
    int DeliveryDays,
    decimal Price,
    long EstimatedDeliveryTimeMs
);
