namespace MyApp.Api.Models;

public class Products
{
    public int Id{get; set;}
    public required string Name{get; set;}
    public decimal Price{get;set;}
    public required string Description{get;set;}
}