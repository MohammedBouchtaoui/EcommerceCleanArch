namespace Domain.Entities;

/// <summary>
/// DESIGN PATTERN: Domain Entity
/// Représente l'objet métier 'Produit' pour la plateforme e-commerce Amazon-like.
/// Principe SOLID: Single Responsibility Principle (SRP) - Encapsule uniquement l'état et la structure d'un produit.
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PictureUrl { get; set; } = string.Empty;

    // Propriétés de classification (utilisées par les Specification Patterns pour le filtrage)
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    // Gestion du stock
    public int StockQuantity { get; set; }

    // Date d'ajout au catalogue (utile pour le tri 'Nouveautés')
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}