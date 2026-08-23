using System.ComponentModel.DataAnnotations;
using MyApp.Api.Models;
namespace MyApp.Api.Dto;
public record OrderDto
(
    int ProductId,
    [Range(1,int.MaxValue)] int Quantity
);