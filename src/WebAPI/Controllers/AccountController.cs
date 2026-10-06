using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAuthService auth) : ControllerBase
{
    [EnableRateLimiting("auth")]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        => Ok(await auth.RegisterAsync(dto));

    [EnableRateLimiting("auth")]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        => Ok(await auth.LoginAsync(dto));

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> Me()
    {
        var userId = User.FindFirst("sub")?.Value
















            ?? throw new UnauthorizedException("Jeton invalide.");
        return Ok(await auth.GetCurrentUserAsync(userId));
    }
}
