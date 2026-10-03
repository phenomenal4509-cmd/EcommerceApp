namespace MyApp.Api.Models;

public class DeliveryOption
{
    public int Id { get; set; }
    public int DeliveryDays { get; set; }
    public decimal Price { get; set; }
    public required string Name { get; set; }
}
