using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkDemo.Models;

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
            new Product("Kayak", "A boat for one person", "Water-sports", 275),
            new Product("Life-jacket", "Protective and fashionable", "Water-sports", 48.95),
            new Product("Soccer Ball", "Approved size and weight", "Soccer", 19.50),
            new Product("Corner Flags", "Give your playing field a professional touch", "Soccer", 34.95),
            new Product("Stadium", "Flat-packed 35,000-seat stadium", "Soccer", 79500),
            new Product("Thinking Cap", "Improve brain efficiency by 75%", "Chess", 16),
            new Product("Unsteady Chair", "Secretly give your opponent a disadvantage", "Chess", 29.95),
            new Product("Human Chess Board", "A fun game for the family", "Chess", 75),
            new Product("Bling-Bling King", "Gold-plated, diamond-studded King", "Chess", 1200)
        );

        context.SaveChanges();
    }
}