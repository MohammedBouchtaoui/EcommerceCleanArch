using Domain.Entities;

namespace Domain.Specifications;

public class OrdersByBuyerSpecification : BaseSpecification<Order>
{
    public OrdersByBuyerSpecification(string buyerId) : base(o => o.BuyerId == buyerId)
    {
        AddInclude("Items");
        AddOrderByDescending(o => o.CreatedAtUtc);
    }
}

// buyerId = null : accès administrateur (toutes les commandes)
public class OrderByIdSpecification : BaseSpecification<Order>
{
    public OrderByIdSpecification(int id, string? buyerId = null)
        : base(o => o.Id == id && (buyerId == null || o.BuyerId == buyerId))
    {
        AddInclude("Items");
    }
}

public class AllOrdersSpecification : BaseSpecification<Order>
{
    public AllOrdersSpecification(OrderStatus? status)
        : base(o => status == null || o.Status == status)
    {
 















       AddInclude("Items");
        AddOrderByDescending(o => o.CreatedAtUtc);
        ApplyPaging(0, 100);
    }
}
