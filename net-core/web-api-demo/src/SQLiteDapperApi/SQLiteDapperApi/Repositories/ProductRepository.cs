namespace SQLiteDapperApi.Repositories
{
	using Dapper;
	using Microsoft.Data.Sqlite;
	using SQLiteDapperApi.Database;
	using SQLiteDapperApi.Models;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class ProductRepository : IProductRepository
	{
		private readonly DatabaseConfig databaseConfig;

		public ProductRepository(DatabaseConfig databaseConfig)
		{
			this.databaseConfig = databaseConfig;
		}

		#region CRUD Methods

		public async Task<int> Create(ProductModel product)
		{
			using(var connection = new SqliteConnection(databaseConfig.Name))
			{
				return await connection.ExecuteAsync(
					"INSERT INTO Product (Name, Description)" +
					"VALUES (@Name, @Description);", product);
			}
		}

		public async Task<IEnumerable<ProductModel>> Get()
		{
			using (var connection = new SqliteConnection(databaseConfig.Name))
			{
				return await connection.QueryAsync<ProductModel>(
					"SELECT rowid AS Id, Name, Description FROM Product;");
			}
		}

		public async Task<ProductModel> GetByName(string name)
		{
			using (var connection = new SqliteConnection(databaseConfig.Name))
			{
				var list = await connection.QueryAsync<ProductModel>(
					"SELECT rowid AS Id, Name, Description WHERE Name = @name;", name);

				return list.FirstOrDefault();
			}
		}

		public async Task Delete(int rowid)
		{
			using (var connection = new SqliteConnection(databaseConfig.Name))
			{
				await connection.ExecuteAsync(
					"DELETE FROM Product WHERE rowid = @rowid;", rowid);
			}
		}

		#endregion
	}
}
