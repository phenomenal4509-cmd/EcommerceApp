using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record CreateProductDto(
    [Range(1,100)][Required]string Name,
    [Required]decimal Price,
    [Required]string Description
);