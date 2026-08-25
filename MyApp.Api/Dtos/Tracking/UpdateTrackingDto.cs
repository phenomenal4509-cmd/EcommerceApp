using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;
public record UpdateTrackingDto(
    [Required]string Status 
);