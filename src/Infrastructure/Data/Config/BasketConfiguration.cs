using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.HasIndex(b => b.PublicId).IsUnique();
        builder.HasMany(b => b.Items)
               .WithOne()
               .HasForeignKey(i => i.BasketId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.HasOne(i => i.Product)
               .WithMany()
               .HasForeignKey(i => i.ProductId);

        // Un produit n'apparaît qu'une fois par panier
        builder.HasIndex(i => new { i.BasketId, i.ProductId }).IsUnique();
    }
}