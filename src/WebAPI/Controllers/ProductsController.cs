using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

/// <summary>
/// DESIGN PATTERN: Controller / Dependency Injection
/// Principe SOLID: Dependency Inversion Principle (DIP) - dépend uniquement des interfaces (IGenericRepository).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IGenericRepository<Product> _productsRepo;

    public ProductsController(IGenericRepository<Product> productsRepo)
    {
        _productsRepo = productsRepo;
    }

    /// <summary>
    /// Récupère les produits paginés et filtrés façon Amazon.
    /// Exemple : GET /api/products?pageIndex=1&pageSize=10&sort=priceAsc&search=phone
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
    {
        // 1. Instanciation des deux spécifications (Données paginées + Compte total)
        var spec = new ProductsWithTypesAndBrandsSpecification(specParams);
        var countSpec = new ProductWithFiltersForCountSpecification(specParams);

        // 2. Exécution via le Repository
        var totalItems = await _productsRepo.CountAsync(countSpec);
        var products = await _productsRepo.ListAsync(spec);

        // 3. Retour sous forme d'enveloppe paginée
        return Ok(new Pagination<Product>(specParams.PageIndex, specParams.PageSize, totalItems, products));
    }
}