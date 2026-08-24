namespace MyApp.Api.Dto;


public record UpdateCartDto(
    ICollection<CartItemDto> items
);