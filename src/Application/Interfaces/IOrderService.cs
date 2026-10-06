using Application.DTOs;

namespace Application.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateAsync(string userId, CreateOrderDto dto);
    Task<IReadOnlyList<OrderDto>> GetForUserAsync(string userId);
    Task<OrderDto> GetByIdAsync(int id, string userId, bool isAdmin);
    Task<OrderDto> PayAsync(int id, string userId);
    Task CancelAsync(int id, string userId);
    Task<IReadOnlyList<OrderDto>> GetAllAsync(string? status);
    Task<OrderDto> UpdateStatusAsync(int id, string status);
}
