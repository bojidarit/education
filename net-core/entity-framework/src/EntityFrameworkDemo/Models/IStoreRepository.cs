namespace EntityFrameworkDemo.Models;

public interface IStoreRepository
{
    IQueryable<Product> Products { get; }
}