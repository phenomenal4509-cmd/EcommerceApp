namespace MyApp.Api.Models;
public class Order
{
    public int Id{get; set;}
    public string UserId {get;set;}=null!;
    public decimal ProductSubtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalCost{get;set;}
    public ICollection<OrderItem> OrderItems{get; set;}=new List<OrderItem>();
    public DateOnly OrderDate{get;set;}
    public required string CurrentStatus{get;set;}
    public ICollection<Tracking> TrackingHistory { get; set; } = new List<Tracking>();
}