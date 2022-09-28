namespace LanguageFeatures.Models;

public class Product
{
    public string Name { get; set; }

    public decimal? Price { get; set; }

    public static Product[] GetProducts() =>
        new Product[]
        {
            new Product { Name = "Kayak", Price = 275M },
            new Product { Name = "Life Jacket", Price = 48.95M },
        };
}