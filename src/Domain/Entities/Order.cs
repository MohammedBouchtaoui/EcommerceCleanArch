using Domain.Exceptions;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public const decimal FreeShippingThreshold = 100m;
    public const decimal FlatShippingCost = 5.90m;

    private static readonly Dictionary<OrderStatus, OrderStatus[]> Transitions = new()
    {
        [OrderStatus.Pending]   = [OrderStatus.Paid, OrderStatus.Cancelled],
        [OrderStatus.Paid]      = [OrderStatus.Shipped, OrderStatus.Cancelled],
        [OrderStatus.Shipped]   = [OrderStatus.Delivered],
        [OrderStatus.Delivered] = [],
        [OrderStatus.Cancelled] = [],
    };

    public string BuyerId { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public ShippingAddress ShippingAddress { get; set; } = new();
    public List<OrderItem> Items { get; set; } = [];












    public decimal Subtotal { get; private set; }
    public decimal ShippingCost { get; private set; }
    public decimal Total { get; private set; }

    public void RecalculateTotals()
    {
        Subtotal = Items.Sum(i => i.UnitPrice * i.Quantity);
        ShippingCost = Subtotal == 0 || Subtotal >= FreeShippingThreshold ? 0m : FlatShippingCost;
        Total = Subtotal + ShippingCost;
    }

    public bool CanTransitionTo(OrderSta











tus next) => Transitions[Status].Contains(next);

    public void ChangeStatus(OrderStatus next)
    {
        if (!CanTransitionTo(next))
            throw new DomainRuleException($"Transition impossible : {Status} → {next}.");
        Status = next;
    }
}
'








@

Save (Target 'Product.cs' 'src\Domain\Entities\Product.cs') @'
namespace Domain.Entities;

public class Product : BaseEntity
{
    public st






ring Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PictureUrl { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
 




   public string Category { get; set; } = string.Empty;
    public int StockQuantity { get; set; }

    // Jeton de concurrence optimiste (rowversion SQL Server)
    public byte[] RowVersion { get; set; } = [];






    public bool TryRemoveStock(int quantity)
    {
        if (quantity <= 0 || quantity > StockQuantity) return false;
      



  StockQuantity -= quantity;
        return true;
    }

    pu



blic void AddStock(int quantity)
    {
        if (quantity > 0) StockQuantity += quantity;
    }
}




