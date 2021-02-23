namespace MoviesConsole
{
	using Neo4jLib;
	using System;
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
				var reader = new MoviesReader(passResult);

				var labels = await reader.ReadAllLabels().ConfigureAwait(false);
				if (labels.Count == 0)
				{
					Console.WriteLine("There are no nodes.");
				}
				else
				{
					Statics.WriteTitle($"All Labels");
					Console.WriteLine(Statics.FormatAsArray(labels));
				}

				Console.WriteLine();

				await GetNodesByLabel(reader, "Movie");
		
				Console.WriteLine();

				await GetNodesByLabel(reader, "Person");
			}
			catch (Exception ex)
			{
				Statics.WriteException(ex);
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
