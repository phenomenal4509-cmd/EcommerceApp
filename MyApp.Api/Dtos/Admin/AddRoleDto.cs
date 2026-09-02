using System.ComponentModel.DataAnnotations;

namespace MyApp.Api.Dto;

public record AddRoleDto(
    [Required]string role
);