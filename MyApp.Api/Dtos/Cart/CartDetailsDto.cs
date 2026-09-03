using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record CartDetailsDto(
    int CartId,
    [Range(0.01,double.MaxValue)][Required]decimal TotalCost,
    ICollection<CartItemDetailsDto>Products
);
