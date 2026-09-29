using Application.Common;
using Application.DTOs;
using Domain.Specifications;

namespace Application.Interfaces;

public interface IProductService
{
    Task<Pagination<ProductDto>> GetProductsAsync(ProductSpecParams p);
    Task<ProductDto> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task UpdateAsync(int id, CreateProductDto dto);
    Task DeleteAsync(int id);
    Task<IReadOnlyList<string>> GetBrandsAsync();
    Task<IReadOnlyList<string>> GetCategoriesAsync();
}