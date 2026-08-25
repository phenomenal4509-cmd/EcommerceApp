using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;
public record TrackingDto(
    int OrderId,
    [Required]string CurrentStatus,
    ICollection<TrackingHistoryDto>History
);