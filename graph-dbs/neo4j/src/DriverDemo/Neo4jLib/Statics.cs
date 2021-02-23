namespace Neo4jLib
{
	using System;

	public static class Statics
	{
		public static string DefaultUri = "bolt://localhost:7687";
		public static string DefaultUser = "neo4j";

		public static (bool, string) AskForPassowrd()
		{
			Console.Write("Enter password (:q to quit) -> ");
			var pass = Console.ReadLine();

			if (pass == ":q")
			{
				return (false, "Just quit. Bye...");
			}

			return (true, pass);
		}
	}
}
