namespace Vidly.ViewModels
{
	using Models;
	using System.Collections.Generic;

	public class NewCustomerViewModel
	{
		public NewCustomerViewModel(IEnumerable<MembershipType> membershipTypes, Customer customer)
		{
			this.MembershipTypes = membershipTypes;
			this.Customer = customer;
		}

		public IEnumerable<MembershipType> MembershipTypes { get; private set; }
		public Customer Customer { get; set; }
	}
}