using Application.Common;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services;

public class ProductService(IGenericRepository<Product> repo) : IProductService
{
    public async Task<Pagination<ProductDto>> GetProductsAsync(ProductSpecParams p)
    {
        var total = await repo.CountAsync(new ProductWithFiltersForCountSpecification(p));
        var items = await repo.ListAsync(new ProductsWithTypesAndBrandsSpecification(p));
        return new Pagination<ProductDto>(p.PageIndex, p.PageSize, total,
            items.Select(x => x.ToDto()).ToList());
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await repo.GetEntityWithSpecAsync(new ProductsWithTypesAndBrandsSpecification(id))
            ?? throw new NotFoundException(nameof(Product), id);
        return product.ToDto();
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var entity = dto.ToEntity();
        repo.Add(entity);
        if (!await repo.SaveAllAsync())
            throw new BadRequestException("Échec de la création du produit.");
        return entity.ToDto();
    }

    public async Task UpdateAsync(int id, CreateProductDto dto)
    {
        var entity = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Product), id);

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Price = dto.Price;
        entity.PictureUrl = dto.PictureUrl;
        entity.Brand = dto.Brand;
        entity.Category = dto.Category;
        entity.StockQuantity = dto.StockQuantity;

        // SaveChanges renvoie 0 si rien n'a changé : ce n'est pas une erreur
        await repo.SaveAllAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await repo.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Product), id);
        repo.Delete(entity);
        await repo.SaveAllAsync();
    }

    public Task<IReadOnlyList<string>> GetBrandsAsync() => repo.ListDistinctAsync(x => x.Brand);
    public Task<IReadOnlyList<string>> GetCategoriesAsync() => repo.ListDistinctAsync(x => x.Category);
}