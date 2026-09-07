using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace MyApp.Api.Dto;

public record DeleteProductDto
(
     Guid Version
);