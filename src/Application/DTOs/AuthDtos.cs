using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record RegisterDto(
    [Required, EmailAddress, StringLength(256)] string Email,
    [Required, StringLength(100, MinimumLength = 8)] string Password,
    [Required, StringLength(100, MinimumLength = 2)] string DisplayName);

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password);

public record UserDto(string Id, string Email, string DisplayName, IReadOnlyList<string> Roles);

public record AuthResponseDto(string Token, DateTime ExpiresAtUtc, UserDto User);
