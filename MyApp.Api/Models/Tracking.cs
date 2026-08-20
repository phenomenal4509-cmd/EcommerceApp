namespace MyApp.Api.Models;
public class Tracking
{
    public int Id{get; set;}
    public Order? OrderId{get; set;}
    public required string Status {get; set;}
}