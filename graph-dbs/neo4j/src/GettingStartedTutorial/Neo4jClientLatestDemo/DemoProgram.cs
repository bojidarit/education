namespace Neo4jClientLatestDemo
{
	using Neo4jClient;
	using Neo4jClientLatestDemo.Dtos;
	using Newtonsoft.Json;
	using System;
	using System.Threading.Tasks;

	// ----------------------------------------------------------------------------------
	// The old 3.x library cannot connect to the new 4.x Neo4j server :-(
	// ----------------------------------------------------------------------------------
	class DemoProgram
	{
		private static readonly string address = @"http://localhost:7474/";
		private static readonly string user = "neo4j";
		private static readonly string pass = "neo4j";

		static async Task Main(string[] args)
		{
			try
			{
				await Demo1AllMovies().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"*** ERROR *** {ex.GetType().FullName}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
			}
		}

		static async Task Demo1AllMovies()
		{
			using (var client = new GraphClient(new Uri(address), user, pass))
			{
				await client.ConnectAsync();

				var query = client.Cypher
					.Match("(n:Movie)")
					//.Where("n.title = 'The Matrix'")
					.Return(n => n.As<Movie>());

				var num = 1;
				foreach (var movie in await query.ResultsAsync)
				{
					var movieJson = JsonConvert.SerializeObject(movie, Formatting.None);
					Console.WriteLine($"{(num++).ToString().PadLeft(3, '0')} {movieJson}");
				}
			}

		}
	}
}
