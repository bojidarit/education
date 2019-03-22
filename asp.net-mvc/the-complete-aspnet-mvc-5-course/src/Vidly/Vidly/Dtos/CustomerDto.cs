namespace Vidly.Dtos
{
	using System;
	using System.ComponentModel.DataAnnotations;

	public class CustomerDto
	{
		public int Id { get; set; }

		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		public bool IsSubscribeToNewsletter { get; set; }

		[Display(Name = "Membership Type")]
		public byte MembershipTypeId { get; set; }

		[Display(Name = "Date of Birth")]
		public DateTime? BirthDate { get; set; }
	}
}