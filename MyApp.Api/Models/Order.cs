namespace MyApp.Api.Models;
public class Order
{
    public int Id{get; set;}
    public decimal TotalCost{get;set;}
    public ICollection<OrderItem> OrderItems{get; set;}=new List<OrderItem>();
    public DateOnly OrderDate{get;set;}
}