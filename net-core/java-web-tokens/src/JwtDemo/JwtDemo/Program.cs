namespace JwtDemo
{
	using JwtLib;
	using System;
	using System.IO;

	class Program
	{
		private static string help = "Parameters: username, secret, [daysValid]";
		private static string tokenTitle = "InfluxDB Json Web Token (JWT)";
		private static readonly string line = new string('-', 100);

		static void Main(string[] args)
		{
			if (args.Length == 1 && string.Compare(args[0], "help", true) == 0)
			{
				Console.WriteLine(help);
				return;
			}

			if (args.Length < 2)
			{
				Console.WriteLine($"Must supply atleast two parameters.{Environment.NewLine}{help}");
				return;
			}

			string username = args[0];
			string secret = args[1];
			int? daysValid = null;

			if (args.Length >= 3)
			{
				if (int.TryParse(args[2], out var number))
				{
					daysValid = number;
				}
			}

			Console.WriteLine($"Generated {tokenTitle}");
			var token = string.Empty;

			if (daysValid.HasValue)
			{
				token = Factory.CreateInfluxToken(username, secret, daysValid.Value);
			}
			else
			{
				token = Factory.CreateInfluxToken(username, secret);
			}

			var valid = daysValid.HasValue 
				? $"-- Valid for {daysValid.Value} days.{Environment.NewLine}"
				: string.Empty;
			var result = $"{line}{Environment.NewLine}" +
				$"-- {tokenTitle}{Environment.NewLine}" +
				$"-- For user: '{username}'{Environment.NewLine}" +
				$"-- With secret: '{secret}'{Environment.NewLine}{valid}" +
				$"{line}{Environment.NewLine}{Environment.NewLine}{token}{Environment.NewLine}";

			var path = $@".\token-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.txt";
			Console.WriteLine($"Written in file: '{path}'");

			File.WriteAllText(path, result);
		}
	}
}
