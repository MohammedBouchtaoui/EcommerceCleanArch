using Application.DTOs;

namespace Application.Interfaces;

public interface IBasketService
{
    Task<BasketDto> GetAsync(Guid key);
    Task<BasketDto> AddItemAsync(Guid key, AddBasketItemDto dto);
    Task<BasketDto> SetQuantityAsync(Guid key, int productId, int quantity);
    Task<BasketDto> RemoveItemAsync(Guid key, int productId);
    Task ClearAsync(Guid key);
}
