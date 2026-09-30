using Domain.Entities;

namespace Domain.Specifications;

public class BasketByPublicIdSpecification : BaseSpecification<Basket>
{
    public BasketByPublicIdSpecification(Guid publicId) : base(b => b.PublicId == publicId)
    {
        AddInclude("Items.Product");
    }
}