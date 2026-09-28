namespace Domain.Entities;

/// <summary>
/// DESIGN PATTERN: Entity / Base Class
/// Encapsule l'identifiant unique commun à toutes les entités du domaine.
/// Principe SOLID: Single Responsibility Principle (SRP) & DRY (Don't Repeat Yourself).
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
}