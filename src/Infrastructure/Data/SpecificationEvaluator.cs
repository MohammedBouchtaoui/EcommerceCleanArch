using Domain.Entities;
using Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// DESIGN PATTERN: Specification Evaluator (Composant d'exécution)
/// Applique dynamiquement les critères de la Specification sur le IQueryable d'EF Core.
/// Principe SOLID: Single Responsibility Principle (SRP) - Responsable uniquement d'appliquer la spec à la requête SQL.
/// </summary>
public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
    {
        var query = inputQuery;

        // 1. Application des critères de filtrage (WHERE)
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        // 2. Application du tri (ORDER BY)
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // 3. Application de la pagination (SKIP / TAKE)
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        // 4. Application des inclusions / jointures (INCLUDE)
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}