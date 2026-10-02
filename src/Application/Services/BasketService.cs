using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services;

public class BasketService(
    IGenericRepository<Basket> baskets,
    IGenericRepository<Product> products) : IBasketService
{
    public async Task<BasketDto> GetAsync(Guid key)
    {
        var basket = await Find(key);
        return basket?.ToDto() ?? BasketMappings.Empty(key);
    }

    public async Task<BasketDto> AddItemAsync(Guid key, AddBasketItemDto dto)
    {
        var product = await products.GetByIdAsync(dto.ProductId)
            ?? throw new NotFoundException(nameof(Product), dto.ProductId);

        var basket = await Find(key);
        var isNew = basket is null;
        basket ??= new Basket { PublicId = key };

        var current = basket.Items.FirstOrDefault(i => i.ProductId == product.Id)?.Quantity ?? 0;
        EnsureStock(product, current + dto.Quantity);

        basket.AddItem(product.Id, dto.Quantity);
        if (isNew) baskets.Add(basket);
        await baskets.SaveAllAsync();

        return await GetAsync(key);
    }

    public async Task<BasketDto> SetQuantityAsync(Guid key, int productId, int quantity)
    {
        var basket = await Find(key) ?? throw new NotFoundException(nameof(Basket), key);
        var item = basket.Items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new NotFoundException("Article du panier", productId);

        if (quantity > 0) EnsureStock(item.Product!, quantity);

        basket.SetQuantity(productId, quantity);
        await baskets.SaveAllAsync();
        return basket.ToDto();
    }

    public async Task<BasketDto> RemoveItemAsync(Guid key, int productId)
        => await SetQuantityAsync(key, productId, 0);

    public async Task ClearAsync(Guid key)
    {
        var basket = await Find(key);
        if (basket is null) return;
        baskets.Delete(basket);
        await baskets.SaveAllAsync();
    }

    private Task<Basket?> Find(Guid key)
        => baskets.GetEntityWithSpecAsync(new BasketByPublicIdSpecification(key));

    private static void EnsureStock(Product product, int wanted)
    {
        if (wanted > product.StockQuantity)
            throw new BadRequestException(
                $"Stock insuffisant pour « {product.Name} » : {product.StockQuantity} disponible(s).");
    }
}
