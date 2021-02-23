namespace Neo4jLib
{
	using Neo4j.Driver;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public static class Statics
	{
		#region Constants

		public static string DefaultUri = "bolt://localhost:7687";
		public static string DefaultUser = "neo4j";

		public static string CypherGetAllLabels = "MATCH(node) RETURN distinct labels(node) as Label";
		public static string CypherGetAllNodes = "MATCH (node:{0}) RETURN node as Node";
		public static string CypherGetAllRelationTypes = "MATCH (n:Person)-[r]-(m:Movie) RETURN distinct type(r) as RelationType";

		public static readonly string Line = new string('-', 50);

		#endregion


		#region Methods

		public static string FormatAsArray(IEnumerable<string> list)
		{
			var text = (list == null || !list.Any()) 
				? string.Empty 
				: string.Join(", ", list);

			return $"[{text}]";
		}

		public static void WriteTitle(object data)
		{
			Console.WriteLine(Line);
			Console.WriteLine($"-- {data}");
			Console.WriteLine(Line);
		}

		public static void WriteNumberedLine(int num, object data) =>
			Console.WriteLine($"{(num).ToString().PadLeft(3, '0')} {data}");

		public static string NodeToString(INode node, Formatting formatting = Formatting.None) =>
			JsonConvert.SerializeObject(node, formatting);

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

		public static void WriteException(Exception ex) =>
			Console.WriteLine($"ERROR {ex.GetType().FullName}: {ex.Message}");

		static Task<string> GetDummyTextAsync(string text) =>
			Task.FromResult(text);

		#endregion
	}
}
