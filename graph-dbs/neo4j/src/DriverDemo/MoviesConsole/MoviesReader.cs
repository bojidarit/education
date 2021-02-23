namespace MoviesConsole
{
	using Neo4j.Driver;
	using Neo4jLib;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class MoviesReader : IDisposable
	{
		#region Fields

		private readonly IDriver driver;

		#endregion


		#region Constructor

		public MoviesReader(string password)
		{
			driver = GraphDatabase.Driver(Statics.DefaultUri, AuthTokens.Basic(Statics.DefaultUser, password));
		}

		#endregion


		#region Public Interface

		public string CurrentLabel { get; set; } = "Movie";

		public ValueTask<List<string>> ReadAllLabels() =>
			ReadStringList(GetAllLabels);

		public ValueTask<List<string>> ReadAllRelationTypes() =>
			ReadStringList(GetAllRelationTypes);

		public ValueTask<List<string>> ReadAllByLabel(string label)
		{
			if (!string.IsNullOrEmpty(label))
			{
				CurrentLabel = label;
			}

			return ReadStringList(GetAllByLabel);
		}

		public void Dispose()
		{
			driver?.Dispose();
		}

		#endregion


		#region Helpers

		public async ValueTask<List<string>> ReadStringList(Func<IAsyncTransaction, Task<List<string>>> work)
		{
			List<string> result = null;
			var session = driver.AsyncSession();

			try
			{
				result = await session
					.ReadTransactionAsync(work)
					.ConfigureAwait(false);
			}
			finally
			{
				await session?.CloseAsync();
			}

			return result;
		}

		private async Task<List<string>> GetAllRelationTypes(IAsyncTransaction transaction)
		{
			var result = new List<string>();

			var reared = await transaction
				.RunAsync(Statics.CypherGetAllRelationTypes)
				.ConfigureAwait(false);

			while (await reared.FetchAsync())
			{
				result.Add(reared.Current[0].ToString());
			}

			return result;
		}

		private async Task<List<string>> GetAllLabels(IAsyncTransaction transaction)
		{
			var result = new List<string>();

			var reared = await transaction
				.RunAsync(Statics.CypherGetAllLabels)
				.ConfigureAwait(false);

			while (await reared.FetchAsync())
			{
				var node = reared.Current[0] as IEnumerable<object>;
				result.Add(Statics.FormatAsArray(node.Select(n => n.ToString())));
			}

			return result;
		}

		private async Task<List<string>> GetAllByLabel(IAsyncTransaction transaction)
		{
			var result = new List<string>();

			var reared = await transaction
				.RunAsync(string.Format(Statics.CypherGetAllNodes, CurrentLabel))
				.ConfigureAwait(false);

			while (await reared.FetchAsync())
			{
				var node = reared.Current[0].As<INode>();
				result.Add(Statics.NodeToString(node));
			}

			return result;
		}

		#endregion
	}
}
