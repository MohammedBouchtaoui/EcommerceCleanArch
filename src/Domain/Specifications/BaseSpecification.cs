using System.Linq.Expressions;

namespace Domain.Specifications;

/// <summary>
/// DESIGN PATTERN: Specification Pattern (Classe de base)
/// Implémentation réutilisable de ISpecification.
/// Principe SOLID: Open/Closed Principle (OCP) - extensible par héritage sans modifier la classe de base.
/// </summary>
public class BaseSpecification<T> : ISpecification<T>
{
    public BaseSpecification() { }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>>? Criteria { get; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; set; }

    // Méthodes 'protected' réservées aux classes filles pour construire la requête
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}