namespace ResultConsumeAsyncConsole
{
	using Neo4j.Driver;
	using Neo4jLib;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("*** Async Result Consumption App ***");
			var (flag, passResult) = Statics.AskForPassowrd();
			if (!flag)
			{
				Console.WriteLine(passResult);
				return;
			}

			try
			{
				using (var driver = Statics.CreateDriver(passResult))
				{
					var session = driver.AsyncSession(SessionConfigBuilder.ForDatabase("neo4j"));
					Console.WriteLine($"Session's Database is '{session.SessionConfig.Database}'");
					
					try
					{
						var names = await session
							.ReadTransactionAsync(ReadPerrsonNamesAsync)
							.ConfigureAwait(false);

						Statics.PrintList(names, "persons");
					}
					finally
					{
						await session
							.CloseAsync()
							.ConfigureAwait(false);
					}
				}
			}
			catch (Exception ex)
			{
				Statics.WriteException(ex);
			}
		}

		private static async Task<List<string>> ReadPerrsonNamesAsync(IAsyncTransaction transaction)
		{
			var result = await transaction.RunAsync(
				"MATCH (a:Person) RETURN a.name + ' (id: ' + id(a) + ')' ORDER BY a.name");

			// Asynchronous consuming results
			return await result.ToListAsync(record => record[0].As<string>());
		}
	}
}
