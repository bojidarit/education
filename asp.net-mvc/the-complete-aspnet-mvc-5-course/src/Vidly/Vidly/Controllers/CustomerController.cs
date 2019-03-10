﻿namespace Vidly.Controllers
{
	using System.Data.Entity;
	using System.Linq;
	using System.Web.Mvc;
	using Vidly.Models;

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
    }
}