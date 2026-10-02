using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T>
    where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(int id) => await context.Set<T>().FindAsync(id);

    public async Task<IReadOnlyList<T>> ListAllAsync() => await context.Set<T>().ToListAsync();

    public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec)
        => await Apply(spec).FirstOrDefaultAsync();

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        => await Apply(spec).ToListAsync();

    public async Task<int> CountAsync(ISpecification<T> spec)
        => await Apply(spec).CountAsync();

    public async Task<IReadOnlyList<string>> ListDistinctAsync(Expression<Func











<T, string>> selector)
        => await context.Set<T>()
            .Select(selector)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

    public void Add(T entity) => context.Set<T>().Add(entity);

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity) => context.Set<T>().Remove(entity);

    public async Task<bool> SaveAllAsync() => await context.SaveChangesAsync() > 0;

    public bool Exists(int id) => context.Set<T>().Any(x => x.Id == id);

 








   private IQueryable<T> Apply(ISpecification<T> spec)
        => SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
}
