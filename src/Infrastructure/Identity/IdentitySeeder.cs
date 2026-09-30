using Application.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider sp, IConfiguration config)
    {
        var roles = sp.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in new[] { Roles.Admin, Roles.Customer })
        {
            if (!await roles.RoleExistsAsync(role))
                await roles.CreateAsync(new IdentityRole(role));
        }

        // Pas de compte admin sans mot de passe configuré (user-secrets ou variable d'environnement)
        var adminPassword = config["Seed:AdminPassword"];
        if (string.IsNullOrWhiteSpace(adminPassword)) return;

        var users = sp.GetRequiredService<UserManager<ApplicationUser>>();
        const string email = "admin@shop.local";
        if (await users.FindByEmailAsync(email) is not null) return;

        var admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = "Administrateur",
            EmailConfirmed = true,
        };

        var result = await users.CreateAsync(admin, adminPassword);
        if (result.Succeeded) await users.AddToRoleAsync(admin, Roles.Admin);
    }
}