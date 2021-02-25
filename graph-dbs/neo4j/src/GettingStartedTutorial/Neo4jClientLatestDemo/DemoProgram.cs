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
	static class DemoProgram
	{
		#region Fields

		private static readonly string address = @"http://localhost:7474/";
		private static readonly string user = "neo4j";
		private static readonly string pass = "neo4j";

		#endregion


		#region Constructor

		static async Task Main(string[] args)
		{
			Console.WriteLine("Choose a Demo from the list:");
			Console.WriteLine("\t1) All Movies.");
			Console.WriteLine("\t2) All Persons.");
			Console.WriteLine("\t3) Movies From 90's.");
			Console.WriteLine("\t4) In which movies does Tom Hanks star?");
			Console.Write("Enter the number of the demo here: ");
			var text = Console.ReadLine();
			if (!int.TryParse(text, out var number))
			{
				return;
			}

			Console.WriteLine();

			try
			{
				switch (number)
				{
					case 1:
						await DemoAllNodesByLabel<Movie>(Label.Movie).ConfigureAwait(false);
						break;

					case 2:
						await DemoAllNodesByLabel<Person>(Label.Person).ConfigureAwait(false);
						break;

					case 3:
						await DemoMoviesFrom90s().ConfigureAwait(false);
						break;

					case 4:
						await DemoTomHanksMovies().ConfigureAwait(false);
						break;

					default:
						Console.WriteLine("The number is not supported. Bye...");
						break;
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine($"*** ERROR *** {ex.GetType().FullName}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
			}
		}

		#endregion


		#region Demos

		static async Task DemoAllNodesByLabel<T>(Label label)
		{
			using (var client = CreateGraphClient())
			{
				await client.ConnectAsync();

				var query = client.Cypher
					.Match($"(n:{label})")
					.Return(n => n.As<T>());

				var num = 1;
				foreach (var node in await query.ResultsAsync)
				{
					PrintNode(num++, node);
				}
			}
		}

		static async Task DemoMoviesFrom90s()
		{
			using (var client = CreateGraphClient())
			{
				await client.ConnectAsync();

				var query = client.Cypher
					.Match("(n:Movie)")
					.Where((Movie n) => n.released >= 1990)
					.AndWhere((Movie n) => n.released < 2000)
					.Return(n => n.As<Movie>());
				//.Limit(3);

				var num = 1;
				foreach (var movie in await query.ResultsAsync)
				{
					PrintNode(num++, movie);
				}
			}
		}

		static async Task DemoTomHanksMovies()
		{
			using (var client = CreateGraphClient())
			{
				await client.ConnectAsync();

				var query = client.Cypher
					.Match("(p:Person {name: 'Tom Hanks'})-[:ACTED_IN]->(m:Movie)")
					.Return((p, m) => new { Person = p.As<Person>(), Movie = m.As<Movie>() })
					.OrderBy("m.released");

				var num = 1;
				foreach (var item in await query.ResultsAsync)
				{
					if (num == 1)
					{
						var json = JsonConvert.SerializeObject(item.Person, Formatting.None);
						Console.WriteLine($"Actor: {json}");
						Console.WriteLine("Role: ACTED_IN");
						Console.WriteLine();
					}

					PrintNode(num++, item.Movie);
				}
			}
		}

		#endregion


		#region Helpers

		private static GraphClient CreateGraphClient() =>
			new GraphClient(new Uri(address), user, pass);

		private static void PrintNode(int num, object obj)
		{
			var json = JsonConvert.SerializeObject(obj, Formatting.None);
			Console.WriteLine($"{(num++).ToString().PadLeft(3, '0')} {json}");
		}

		#endregion
	}
}
