using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync()) return;

        context.Products.AddRange(
            new Product { Name = "Casque Bluetooth", Description = "Casque sans fil avec réduction de bruit", Price = 89.99m, PictureUrl = "images/products/headset.png", Brand = "Sony", Category = "Audio", StockQuantity = 50 },
            new Product { Name = "Clavier mécanique", Description = "Clavier RGB switches rouges", Price = 59.90m, PictureUrl = "images/products/keyboard.png", Brand = "Logitech", Category = "Informatique", StockQuantity = 30 },
            new Product { Name = "Souris gaming", Description = "Souris 16000 DPI", Price = 39.50m, PictureUrl = "images/products/mouse.png", Brand = "Razer", Category = "Informatique", StockQuantity = 75 },
            new Product { Name = "Enceinte portable", Description = "Enceinte étanche 20h d'autonomie", Price = 69.00m, PictureUrl = "images/products/speaker.png", Brand = "JBL", Category = "Audio", StockQuantity = 40 }
        );

        await context.SaveChangesAsync();
    }
}