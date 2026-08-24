using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record CartItemDetailsDto(
    int Id,
    [Range(1,100)][Required]string Name,
    [Required]decimal Price,
    [Required]string Description,
    int Quantity
);
