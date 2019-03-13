namespace Vidly.Controllers
{
	using System.Data.Entity;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Web.Mvc;
	using Models;
	using ViewModels;

	public class CustomerController : Controller
	{
		private ApplicationDbContext _context;

		public CustomerController()
		{
			_context = new ApplicationDbContext();
		}

		protected override void Dispose(bool disposing)
		{
			_context.Dispose();
		}

		// GET: Customer
		public ActionResult Index()
		{
			var customers = _context.Customers.Include(c => c.MembershipType);
			return View(customers);
		}

		// Get: Customer/New
		public ActionResult New()
		{
			CustomerFormViewModel viewModel =
				new CustomerFormViewModel(_context.MembershipTypes, Customer.GetNewCustomer(), "New");

			return View("Manage", viewModel);
		}

		[HttpPost]
		public async Task<ActionResult> Create(Customer customer)
		{
			_context.Customers.Add(customer);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index", "Customer");
		}

		// GET: Customer/Details/{id}
		public ActionResult Details(int id)
		{
			Customer customer = _context.Customers
				.Include(c => c.MembershipType)
				.SingleOrDefault(c => c.Id == id);

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
			Customer customer = _context.Customers
				.SingleOrDefault(c => c.Id == id);

			if (customer != null)
			{
				CustomerFormViewModel viewModel =
					new CustomerFormViewModel(_context.MembershipTypes, customer, "Edit");

				return View("Manage", viewModel);
			}
			else
			{
				return HttpNotFound($"There is no customer with Id = {id}.");
			}
		}
	}
}