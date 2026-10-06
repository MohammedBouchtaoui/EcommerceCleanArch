using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public static class OrderMappings
{
    public static ShippingAddressDto ToDto(this ShippingAddress a)
        => new(a.FullName, a.Line1, a.City, a.PostalCode, a.Country);

    public static ShippingAddress ToEntity(this ShippingAddressDto d) => new()
    {
        FullName = d.FullName.Trim(),
        Line1 = d.Line1.Trim(),
        City = d.City.Trim(),
        PostalCode = d.PostalCode.Trim(),
        Country = d.Country.Trim(),
    };

    public static OrderItemDto ToDto(this OrderItem i)
        => new(i.ProductId, i.ProductName, i.PictureUrl, i.UnitPrice, i.Quantity, i.LineTotal);

    public static OrderDto ToDto(this Order o) => new(
        o.Id,
        // SQL Server renvoie un DateTime sans "Kind" : on force UTC pour que le JSON porte le "Z"
        DateTime.SpecifyKind(o.CreatedAtUtc, DateTimeKind.Utc),
        o.Status.ToString(),
 














       o.ShippingAddress.ToDto(),
        o.Items.Select(i => i.ToDto()).ToList(),
        o.Subtotal, o.ShippingCost, o.Total);
}
