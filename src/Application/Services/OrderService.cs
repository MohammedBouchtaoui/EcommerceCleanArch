using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services;

public class OrderService(
    IGenericRepository<Order> orders,
    IGenericRepository<Basket> baskets,
    IGenericRepository<Product> products) : IOrderService
{
    public async Task<OrderDto> CreateAsync(string userId, CreateOrderDto dto)
    {
        if (dto.BasketKey == Guid.Empty)
            throw new BadRequestException("Clé de panier invalide.");

        var basket = await baskets.GetEntityWithSpecAsync(new BasketByPublicIdSpecification(dto.BasketKey));
        if (basket is null || basket.Items.Count == 0)
            throw new BadRequestException("Votre panier est vide.");

        var order = new Order
        {
            BuyerId = userId,
            ShippingAddress = dto.ShippingAddress.ToEntity(),
    
















    };

        foreach (var item in basket.Items)
        {
            var product = item.Product ?? throw new BadRequestException("Un produit du panier n'est plus disponible.");

            if (!product.TryRemoveStock(item.Quantity))
                throw new BadRequestException(
                    $"Stock insuffisant pour « {product.Name} » : {product.StockQuantity} disponible(s).");

            order.Items.Add(new OrderItem
            {
 











               ProductId = product.Id,
                ProductName = product.Name,
                PictureUrl = product.PictureUrl,
                UnitPrice = product.Price,   // prix figé au moment de l'achat
                Quantity = item.Quantity,
            });
 





       }

        order.RecalculateTotals();
        orders.Add(order);
        baskets.Delete(basket);

        // Une seule transaction : commande + stock + suppression du panier.
 






       // Si deux acheteurs visent la dernière unité, le jeton rowversion déclenche un ConflictException (409).
        await orders.SaveAllAsync();
        return order.ToDto();
    }





    public async Task<IReadOnlyList<OrderDto>> GetForUserAsync(string userId)
    {
        var list = await orders.ListAsync(new OrdersByBuyerSpecification(userId));
 



       return list.Select(o => o.ToDto()).ToList();
    }

    public async Task<OrderDto> GetByIdAsync(int id, string userId, bool isAdmin)
 



       => (await Find(id, isAdmin ? null : userId)).ToDto();

    public async Task<OrderDto> PayAsync(int id, string userId)
    {
 



       var order = await Find(id, userId);
        order.ChangeStatus(OrderStatus.Paid);   // simulation : un vrai paiement passerait par un prestataire
        await orders.SaveAllAsync();
 


       return order.ToDto();
    }

    public async Task CancelAsync(int id, string userId)
 



   {
        var order = await Find(id, userId);
        if (order.Status != OrderStatus.Pending)
     


       throw new DomainRuleException("Seules les commandes en attente peuvent être annulées.");

        await CancelAndRestockAsync(order);
 


   }

    public async Task<IReadOnlyList<OrderDto>> GetAllAsync(string? status)
 


   {
        OrderStatus? filter = string.IsNullOrWhiteSpace(status) ? null : ParseStatus(status);
 

       var list = await orders.ListAsync(new AllOrdersSpecification(filter));
        return list.Select(o => o.ToDto()).ToList();
 

   }

    public async Task<OrderDto> UpdateStatusAsync(

int id, string status)
    {
 

       var next = ParseStatus(status);
        var order = await Find(id, null);



        if (next == OrderStatus.Cancelled)
 

           await CancelAndRestockAsync(order);
        else
 

       {
            order.ChangeStatus(next);
 

           await orders.SaveAllAsync();
        }



        return order.ToDto();
 

   }

 

   private async Task CancelAndRestockAsync(Order order)
    {
 

       // La transition est validée avant toute modification du stock
        order.ChangeStatus(OrderStatus.Cancelled);



        foreach (var item in order.Items)
 

       {
            var product = await products.GetByIdAsync(item.ProductId);
 

           product?.AddStock(item.Quantity);
        }



        await orders.SaveAllAsync();
 

   }

 

   private async Task<Order> Find(int id, string? buyerId)
        => await orders.GetEntityWithSpecAsync(new OrderByIdSpecification(id, buyerId))
 

          ?? throw new NotFoundException(nameof(Order), id);

 

   private static OrderStatus ParseStatus(string value)
    {
 

       if (Enum.TryParse<OrderStatus>(value, ignoreCase: true, out var status) && Enum.IsDefined(status))
            return status;
 

       throw new BadRequestException($"Statut inconnu : « {value} ».");
    }
}


