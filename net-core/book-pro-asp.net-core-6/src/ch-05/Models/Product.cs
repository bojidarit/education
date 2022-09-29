namespace LanguageFeatures.Models;

public class Product
{
    public string Name { get; set; } = string.Empty;

    public decimal? Price { get; set; }

    public Product() { }

    public Product(string name, decimal? price = null)
    {
        Name = name;
        Price = price;
    }

    public static Product?[] GetProducts() =>
        new Product?[]
        {
            new("Kayak", 275M),
            new("Life Jacket", 48.95M),
            new("Soccer Ball", 19.50M),
            new("Corner Flag", 34.95M),
            new("Priceless one"),
            new(),
            null,
        };
}