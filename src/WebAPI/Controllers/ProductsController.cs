using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecParams p)
        => Ok(await service.GetProductsAsync(p));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
        => Ok(await service.GetByIdAsync(id));

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        => Ok(await service.GetBrandsAsync());

 












   [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCategories()
        => Ok(await service.GetCategoriesAsync());

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetProduct), new { id = created.Id }, created);
 









   }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateProductDto dto)
    {
        await service.UpdateAsync(id, dto);
        return NoContent();
    }









    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
 






       return NoContent();
    }
}
