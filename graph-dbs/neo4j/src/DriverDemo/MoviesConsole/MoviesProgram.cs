namespace MoviesConsole
{
	using Neo4jLib;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	class MoviesProgram
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("*** Movies Console App ***");
			var (flag, passResult) = Statics.AskForPassowrd();
			if (!flag)
			{
				Console.WriteLine(passResult);
				return;
			}

			try
			{
				using (var reader = new MoviesReader(passResult))
				{
					var labels = await reader.ReadAllLabels().ConfigureAwait(false);
					PrintObjectsList(labels, "labels");

					Console.WriteLine();

					var realations = await reader.ReadAllRelationTypes().ConfigureAwait(false);
					PrintObjectsList(realations.Select(i => $"\"{i}\"").ToList(), "relation types");

					WaitForKey();

					await GetNodesByLabel(reader, "Movie");

					WaitForKey();

					await GetNodesByLabel(reader, "Person");
				}
			}
			catch (Exception ex)
			{
				Statics.WriteException(ex);
			}
		}

		private static void WaitForKey()
		{
			Console.WriteLine();
			Console.Write("Press any key to continue...");
			Console.ReadKey();
			Console.WriteLine();
		}

		static void PrintObjectsList(List<string> list, string label)
		{
			Statics.WriteTitle($"All {label}");
			if (list.Count == 0)
			{
				Console.WriteLine($"*There are no {label}.");
			}
			else
			{
				Console.WriteLine(Statics.FormatAsArray(list));
			}
		}

		static async ValueTask GetNodesByLabel(MoviesReader reader, string label)
		{
			Statics.WriteTitle($"All ({label})");

			var num = 1;
			var list = await reader.ReadAllByLabel(label).ConfigureAwait(false);

			if (list.Count == 0)
			{
				Console.WriteLine($"*Note: There are no nodes with label ({label}).");
			}
			else
			{
				list.ForEach(m => Statics.WriteNumberedLine(num++, m));
			}
		}
	}
}
