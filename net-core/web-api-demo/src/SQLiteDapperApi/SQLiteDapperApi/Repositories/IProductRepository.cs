namespace SQLiteDapperApi.Repositories
{
	using SQLiteDapperApi.Models;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface IProductRepository
	{
		Task<int> Create(ProductModel product);

		Task<IEnumerable<ProductModel>> Get();

		Task<ProductModel> GetByName(string name);

		Task Delete(int rowId);
	}
}
