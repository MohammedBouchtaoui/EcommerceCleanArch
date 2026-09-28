using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helpers;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams p)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(p);
        var countSpec = new ProductWithFiltersForCountSpecification(p);

        var total = await repo.CountAsync(countSpec);
        var items = await repo.ListAsync(spec);

        return Ok(new Pagination<Product>(p.PageIndex, p.PageSize, total, items));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetEntityWithSpecAsync(new ProductsWithTypesAndBrandsSpecification(id));
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
        if (!await repo.SaveAllAsync()) return BadRequest("Échec de la création du produit");
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !repo.Exists(id)) return BadRequest("Produit invalide");
        repo.Update(product);
        return await repo.SaveAllAsync() ? NoContent() : BadRequest("Échec de la mise à jour");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product is null) return NotFound();
        repo.Delete(product);
        return await repo.SaveAllAsync() ? NoContent() : BadRequest("Échec de la suppression");
    }
}