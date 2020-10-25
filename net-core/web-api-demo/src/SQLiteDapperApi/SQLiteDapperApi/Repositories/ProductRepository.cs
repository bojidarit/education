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
		#region Constants

		private readonly DatabaseConfig databaseConfig;

		private const string table = "Product";
		private readonly string getSQL =
			$"SELECT rowid AS Id, Name, Description FROM {table}";
		private readonly string createSQL =
			$"INSERT INTO {table} (Name, Description) VALUES (@Name, @Description);";
		private readonly string deleteSQL =
			$"DELETE FROM {table} WHERE rowid = @Rowid;";
		private readonly string updateSQL =
			$"UPDATE {table} SET Name = @Name, Description = @Description WHERE rowid = @Rowid;";

		#endregion

		public ProductRepository(DatabaseConfig databaseConfig)
		{
			this.databaseConfig = databaseConfig;
		}

		#region CRUD Methods

		public async Task<IEnumerable<ProductModel>> Get()
		{
			using (var connection = new SqliteConnection(databaseConfig.Name))
			{
				return await connection.QueryAsync<ProductModel>($"{getSQL};");
			}
		}

		public async Task<ProductModel> GetByName(string name)
		{
			using (var connection = new SqliteConnection(databaseConfig.Name))
			{
				var list = await connection.QueryAsync<ProductModel>(
					$"{getSQL} WHERE Name = @Name;", new { Name = name });

				return list.FirstOrDefault();
			}
		}

		public async Task<ProductModel> GetById(int id)
		{
			using (var connection = new SqliteConnection(databaseConfig.Name))
			{
				var list = await connection.QueryAsync<ProductModel>(
					$"{getSQL} WHERE rowid = @Id;", new { Id = id });

				return list.FirstOrDefault();
			}
		}

		public async Task<int> Create(ProductModel product)
		{
			using (var connection = new SqliteConnection(databaseConfig.Name))
			{
				return await connection.ExecuteAsync(createSQL, product);
			}
		}

		public async Task<int> Delete(int rowid)
		{
			using (var connection = new SqliteConnection(databaseConfig.Name))
			{
				return await connection.ExecuteAsync(deleteSQL, new { Rowid = rowid });
			}
		}

		public async Task<int> Update(int rowid, string name, string desc)
		{
			using (var connection = new SqliteConnection(databaseConfig.Name))
			{
				return await connection.ExecuteAsync(
					updateSQL,
					new
					{
						Rowid = rowid,
						Name = name,
						Description = desc,
					});
			}
		}

		#endregion
	}
}
