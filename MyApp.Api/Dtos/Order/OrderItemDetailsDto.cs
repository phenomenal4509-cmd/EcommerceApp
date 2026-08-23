using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;
public record OrderItemDetailsDto(
    int productId,
    int Quantity,
    [Required]decimal UnitPrice
);