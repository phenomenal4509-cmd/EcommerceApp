using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record ProductDto(
    [Range(1,100)][Required]string Name,
    [Range(0.01,double.MaxValue)][Required]decimal Price,
    [Required]string Description,
    [Required] Guid Version
);