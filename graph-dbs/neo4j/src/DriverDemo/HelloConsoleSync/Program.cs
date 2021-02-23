namespace HelloConsoleSync
{
	using Neo4j.Driver;
	using Neo4jLib;
	using System;
	using System.IO;
	using System.Linq;

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("*** Hello Console Sync App ***");
			var (flag, passResult) = Statics.AskForPassowrd();
			if (!flag)
			{
				Console.WriteLine(passResult);
				return;
			}

			var message = $"Hello {GetUniqueValue()}";
			using (var driver = Statics.CreateDriver(passResult))
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
