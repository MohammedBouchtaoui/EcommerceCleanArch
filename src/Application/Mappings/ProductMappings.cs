using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public static class ProductMappings
{
    public static ProductDto ToDto(this Product p) =>
        new(p.Id, p.Name, p.Description, p.Price, p.PictureUrl, p.Brand, p.Category, p.StockQuantity);

    public static Product ToEntity(this CreateProductDto d) => new()
    {
        Name = d.Name,
        Description = d.Description,
        Price = d.Price,
        PictureUrl = d.PictureUrl,
        Brand = d.Brand,
        Category = d.Category,
        StockQuantity = d.StockQuantity
    };
}