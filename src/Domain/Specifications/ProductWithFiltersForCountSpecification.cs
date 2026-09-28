using Domain.Entities;

namespace Domain.Specifications;

public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
{
    public ProductWithFiltersForCountSpecification(ProductSpecParams p)
        : base(x =>
            (string.IsNullOrEmpty(p.Search) || x.Name.ToLower().Contains(p.Search!)) &&
            (string.IsNullOrEmpty(p.Brand) || x.Brand == p.Brand) &&
            (string.IsNullOrEmpty(p.Category) || x.Category == p.Category))
    { }
}