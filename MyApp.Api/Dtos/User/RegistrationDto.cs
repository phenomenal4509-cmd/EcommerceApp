using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record RegistrationDto
(
    [Required]string FirstName,
    [Required]string LastName,
    [Required]string Email,
    [Required]string Password
);