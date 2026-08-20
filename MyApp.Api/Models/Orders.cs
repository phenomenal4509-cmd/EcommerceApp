namespace MyApp.Api.Models;
public class Order
{
    public int Id{get; set;}
    public DateOnly OrderDate{get;set;}
    public int TotalCost{get;set;}
}