using System.Linq.Expressions;
using Domain.Entities;
using Domain.Specifications;

namespace Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
    Task<IReadOnlyList<string>> ListDistinctAsync(Expression<Func<T, string>> selector);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(int id);
}
