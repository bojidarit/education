namespace Vidly.Models
{
	public class Movie
	{
		public Movie()
		{

		}

		public Movie(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set; }
	}
}