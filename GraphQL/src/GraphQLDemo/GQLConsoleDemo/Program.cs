namespace GQLConsoleDemo
{
	using System;
	using System.Threading.Tasks;
	using GraphQL;
	using GraphQL.Types;

	class Program
	{
		static async Task Main(string[] args)
		{
			var schema = Schema.For(@"
			  type Query {
				hello: String
			  }
			");

			foreach (var item in schema.AllTypes)
			{
				Console.WriteLine(item);
			}

			//var json = await schema.ExecuteAsync(_ =>
			//{
			//	_.Query = "{ hello }";
			//	_.Root = new { Hello = "Hello World!" };
			//});

			//Console.WriteLine(json);
		}
	}
}
