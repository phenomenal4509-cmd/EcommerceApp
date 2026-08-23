using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record ProductDto(
    [Range(1,100)][Required]string Name,
    [Required]decimal Price,
    [Required]string Description
);