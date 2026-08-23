using System.ComponentModel.DataAnnotations;
using MyApp.Api.Models;
namespace MyApp.Api.Dto;
public record CreateOrderDto
(
    ICollection<OrderDto> items
);