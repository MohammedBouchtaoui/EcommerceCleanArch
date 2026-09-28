using System.Linq.Expressions;

namespace Domain.Specifications;

/// <summary>
/// DESIGN PATTERN: Specification Pattern (Interface)
/// Définit le contrat d'une spécification pour encapsuler la logique de requête LINQ / EF Core.
/// Principe SOLID: Interface Segregation Principle (ISP) & Dependency Inversion Principle (DIP).
/// </summary>
public interface ISpecification<T>
{
    // Condition de filtrage (ex: x => x.Price > 100)
    Expression<Func<T, bool>>? Criteria { get; }

    // Liste des jointures/relations à inclure (ex: x => x.Brand, x => x.Category)
    List<Expression<Func<T, object>>> Includes { get; }

    // Tri croissant
    Expression<Func<T, object>>? OrderBy { get; }

    // Tri décroissant
    Expression<Func<T, object>>? OrderByDescending { get; }

    // Paramètres de pagination
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; set; }
}