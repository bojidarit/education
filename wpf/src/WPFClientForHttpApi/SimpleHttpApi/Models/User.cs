namespace SimpleHttpApi.Models
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
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

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public DateTime DateOfJoining { get; set; }

		[DataMember]
		public decimal Rank { get; set; }

		[DataMember]
		public bool IsPower { get; set; }
	}
}