namespace Vidly.Controllers.Api
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Web.Http;
	using Models;

	public class CustomersController : ApiController
	{
		private ApplicationDbContext _context;

		public CustomersController()
		{
			_context = new ApplicationDbContext();
		}

		protected override void Dispose(bool disposing)
		{
			_context.Dispose();
		}

		// GET: /api/customers
		public IEnumerable<Customer> GetCustomers()
		{
			return _context.Customers.Include(c => c.MembershipType);
		}

		// GET: /api/customer/{id}
		public Customer GetCustomer(int id)
		{
			Customer customer = _context.Customers.SingleOrDefault(c => c.Id == id);

			if (customer == null)
			{
				throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
			}

			return customer;
		}

		// POST: /api/customers
		[HttpPost]
		public Customer CreateCustomer(Customer customer)
		{
			if (!ModelState.IsValid)
			{
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
			}

			_context.Customers.Add(customer);
			_context.SaveChanges();

			return customer;
		}

		// PUT: /api/customers/{id}
		[HttpPut]
		public void UpdateCustomers(int id, Customer customer)
		{
			if (!ModelState.IsValid)
			{
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
			}

			Customer customerFromDb = _context.Customers.SingleOrDefault(c => c.Id == id);
			if (customerFromDb == null)
			{
				throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
			}

			customerFromDb.Name = customer.Name;
			customerFromDb.BirthDate = customer.BirthDate;
			customerFromDb.IsSubscribeToNewsletter = customer.IsSubscribeToNewsletter;
			customerFromDb.MembershipTypeId = customer.MembershipTypeId;

			_context.SaveChanges();
		}

		// DELETE: /api/customers/{id}
		[HttpDelete]
		public void DeleteCustomers(int id)
		{
			Customer customerFromDb = _context.Customers.SingleOrDefault(c => c.Id == id);
			if (customerFromDb == null)
			{
				throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
			}

			_context.Customers.Remove(customerFromDb);

			_context.SaveChanges();
		}
	}
}
