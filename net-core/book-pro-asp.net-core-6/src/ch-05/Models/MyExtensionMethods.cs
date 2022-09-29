namespace LanguageFeatures.Models;

public static class MyExtensionMethods
{
    public static decimal TotalPrices(this IEnumerable<Product?> products)
    {
        decimal total = 0M;

        foreach (Product? prod in products)
        {
            total += prod?.Price ?? 0m;
        }

        return total;
    }

    public static IEnumerable<Product?> Filter(
        this IEnumerable<Product?> products,
        Func<Product?, bool> selector)
    {
        foreach (Product? prod in products)
        {
            if (selector(prod))
            {
                yield return prod;
            }
        }
    }
}