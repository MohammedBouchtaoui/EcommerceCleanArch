using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/basket/{key:guid}")]
public class BasketController(IBasketService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<BasketDto>> Get(Guid key)
        => Ok(await service.GetAsync(key));

    [HttpPost("items")]
    public async Task<ActionResult<BasketDto>> AddItem(Guid key, AddBasketItemDto dto)
        => Ok(await service.AddItemAsync(key, dto));

    [HttpPut("items/{productId:int}")]
    public async Task<ActionResult<BasketDto>> SetQuantity(Guid key, int productId, UpdateBasketItemDto dto)
        => Ok(await service.SetQuantityAsync(key, productId, dto.Quantity));

    [HttpDelete("items/{productId:int}")]
    public async Task<ActionResult<BasketDto>> RemoveItem(Guid key, int productId)
        => Ok(await service.RemoveItemAsync(key, productId));

    [HttpDelete]
    public async Task<IActionResult> Clear(Guid key)
    {
        await service.ClearAsync(key);
        return NoContent();
    }
}