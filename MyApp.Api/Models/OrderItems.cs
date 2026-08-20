namespace MyApp.Api.Models;
public class OrderItems{
    public int Id{get; set;}
    public Order? OrderId{get; set;}
    public Products? ProductId{get;set;}
    public int Quantity{get;set;}
};