using Domain.Entities;
using Domain.Exceptions;

namespace Application.Tests;

public class OrderTests
{
    private static Order NewOrder(params (decimal price, int qty)[] lines)
    {
        var order = new Order { BuyerId = "user-1" };
        foreach (var (price, qty) in lines)
            order.Items.Add(new OrderItem { ProductId = 1, ProductName = "Produit", UnitPrice = price, Quantity = qty });
        order.RecalculateTotals();
        return order;
    }

    [Fact]
    public void Small_order_pays_flat_shipping()
    {
        var order = NewOrder((20m, 1));

        Assert.Equal(20m, order.Subtotal);
        Assert.Equal(Orde









r.FlatShippingCost, order.ShippingCost);
        Assert.Equal(25.90m, order.Total);
    }

    [Fact]
    public void Shipping_is_free_at_the_threshold()
    {
        var order = NewOrder((50m, 2));

 








       Assert.Equal(100m, order.Subtotal);
        Assert.Equal(0m, order.ShippingCost);
        Assert.Equal(100m, order.Total);
    }

    [Fact]
    public void Empty_order_has_no_shipping_cost()
 






   {
        var order = NewOrder();

        Assert.Equal(0m, order.Total);
    }

    [Fact]
    public void Full_lifecycle_is_allowed()
    {
        var order = NewOrder((10m, 1));











        order.ChangeStatus(OrderStatus.Paid);
        order.ChangeStatus(OrderStatus.Shipped);
        order.ChangeStatus(OrderStatus.Delivered);

        Assert.Equal(OrderStatus.Delivered, order.Status);
    }








    [Fact]
    public void Pending_order_cannot_be_shipped()
    {
        var order = NewOrder((10m, 1));

        Assert.Throws<DomainRuleException>(() => order.ChangeStatus(OrderSt





atus.Shipped));
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void Delivered_order_is_fin




al()
    {
        var order = NewOrder((10m, 1));
        order.ChangeStatus(OrderStatus.Paid);
        order.ChangeStatus(OrderStatus.Shipped);
 




       order.ChangeStatus(OrderStatus.Delivered);

        Assert.Throws<DomainRuleException>(() => order.ChangeStatus(OrderStatus.Cancelled));
    }





    [Fact]
    public void Product_never_goes_below_zero_stock()
    {
 



       var product = new Product { StockQuantity = 2 };

        Assert.False(product.TryRemoveStock(3));
        Assert.Equal(2, product.StockQuantity);





        Assert.True(product.TryRemoveStock(2));
        Assert.Equal(0, product.StockQuantity);
 


   }
}
