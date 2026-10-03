using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record CartItemDto(
    int ProductId,
    [Range(1,int.MaxValue)]int Quantity,
    int DeliveryOptionId = 1
);