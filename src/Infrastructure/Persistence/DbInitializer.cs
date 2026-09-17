using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                new("PC Portable Gamer", "Intel i7, 16GB RAM, RTX 4060", 1299.99m, 10),
                new("Écran 27 pouces 144Hz", "Dalle IPS 1ms, résolution QHD", 249.50m, 15),
                new("Clavier Mécanique RGB", "Switchs Red, disposition AZERTY", 89.90m, 25)
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}