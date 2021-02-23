namespace MoviesConsole
{
	using System;
	using System.Threading.Tasks;

	class MoviesProgram
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine(await DummyAsync());
		}

		static Task<string> DummyAsync() =>
			Task.FromResult("Movies Demo....");
	}
}
