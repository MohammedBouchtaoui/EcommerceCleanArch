using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

/// <summary>
/// DESIGN PATTERN: Entity Configuration (Fluent API)
/// Separe la configuration de la base de données de la classe du domaine.
/// Principe SOLID: Single Responsibility Principle (SRP) - L'entité Product reste pure et indépendante d'EF Core.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        // Précision obligatoire pour les montants financiers (ex: 18,2)
        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PictureUrl)
            .IsRequired();

        builder.Property(p => p.Brand)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(50);
    }
}²