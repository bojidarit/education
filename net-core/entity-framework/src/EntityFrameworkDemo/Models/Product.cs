using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkDemo.Models;

public class Product
{
    public long? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public double Price { get; set; }

    public string Category { get; set; } = string.Empty;

    public Product()
    {
    }

    public Product(string name, string desc, string category, double price)
    {
        Name = name;
        Description = desc;
        Category = category;
        Price = price;
    }
}