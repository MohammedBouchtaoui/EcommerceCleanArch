namespace Domain.Entities;

public class Basket : BaseEntity
{
    public Guid PublicId { get; set; } = Guid.NewGuid();
    public DateTime UpdatedAtUtc { get; private set; } = DateTime.UtcNow;
    public List<BasketItem> Items { get; set; } = [];

    public void AddItem(int productId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
            Items.Add(new BasketItem { ProductId = productId, Quantity = quantity });
        else
            item.Quantity += quantity;
        Touch();
    }

    public void SetQuantity(int productId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return;

        if (quantity <= 0) Items.Remove(item);
        else item.Quantity = quantity;
        Touch();
    }

    public void RemoveItem(int productId) => SetQuantity(productId, 0);

    private void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}