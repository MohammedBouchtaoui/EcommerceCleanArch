using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.BuyerId).IsRequired().HasMaxLength(450);
        builder.HasIndex(o => o.BuyerId);

        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
        builder.Property(o => o.ShippingCost).HasColumnType("decimal(18,2)");
        builder.Property(o => o.Total).HasColumnType("decimal(18,2)");

        builder.OwnsOne(o => o.ShippingAddress, a =>
        {
            a.Property(x => x.FullName).IsRequired().HasMaxLength(100);








            a.Property(x => x.Line1).IsRequired().HasMaxLength(200);
            a.Property(x => x.City).IsRequired().HasMaxLength(100);
            a.Property(x => x.PostalCode).IsRequired().HasMaxLength(20);
            a.Property(x => x.Country).IsRequired().HasMaxLength(100);
        });

        builder.HasMany(o => o.Items)
               .WithOne()
               .HasForeignKey(i => i.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
 










   }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(i => i.ProductName).IsRequired().HasMaxLength(200);
        builder.Property(i => i.PictureUrl).IsRequired();
 








       builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)");
    }
}
