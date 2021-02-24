namespace Neo4jClientIntro
{
	using Neo4jClient;
	using Neo4jClient.Cypher;
	using Neo4jClientIntro.Dtos;
	using System;
	using System.Linq;

	class IntroProgram
	{
		#region Fields

		private static readonly Uri uri = new Uri("http://localhost:7474/db/data");
		private static readonly string user = "neo4j";
		private static readonly string pass = "neo4j";

		private static readonly GraphClient client = new GraphClient(uri, user, pass);

		#endregion


		#region Constructor

		static void Main(string[] args)
		{
			try
			{
				Demo1();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"*** ERROR *** {ex.GetType().FullName}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
			}

			Console.ReadLine();
		}

		#endregion


		#region Demos

		static void Demo1()
		{
			client.Connect();

			var query = client.Cypher
				.Match("(n:Movie)")
				.Return<Movie>(n => n.As<Movie>());

			foreach (var movie in query.Results)
			{
				Console.WriteLine(movie.title);
			}
		}


		static void Demo2()
		{
			client.Connect();
			var query = client.Cypher
				.Match("(n:Person)")
				.Return<Person>(n => n.As<Person>());
			foreach (var person in query.Results)
			{
				Console.WriteLine(person.name);
			}
		}

		static void Demo3()
		{
			client.Connect();
			var query = client.Cypher
				.Match("(n:Movie)")
				.Where((Movie n) => n.released > 1990)
				.AndWhere((Movie n) => n.released < 2000)
				.Return<Movie>(n => n.As<Movie>());
			foreach (var movie in query.Results)
			{
				Console.WriteLine(movie.title);
			}
		}

		static void Demo4()
		{
			client.Connect();
			var query = client.Cypher
				.Match("(tom:Person {name: \"Tom Hanks\"})-[:ACTED_IN]->(m:Movie)")
				.Return((tom, m) => new
				{
					person = tom.As<Person>()
					,
					movie = m.As<Movie>()
				});
			Console.WriteLine(query.Results.ElementAt(0).person.name);
			foreach (var item in query.Results)
			{
				Console.WriteLine("      {0}", item.movie.title);
			}
		}

		static void Demo5()
		{
			client.Connect();
			Movie movie = new Movie() { title = "Interstellar", released = 2014, tagline = "Mankind was born on Earth. It was never meant to die here." };
			client.Cypher
				.Create("(m:Movie {param0})")
				.WithParam("param0", movie)
				.ExecuteWithoutResults();

			Person person = new Person() { born = 1977, id = 1977, name = "Jessica Chastain" };
			client.Cypher
				.Create("(p:Person {param0})")
				.WithParam("param0", person)
				.ExecuteWithoutResults();

			var query = client.Cypher
				.Match("(m:Movie {title: \"Interstellar\"})")
				.Match("(p:Person {name: \"Jessica Chastain\"})")
				.Return((m, p) => new
				{
					movie = m.As<Movie>()
				   ,
					person = p.As<Person>()
				});

			Console.WriteLine("{0} in {1}", person.name, movie.title);
		}

		static void Demo6()
		{
			client.Connect();
			client.Cypher
				.Match("(m:Movie {title: \"Interstellar\"})")
				.Match("(p:Person {name: \"Jessica Chastain\"})")
				.CreateUnique("(p)-[:ACTED_IN {roles: [\"Murphy Cooper\"]}]->(m)")
				.ExecuteWithoutResults();

			var query = client.Cypher
				.Match("(p:Person {name: \"Jessica Chastain\"})-[:ACTED_IN]->(m:Movie)")
				.Return(m => m.As<Movie>());
			Console.WriteLine(query.Results.ElementAt(0).title);

			var pQuery = client.Cypher
				.Match("(m:Movie {title: \"Interstellar\"})<-[:ACTED_IN]-(p:Person)")
				.Return(p => p.As<Person>());
			Console.WriteLine(pQuery.Results.ElementAt(0).name);
		}

		static void Demo7()
		{
			client.Connect();
			var query = client.Cypher
				.Match("(p:Person)-[r:ACTED_IN]->(m:Movie)")
				.Return((m, p, r) => new
				{
					person = p.As<Person>()
					,
					movie = m.As<Movie>()
					,
					roles = r.As<ActedIn>()
				});
			foreach (var item in query.Results)
			{
				if (item.roles.roles != null)
					Console.WriteLine("{0}::{1}::{2}", item.person.name, item.movie.title, item.roles.roles[0]);
			}
		}

		static int Demo10(string name)
		{
			client.Connect();
			var query = client.Cypher
				.Match("(p:Person {name: {name}})-[:ACTED_IN]->(m:Movie)")
				.WithParam("name", name)
				.Return(() => Return.As<int>("count(m)"));
			Console.WriteLine(query.Query.DebugQueryText);
			Console.WriteLine(query.Query.QueryText + "::" + query.Query.QueryParameters.Count);
			return query.Results.ElementAt(0);
		}

		#endregion
	}
}
