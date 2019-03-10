namespace Vidly.ViewModels
{
	using Models;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;

	public class NewCustomerViewModel
	{
		public NewCustomerViewModel(IEnumerable<MembershipType> membershipTypes, Customer customer)
		{
			this.MembershipTypesList =
				membershipTypes.Select(i => new SelectListItem()
				{
					Text = i.Name,
					Value = i.Id.ToString(),
					Selected = false
				});

			this.Customer = customer;
		}

		public IEnumerable<SelectListItem> MembershipTypesList { get; private set; }

		public Customer Customer { get; private set; }
	}
}