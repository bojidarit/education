namespace Vidly.Controllers
{
	using System.Data.Entity;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web;
	using System.Web.Mvc;
	using Microsoft.AspNet.Identity.Owin;
	using Models;
	using ViewModels;

	public class CustomerController : Controller
	{
		public CustomerController()
		{
		}

		public ApplicationDbContext DbContext =>
			HttpContext.GetOwinContext().Get<ApplicationDbContext>();

		// GET: Customer
		public ActionResult Index()
		{
			var customers = this.DbContext.Customers.Include(c => c.MembershipType);
			return View(customers);
		}

		private ActionResult CreateNew(Customer customer)
		{
			CustomerFormViewModel viewModel = new CustomerFormViewModel(this.DbContext.MembershipTypes, customer);

			return View("ManageCustomer", viewModel);
		}

		// Get: Customer/New
		public ActionResult New()
		{
			return CreateNew(Customer.GetNewCustomer());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Save(Customer customer)
		{
			if (!ModelState.IsValid)
			{
				//return Content($"Customer model is NOT valid.{System.Environment.NewLine}{customer}");
				var viewModel = new CustomerFormViewModel(this.DbContext.MembershipTypes, customer);

				return View("ManageCustomer", viewModel);
			}

			if (customer.Id <= 0)
			{
				this.DbContext.Customers.Add(customer);
			}
			else
			{
				Customer customerFromDb = this.DbContext.Customers
					.SingleOrDefault(c => c.Id == customer.Id);
				if (customerFromDb != null)
				{
					customerFromDb.Name = customer.Name;
					customerFromDb.BirthDate = customer.BirthDate;
					customerFromDb.MembershipTypeId = customer.MembershipTypeId;
					customerFromDb.IsSubscribeToNewsletter = customer.IsSubscribeToNewsletter;
				}
			}

			await this.DbContext.SaveChangesAsync();
			return RedirectToAction("Index", "Customer");
		}

		// GET: Customer/Details/{id}
		public ActionResult Details(int id)
		{
			Customer customer = GetCustomerById(id);

			if (customer != null)
			{
				return View(customer);
			}
			else
			{
				return HttpNotFound($"There is no customer with Id = {id}.");
			}
		}

		// GET: Customer/Edit/{id}
		public ActionResult Edit(int id)
		{
			Customer customer = this.DbContext.Customers
				.SingleOrDefault(c => c.Id == id);

			if (customer == null)
			{
				return HttpNotFound($"There is no customer with Id = {id}.");
			}

			CustomerFormViewModel viewModel = new CustomerFormViewModel(this.DbContext.MembershipTypes, customer);

			return View("ManageCustomer", viewModel);
		}

		#region Helpers

		private Customer GetCustomerById(int id)
		{
			Customer customer = this.DbContext.Customers
				.Include(c => c.MembershipType)
				.SingleOrDefault(c => c.Id == id);

			return customer;
		}

		#endregion //Helpers
	}
}