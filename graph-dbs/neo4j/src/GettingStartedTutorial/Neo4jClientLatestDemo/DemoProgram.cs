namespace Neo4jClientLatestDemo
{
	using Neo4jClient;
	using Neo4jClientLatestDemo.Dtos;
	using Newtonsoft.Json;
	using System;
	using System.Linq;
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

		private static readonly string movieTitle = "Interstellar";
		private static readonly string personName = "Jessica Chastain";


		#endregion


		#region Constructor

		static async Task Main(string[] args)
		{
			Console.WriteLine("Choose a Demo from the list:");
			Console.WriteLine("\t1) All Movies;");
			Console.WriteLine("\t2) All Persons;");
			Console.WriteLine("\t3) Movies From 90's;");
			Console.WriteLine("\t4) In which movies does Tom Hanks star;");
			Console.WriteLine("\t5) Create Movie And Actor;");
			Console.WriteLine("\t6) Relate previously created Movie And Actor with ACTED_IN;");
			Console.WriteLine("\t ... or negative number to quit.");
			Console.Write("Enter the number of the demo here: ");
			var text = Console.ReadLine();

			if (!int.TryParse(text, out var number))
			{
				return;
			}

			Console.WriteLine();

			if (number <= 0)
			{
				Console.WriteLine("You have quit. Bye...");
				return;
			}

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

					case 5:
						await CreateMovieAndActor(insert: true).ConfigureAwait(false);
						break;

					case 6:
						await CreateActedInRelation(create: true).ConfigureAwait(false);
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

		static async Task CreateMovieAndActor(bool insert)
		{
			var pressAnyKey = "Pres any key to continue.";

			using (var client = CreateGraphClient())
			{
				await client.ConnectAsync();

				if (insert)
				{
					// Create new movie
					var movie = Movie.Create(
						movieTitle,
						"Mankind was born on Earth. It was never meant to die here.",
						2014);

					await client.Cypher
						.Create("(m:Movie $movie)")
						.WithParam("movie", movie)
						.ExecuteWithoutResultsAsync();

					Console.WriteLine($"Created movie with title '{movie.title}'" +
						$"{Environment.NewLine}{pressAnyKey}");
					Console.ReadKey();

					// Create new actor
					var person = Person.Create(personName, 1977);

					await client.Cypher
						.Create("(p:Person $person)")
						.WithParam("person", person)
						.ExecuteWithoutResultsAsync();

					Console.WriteLine($"Created person with name '{personName}'" +
						$"{Environment.NewLine}{pressAnyKey}");
					Console.ReadKey();
				}

				// Query both movie and person

				// MATCH(p:Person) WHERE p.name = 'Jessica Chastain' RETURN p
				// MATCH(p:Person) WHERE p.name = 'Jessica Chastain' DETACH DELETE p

				// MATCH (m:Movie) WHERE m.title = 'Interstellar' RETURN m
				// MATCH (m:Movie) WHERE m.title = 'Interstellar' DETACH DELETE m

				// MATCH(p:Person {name: 'Jessica Chastain'}) MATCH(m:Movie {title: 'Interstellar'}) RETURN m,p
				var query = client.Cypher
					.Match("(p:Person)")
					.Where((Person p) => p.name == personName)
					.Match("(m:Movie)")
					.Where((Movie m) => m.title == movieTitle)
					.Return((p, m) => new { Person = p.As<Person>(), Movie = m.As<Movie>() });

				var result = await query.ResultsAsync;
				var item = result.FirstOrDefault();
				if (item != null)
				{
					Console.WriteLine("The nodes just created are: ");
					PrintNode(0, item.Movie);
					PrintNode(0, item.Person);
				}

				//foreach (var item in await query.ResultsAsync)
				//{
				//	PrintNode(0, item.Movie);
				//	PrintNode(0, item.Person);
				//}
			}
		}

		static async Task CreateActedInRelation(bool create)
		{
			using (var client = CreateGraphClient())
			{
				await client.ConnectAsync();

				if (create)
				{
					// MATCH(p:Person {name: 'Jessica Chastain'}) MATCH(m:Movie {title: 'Interstellar'}) RETURN m,p
					await client.Cypher
						.Match("(p:Person)")
						.Where((Person p) => p.name == personName)
						.Match("(m:Movie)")
						.Where((Movie m) => m.title == movieTitle)
						.Create("(p)-[:ACTED_IN {roles: ['Murphy Cooper']}]->(m)")
						.ExecuteWithoutResultsAsync();
				}

				// MATCH(p:Person {name: 'Jessica Chastain'})-[r:ACTED_IN]->(m:Movie) RETURN p,r,m
				var query = client.Cypher
					.Match("(p:Person)-[r:ACTED_IN]->(m:Movie)")
					.Where((Person p) => p.name == personName)
					.Return((p, r, m) => new
					{
						Person = p.As<Person>(),
						Relation = r.As<ActedIn>(),
						Movie = m.As<Movie>()
					});

				foreach (var item in await query.ResultsAsync)
				{
					PrintNode(0, item.Person);
					PrintNode(0, item.Relation);
					PrintNode(0, item.Movie);
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
