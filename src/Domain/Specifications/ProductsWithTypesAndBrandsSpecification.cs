using Domain.Entities;
using Application.DTOs;

namespace Domain.Specifications;

/// <summary>
/// DESIGN PATTERN: Concrete Specification Pattern (Spécification Métier)
/// Encapsule la logique métier précise du catalogue produits Amazon (recherche textuelle, filtre par marque/catégorie, tri, pagination).
/// Principe SOLID: Open/Closed Principle (OCP) & Liskov Substitution Principle (LSP).
/// </summary>
public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(ProductSpecParams specParams)
        : base(x =>
            (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
            (string.IsNullOrEmpty(specParams.Brand) || x.Brand == specParams.Brand) &&
            (string.IsNullOrEmpty(specParams.Category) || x.Category == specParams.Category))
    {
        // Applique la pagination (ex: Page 2 avec 10 items => Skip 10, Take 10)
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        // Applique le tri selon le paramètre reçu
        if (!string.IsNullOrEmpty(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
        else
        {
            AddOrderBy(p => p.Name);
        }
    }
}