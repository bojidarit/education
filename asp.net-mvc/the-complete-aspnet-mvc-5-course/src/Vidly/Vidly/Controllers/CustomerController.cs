namespace Vidly.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;

	public class CustomerController : Controller
    {
		private static IEnumerable<Models.Customer> customers = //new List<Models.Customer>();
			new List<Models.Customer>(Enumerable.Range(1, 5).Select(i => new Models.Customer(i, $"Customer {i}")));

        // GET: Customer
        public ActionResult Index()
        {
			return View(customers);
        }

		// GET: Customet/Details/{id}
		public ActionResult Details(int id)
		{
			Models.Customer customer = customers.FirstOrDefault(c => c.Id == id);

			if (customer != null)
			{
				return View(customer);
			}
			else
			{
				return HttpNotFound($"There is no customer with Id = {id}.");
			}
		}
    }
}