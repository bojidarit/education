namespace Vidly.Controllers.Api
{
	using System.Linq;
	using System.Web.Http;
	using Models;
	using Dtos;
	using AutoMapper;
	using System;

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
		public IHttpActionResult GetCustomers()
		{
			return Ok(_context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>));
		}

		// GET: /api/customer/{id}
		public IHttpActionResult GetCustomer(int id)
		{
			Customer customer = _context.Customers.SingleOrDefault(c => c.Id == id);

			if (customer == null)
			{
				return NotFound();
			}

			return Ok(Mapper.Map<Customer, CustomerDto>(customer));
		}

		// POST: /api/customers
		[HttpPost]
		public IHttpActionResult CreateCustomer(CustomerDto customerDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var customer = Mapper.Map<CustomerDto, Customer>(customerDto);

			_context.Customers.Add(customer);
			_context.SaveChanges();

			customerDto.Id = customer.Id;

			return Created(new Uri($"{Request.RequestUri}/{customerDto.Id}"), customerDto);
		}

		// PUT: /api/customers/{id}
		[HttpPut]
		public IHttpActionResult UpdateCustomers(int id, CustomerDto customerDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Customer customerFromDb = _context.Customers.SingleOrDefault(c => c.Id == id);
			if (customerFromDb == null)
			{
				return NotFound();
			}

			Mapper.Map(customerDto, customerFromDb);

			_context.SaveChanges();

			return StatusCode(System.Net.HttpStatusCode.NoContent);
		}

		// DELETE: /api/customers/{id}
		[HttpDelete]
		public IHttpActionResult DeleteCustomers(int id)
		{
			Customer customerFromDb = _context.Customers.SingleOrDefault(c => c.Id == id);
			if (customerFromDb == null)
			{
				return NotFound();
			}

			_context.Customers.Remove(customerFromDb);

			_context.SaveChanges();

			return StatusCode(System.Net.HttpStatusCode.NoContent);
		}
	}
}
