namespace Vidly.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class Customer
	{
		#region Ctors

		public Customer()
		{

		}

		public Customer(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		#endregion //Ctors

		public int Id { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		public bool IsSubscribeToNewsletter { get; set; }

		public MembershipType MembershipType { get; set; }

		public byte MembershipTypeId { get; set; }

		public DateTime? BirthDate { get; set; }
	}
}