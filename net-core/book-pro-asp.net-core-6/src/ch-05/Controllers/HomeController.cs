namespace LanguageFeatures.Controllers;

public class HomeController : Controller
{
    public async Task<ViewResult> Index()
    {
        var output = new List<string>();

        await foreach (var length in MyAsyncMethods.GetPageLengths(output, "apress.com", "microsoft.com", "amazon.com"))
        {
            output.Add($"Page length = {length}");
        }

        return View(output);
    }

    public async Task<ViewResult> IndexGetLengthAllAtOnce()
    {
        var output = new List<string>();

        foreach (var length in await MyAsyncMethods.GetPageLengthsAtOnce(output, "apress.com", "microsoft.com", "amazon.com"))
        {
            output.Add($"Page length = {length}");
        }

        return View(output);
    }

    public async Task<ViewResult> IndexGetLength()
    {
        var length = await MyAsyncMethods.GetPageLength();

        return View(new string[] { $"Page length = {length}" });
    }

    public ViewResult IndexProducts()
    {
        var allProducts = Product.GetProducts().Select(p => FormatProduct(p));
        var list = new List<string>(allProducts);

        var products = Product.GetProducts();
        ShoppingCart cart = new() { Products = products };
        list.Add(FormatTotal("cart", cart));
        list.Add(FormatTotal("array", products));

        // Applying filters
        decimal priceFilterTotal = products
            .Filter(p => (p?.Price ?? 0) >= 20)
            .TotalPrices();
        list.Add(FormatTotal("filter by price", priceFilterTotal));

        decimal nameFilterTotal = products
            .Filter(p => p?.Name?.StartsWith("S") ?? false)
            .TotalPrices();
        list.Add(FormatTotal("filter by name", nameFilterTotal));

        return View(list);
    }

    #region Helpers

    private string FormatTotal(string subTitle, IEnumerable<Product?> products) =>
        FormatTotal(subTitle, products.TotalPrices());

    private string FormatTotal(string subTitle, decimal price) =>
        $"Total ({subTitle}): {price:C2}";

    private string FormatProduct(Product? product)
    {
        if (product == null)
        {
            return "'<NULL>'";
        }

        return $"Name: '{product.Name}', Price: {product.Price:C2}";
    }

    #endregion
}