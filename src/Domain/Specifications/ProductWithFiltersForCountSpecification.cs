
using Domain.Entities;
using Application.DTOs;

namespace Domain.Specifications;

/// <summary>
/// DESIGN PATTERN: Concrete Specification Pattern
/// Calcule le NOMBRE TOTAL de produits correspondant aux filtres (SANS la pagination)
/// afin d'afficher le nombre total de pages sur le frontend.
/// </summary>
public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
{
    public ProductWithFiltersForCountSpecification(ProductSpecParams specParams)
        : base(x =>
            (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
            (string.IsNullOrEmpty(specParams.Brand) || x.Brand == specParams.Brand) &&
            (string.IsNullOrEmpty(specParams.Category) || x.Category == specParams.Category))
    {
    }
}