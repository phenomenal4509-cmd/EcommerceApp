using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record CartDetailsDto(
    int CartId,
    [Required]decimal TotalCost,
    ICollection<CartItemDetailsDto>Products
);
