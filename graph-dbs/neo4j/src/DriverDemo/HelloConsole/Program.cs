namespace HelloConsole
{
	using Neo4jLib;
	using System;
	using System.IO;
	using System.Threading.Tasks;

	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("*** Hello Console Async App ***");
			var (flag, passResult) = Statics.AskForPassowrd();
			if (!flag)
			{
				Console.WriteLine(passResult);
				return;
			}

			try
			{
				var greetingWriter = new GreetingWriter(passResult);
				var result = await greetingWriter.WriteGreeting($"Hello {GetUniqueValue()}");
				Console.WriteLine(result);
			}
			catch (Exception ex)
			{
				Statics.WriteException(ex);
			}
		}

		static string GetUniqueValue()
		{
			var path = Path.GetRandomFileName();
			return Path.GetFileName(path).Replace(".", string.Empty);
		}
	}
}
