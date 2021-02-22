namespace HelloConsole
{
	using System;
	using System.IO;
	using System.Threading.Tasks;

	class Program
	{
		static async Task Main(string[] args)
		{
			Console.Write("Enter password (:q to quit) -> ");
			var pass = Console.ReadLine();

			if (pass == ":q")
			{
				return;
			}

			try
			{
				var greetingWriter = new GreetingWriter(pass);
				var result = await greetingWriter.WriteGreeting($"Hello {GetUniqueValue()}");
				Console.WriteLine(result);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR {ex.GetType().FullName}: {ex.Message}");
			}
		}

		static string GetUniqueValue()
		{
			var path = Path.GetRandomFileName();
			return Path.GetFileName(path).Replace(".", string.Empty);
		}
	}
}
