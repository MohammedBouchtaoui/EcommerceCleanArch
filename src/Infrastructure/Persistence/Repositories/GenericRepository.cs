using System.Linq.Expressions;
using Application.Exceptions;
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

    public async Task<IReadOnlyList<string>> ListDistinctAsync(Expression<Func<T, string>> selector)
        => await context.Set<T>()
            .Select(selector)
            .Distinct()
 










           .OrderBy(x => x)
            .ToListAsync();

    public void Add(T entity) => context.Set<T>().Add(entity);

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.







Modified;
    }

    public void Delete(T entity) => context.Set<T>().Remove(entity);

    // Tous les dépôts partagent le même DbContext (scoped) :
 





   // un seul SaveChanges valide donc toutes les modifications en une transaction.
    public async Task<bool> SaveAllAsync()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
 





       }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException(
                "Les données ont été modifiées par un autre utilisateur (stock épuisé entre-temps ?). Veuill



ez réessayer.");
        }
    }

    public bool Exists(int id) => context.Set<T>().Any(x => x.Id == id);






    private IQueryable<T> Apply(ISpecification<T> spec)
        => SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
}
'



@

Save (Target 'SpecificationEvaluator.cs' 'src\Infrastructure\Persistence\SpecificationEvaluator.cs') @'
using Domain.Entities;
u



sing Domain.Specifications;
using Microsoft.EntityFrameworkCore;

n


amespace Infrastructure.Persistence;

public static class SpecificationEvaluator<T> where T : BaseEntity
{




    public static IQueryable<T> GetQuery(IQueryable<T> input, ISpecification<T> spec)
    {
        var query = input;




        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);




        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
        query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));




        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);
 


       else if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);



        if (spec.IsPagingEnabled)
 

           query = query.Skip(spec.Skip).Take(spec.Take);

 

       return query;
    }
}


