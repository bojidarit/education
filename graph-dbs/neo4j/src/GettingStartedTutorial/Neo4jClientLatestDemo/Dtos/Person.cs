namespace Neo4jClientLatestDemo.Dtos
{
	public class Person
	{
		public int id { get; set; }
		public string name { get; set; }
		public int born { get; set; }

		public static Person Create(string name, int born)
		{
			var person = new Person();
			person.name = name;
			person.born = born;

			return person;
		}
	}
}
