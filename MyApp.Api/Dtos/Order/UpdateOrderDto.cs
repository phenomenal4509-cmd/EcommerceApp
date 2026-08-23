using System.ComponentModel.DataAnnotations;
using MyApp.Api.Models;

namespace MyApp.Api.Dto;
public record UpdateOrderDto(
    [Required]ICollection<OrderItemDetailsDto>items
);