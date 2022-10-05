using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        var context = app.ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<StoreDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        if (context.Products.Any())
        {
            return;
        }

        // Populate database with products initially
        context.Products.AddRange(
            new Product("Kayak", "A boat for one person", "Water-sports", 275M),
            new Product("Life-jacket", "Protective and fashionable", "Water-sports", 48.95M),
            new Product("Soccer Ball", "Approved size and weight", "Soccer", 19.50M),
            new Product("Corner Flags", "Give your playing field a professional touch", "Soccer", 34.95M),
            new Product("Stadium", "Flat-packed 35,000-seat stadium", "Soccer", 79500M),
            new Product("Thinking Cap", "Improve brain efficiency by 75%", "Chess", 16M),
            new Product("Unsteady Chair", "Secretly give your opponent a disadvantage", "Chess", 29.95M),
            new Product("Human Chess Board", "A fun game for the family", "Chess", 75M),
            new Product("Bling-Bling King", "Gold-plated, diamond-studded King", "Chess", 1200M)
        );

        context.SaveChanges();
    }
}