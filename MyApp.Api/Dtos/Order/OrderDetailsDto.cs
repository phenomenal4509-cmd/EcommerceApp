using System.ComponentModel.DataAnnotations;
using MyApp.Api.Models;

namespace MyApp.Api.Dto;
public record OrderDetailsDto
(   
    [Required]int Id,
    [Required]decimal TotalCost,
    [Required]DateOnly OrderDate,
    ICollection<OrderItemDetailsDto> items
);