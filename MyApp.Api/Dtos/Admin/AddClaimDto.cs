using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record AddClaimDto(
    [Required] string Type,
    [Required] string Value
);