namespace HelloConsole
{
	using Neo4j.Driver;
	using System;
	using System.Threading.Tasks;

	public class GreetingWriter : IDisposable
	{
		private readonly IDriver driver;
		private readonly string defaultUri = "bolt://localhost:7687";
		private readonly string defaultUser = "neo4j";

		public GreetingWriter(string password)
		{
			driver = GraphDatabase.Driver(defaultUri, AuthTokens.Basic(defaultUser, password));
		}

		public async ValueTask<string> WriteGreeting(string message)
		{
			if (driver == null)
			{
				return null;
			}

			IAsyncSession session = null;
			try
			{
				session = driver.AsyncSession();
				var record = await session.WriteTransactionAsync(tx =>
				{
					var result = tx.RunAsync("CREATE (a:Greeting) " +
									"SET a.message = $message " +
									"RETURN a.message + ', from node ' + id(a)",
									new { message }).Result;
					return result.SingleAsync();
				}).ConfigureAwait(false);

				return record[0].As<string>();
			}
			finally
			{
				await session?.CloseAsync();
			}
		}

		public void Dispose()
		{
			driver?.Dispose();
		}
	}
}
