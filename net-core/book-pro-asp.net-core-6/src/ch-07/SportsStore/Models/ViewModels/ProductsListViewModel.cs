namespace SportsStore.Models.ViewModels;

public class ProductsListViewModel
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

    public PagingInfo PagingInfo { get; set; } = new();

    public static ProductsListViewModel Create(
        IEnumerable<Product> products,
        PagingInfo pagingInfo) =>
        new ProductsListViewModel
        {
            Products = products,
            PagingInfo = pagingInfo,
        };
}