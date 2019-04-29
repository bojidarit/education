namespace SimpleHttpApi.Models
{
	using System;

	public class User
	{
		public User(int id, string name, DateTime joined, decimal rank, bool isPower)
		{
			this.Id = id;
			this.Name = name;
			this.DateOfJoining = joined;
			this.Rank = rank;
			this.IsPower = isPower;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DateOfJoining { get; set; }
		public decimal Rank { get; set; }
		public bool IsPower { get; set; }
	}
}