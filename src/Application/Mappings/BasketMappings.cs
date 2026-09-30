using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public static class BasketMappings
{
    public static BasketDto ToDto(this Basket basket)
    {
        var items = basket.Items
            .Where(i => i.Product is not null)
            .OrderBy(i => i.Id)
            .Select(i => new BasketItemDto(
                i.ProductId, i.Product!.Name, i.Product.PictureUrl, i.Product.Brand,
                i.Product.Price, i.Quantity, i.Product.Price * i.Quantity))
            .ToList();

        return new BasketDto(basket.PublicId, items, items.Sum(x => x.Quantity), items.Sum(x => x.LineTotal));
    }

    public static BasketDto Empty(Guid key) => new(key, [], 0, 0m);
}