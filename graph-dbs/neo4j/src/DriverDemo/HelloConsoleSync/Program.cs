namespace HelloConsoleSync
{
	using Neo4j.Driver;
	using System;
	using System.IO;
	using System.Linq;

	class Program
	{
		private static string defaultUri = "bolt://localhost:7687";
		private static string defaultUser = "neo4j";

		static void Main(string[] args)
		{
			Console.Write("Enter password (:q to quit) -> ");
			var pass = Console.ReadLine();

			if (pass == ":q")
			{
				return;
			}

			var message = $"Hello {GetUniqueValue()}";
			using (var driver = GraphDatabase.Driver(defaultUri, AuthTokens.Basic(defaultUser, pass)))
			using (var session = driver.Session())
			{
				var greeting = session.WriteTransaction(tx =>
				{
					var result = tx.Run("CREATE (a:Greeting) " +
										"SET a.message = $message " +
										"RETURN a.message + ', from node ' + id(a)",
						new { message });
					return result.Single()[0].As<string>();
				});
				Console.WriteLine(greeting);
			}
		}

		static string GetUniqueValue()
		{
			var path = Path.GetRandomFileName();
			return Path.GetFileName(path).Replace(".", string.Empty);
		}
	}
}
