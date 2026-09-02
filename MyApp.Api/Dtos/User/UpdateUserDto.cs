using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record UpdateUserDto(
   [Required] string FirstName,
   [Required] string LastName
);