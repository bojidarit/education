namespace Neo4jClientLatestDemo.Dtos
{
	public class Movie
	{
		public int id { get; set; }
		public string title { get; set; }
		public int released { get; set; }
		public string tagline { get; set; }

		public static Movie Create(string title, string tagline, int released)
		{
			var movie = new Movie();
			movie.title = title;
			movie.tagline = tagline;
			movie.released = released;

			return movie;
		}
	}
}
