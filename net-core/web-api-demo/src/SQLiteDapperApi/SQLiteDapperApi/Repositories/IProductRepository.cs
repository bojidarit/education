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

		Task<ProductModel> GetById(int id);

		Task<int> Delete(int rowId);

		Task<int> Update(int rowId, string name, string desc);
	}
}
