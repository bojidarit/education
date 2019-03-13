namespace Vidly.ViewModels
{
	using Models;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;

	public class CustomerFormViewModel
	{
		public CustomerFormViewModel(IEnumerable<MembershipType> membershipTypes, 
			Customer customer, string title)
		{
			this.MembershipTypesList =
				membershipTypes.Select(i => new SelectListItem()
				{
					Text = i.Name,
					Value = i.Id.ToString(),
					Selected = false
				});

			this.Customer = customer;

			this.Title = title;
		}

		public IEnumerable<SelectListItem> MembershipTypesList { get; private set; }

		public Customer Customer { get; private set; }

		public string Title { get; private set; }
	}
}