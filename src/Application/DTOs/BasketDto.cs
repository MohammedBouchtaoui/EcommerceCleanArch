using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record BasketItemDto(
    int ProductId, string Name, string PictureUrl, string Brand,
    decimal UnitPrice, int Quantity, decimal LineTotal);

public record BasketDto(
    Guid Key, IReadOnlyList<BasketItemDto> Items, int TotalItems, decimal Subtotal);

public record AddBasketItemDto(
    [Range(1, int.MaxValue)] int ProductId,
    [Range(1, 99)] int Quantity);

public record UpdateBasketItemDto([Range(0, 99)] int Quantity);