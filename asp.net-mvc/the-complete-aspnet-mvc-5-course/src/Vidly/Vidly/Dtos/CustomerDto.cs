namespace Vidly.Dtos
{
	using System;

	public class CustomerDto
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public bool IsSubscribeToNewsletter { get; set; }

		public byte MembershipTypeId { get; set; }

		public DateTime? BirthDate { get; set; }
	}
}