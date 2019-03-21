namespace Vidly.Controllers.Api
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Web.Http;
	using Models;
	using Dtos;
	using AutoMapper;

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
		public IEnumerable<CustomerDto> GetCustomers()
		{
			return _context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>);
		}

		// GET: /api/customer/{id}
		public CustomerDto GetCustomer(int id)
		{
			Customer customer = _context.Customers.SingleOrDefault(c => c.Id == id);

			if (customer == null)
			{
				throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
			}

			return Mapper.Map<Customer, CustomerDto>(customer);
		}

		// POST: /api/customers
		[HttpPost]
		public CustomerDto CreateCustomer(CustomerDto customerDto)
		{
			if (!ModelState.IsValid)
			{
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
			}

			var customer = Mapper.Map<CustomerDto, Customer>(customerDto);

			_context.Customers.Add(customer);
			_context.SaveChanges();

			customerDto.Id = customer.Id;

			return customerDto;
		}

		// PUT: /api/customers/{id}
		[HttpPut]
		public void UpdateCustomers(int id, CustomerDto customerDto)
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

			Mapper.Map(customerDto, customerFromDb);

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
