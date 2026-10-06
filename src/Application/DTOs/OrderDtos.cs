using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record ShippingAddressDto(
    [Required, StringLength(100)] string FullName,
    [Required, StringLength(200)] string Line1,
    [Required, StringLength(100)] string City,
    [Required, StringLength(20)] string PostalCode,
    [Required, StringLength(100)] string Country);

public record CreateOrderDto(Guid BasketKey, ShippingAddressDto ShippingAddress);

public record UpdateOrderStatusDto([Required] string Status);

public record OrderItemDto(
    int ProductId, string ProductName, string PictureUrl,
    decimal UnitPrice, int Quantity, decimal LineTotal);

public record OrderDto(
    int Id, DateTime CreatedAtUtc, string Status, ShippingAddressDto ShippingAddress,
    IReadOnlyList<OrderItemDto> Items, decimal Subtotal, decimal ShippingCost, decimal Total);
