namespace Vidly.ViewModels
{
	using System.Collections.Generic;
	using Vidly.Models;

	public class RandomMovieViewModel
	{
		public Movie Movie { get; set; }
		public IEnumerable<Customer> Customers { get; set; }
	}
}