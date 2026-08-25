namespace MyApp.Api.Models;

public class Tracking
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public required string Status { get; set; }

    public DateTimeOffset StatusChangedAt { get; set; }
    
}