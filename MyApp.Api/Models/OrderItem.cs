namespace MyApp.Api.Models;
public class OrderItem{
    public int Id{get; set;}
    public int OrderId{get; set;}   
    public int ProductId{get;set;}
    public int Quantity{get;set;}
    public decimal UnitPrice{get;set;}
    public int DeliveryOptionId { get; set; }
    public DateTimeOffset EstimatedDeliveryAt { get; set; }
};