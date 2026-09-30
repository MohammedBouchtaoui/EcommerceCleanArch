using Application.Common;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AuthService(UserManager<ApplicationUser> users, JwtTokenService tokens) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim();
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = dto.DisplayName.Trim(),
        };

        var result = await users.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));

        await users.AddToRoleAsync(user, Roles.Customer);
        return await BuildResponseAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await users.FindByEmailAsync(dto.Email.Trim())
            ?? throw InvalidCredentials();

        if (await users.IsLockedOutAsync(user))
            throw new UnauthorizedException("Compte temporairement verrouillé. Réessayez dans quelques minutes.");

        if (!await users.CheckPasswordAsync(user, dto.Password))
        {
            await users.AccessFailedAsync(user);
            throw InvalidCredentials();
        }

        await users.ResetAccessFailedCountAsync(user);
        return await BuildResponseAsync(user);
    }

    public async Task<UserDto> GetCurrentUserAsync(string userId)
    {
        var user = await users.FindByIdAsync(userId)
            ?? throw new UnauthorizedException("Utilisateur introuvable.");
        return ToDto(user, await users.GetRolesAsync(user));
    }

    private async Task<AuthResponseDto> BuildResponseAsync(ApplicationUser user)
    {
        var roles = await users.GetRolesAsync(user);
        var (token, expires) = tokens.Create(user, roles);
        return new AuthResponseDto(token, expires, ToDto(user, roles));
    }

    private static UserDto ToDto(ApplicationUser u, IEnumerable<string> roles)
        => new(u.Id, u.Email!, u.DisplayName, roles.ToList());

    private static UnauthorizedException InvalidCredentials()
        => new("Email ou mot de passe incorrect.");
}