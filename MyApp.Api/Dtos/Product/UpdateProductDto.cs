using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record UpdateProductDto(
    
    [Required]string Name,
    [Range(0.01,double.MaxValue)][Required]decimal Price,
    [Required]string Description,
    [Required]string Image,
    [Required]Guid Version
);