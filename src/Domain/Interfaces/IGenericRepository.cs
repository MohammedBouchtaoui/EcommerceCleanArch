using Domain.Entities;
using Domain.Specifications;

namespace Domain.Interfaces;

/// <summary>
/// DESIGN PATTERN: Generic Repository Pattern (Interface)
/// Abstrait la couche d'accès aux données pour l'ensemble des entités héritant de BaseEntity.
/// Principe SOLID: Interface Segregation Principle (ISP) & Dependency Inversion Principle (DIP).
/// </summary>
public interface IGenericRepository<T> where T : BaseEntity
{
    // Opérations de base par ID
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();

    // Opérations basées sur le Specification Pattern
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);

    // Méthodes de modification d'état (UoW pattern / Tracking)
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}