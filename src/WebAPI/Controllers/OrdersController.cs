using Application.Common;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController(IOrderService service) : ControllerBase
{
    private string UserId => User.FindFirst("sub")?.Value
        ?? throw new UnauthorizedException("Jeton invalide.");

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto)
    {
        var order = await service.CreateAsync(UserId, dto);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetMine()
        => Ok(await service.GetForUserAsync(UserId));
















    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetById(int id)
        => Ok(await service.GetByIdAsync(id, UserId, User.IsInRole(Roles.Admin)));

    [HttpPost("{id:int}/pay")]
    public async Task<ActionResult<OrderDto>> Pay(int id)
        => Ok(await service.PayAsync(id, UserId));









    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        await service.CancelAsync(id, UserId);
        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("admin")]
 









   public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAll([FromQuery] string? status)
        => Ok(await service.GetAllAsync(status));

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<OrderDto>> UpdateStatus(int id, UpdateOrderStatusDto dto)
 





       => Ok(await service.UpdateStatusAsync(id, dto.Status));
}
