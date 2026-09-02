using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;
public record RemoveClaimDto(
[Required] string Type,
[Required] string Value
);