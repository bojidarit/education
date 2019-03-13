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

		[Display(Name = "Membership Type")]
		public byte MembershipTypeId { get; set; }

		[Display(Name = "Date of Birth")]
		public DateTime? BirthDate { get; set; }

		#region Methods

		public static Customer GetNewCustomer() =>
			new Customer(-1, string.Empty);

		public override string ToString() =>
			$"{this.GetType().Name} {{ Name = '{this.Name}', " +
			$"BirthDate = {this.BirthDate?.ToShortDateString() ?? "<NULL>"}, " +
			$"MembershipTypeId = {this.MembershipTypeId} }} ";

		#endregion // Methods
	}
}