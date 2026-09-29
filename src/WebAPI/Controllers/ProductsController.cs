<<<<<<< Updated upstream
﻿using Application.Interfaces;
using Domain.Entities;
=======
﻿using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Specifications;
>>>>>>> Stashed changes
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
<<<<<<< Updated upstream
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var product = new Product(dto.Name, dto.Description, dto.Price, dto.StockQuantity);
        await _productRepository.AddAsync(product);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
=======
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

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetProduct), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CreateProductDto dto)
    {
        await service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
>>>>>>> Stashed changes
    }
}

public record CreateProductDto(string Name, string Description, decimal Price, int StockQuantity);