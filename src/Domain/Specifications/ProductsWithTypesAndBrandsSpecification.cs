using Domain.Entities;

namespace Domain.Specifications;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification(ProductSpecParams p)
        : base(x =>
            (string.IsNullOrEmpty(p.Search) || x.Name.ToLower().Contains(p.Search!)) &&
            (string.IsNullOrEmpty(p.Brand) || x.Brand == p.Brand) &&
            (string.IsNullOrEmpty(p.Category) || x.Category == p.Category))
    {
        ApplyPaging((p.PageIndex - 1) * p.PageSize, p.PageSize);

        switch (p.Sort)
        {
            case "priceAsc": AddOrderBy(x => x.Price); break;
            case "priceDesc": AddOrderByDescending(x => x.Price); break;
            default: AddOrderBy(x => x.Name); break;
        }
    }

    public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id) { }
}