namespace MyApp.Api.Dto;
public record OrderDetailsDto(
    int Id,
    decimal TotalCost,
    DateOnly OrderDate,
    string CurrentStatus,
    ICollection<OrderItemDetailsDto> Items,
    ICollection<TrackingHistoryDto> TrackingHistory
);