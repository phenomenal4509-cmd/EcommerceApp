namespace MyApp.Api.Dto;


public record CreateCartDto(
    ICollection<CartItemDto> items
);