namespace ProductsApi.Controllers
{
	using ProductsApi.DAL;
	using ProductsApi.Models;
	using System.Linq;
	using System.Web.Http;
	using System.Data.Entity;

	/// <summary>
	/// Demo controller from  MS Docs article:
	/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api
	/// 
	/// Tutorial: Get Started with Entity Framework 6 Code First using MVC 5
	/// https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application
	/// </summary>
	public class ProductsController : ApiController
	{
		StoreContext _context;

		public ProductsController()
		{
			_context = new StoreContext();
		}

		protected override void Dispose(bool disposing)
		{
			_context.Dispose();
		}

		// GET: /api/products
		public IHttpActionResult GetProducts() =>
			Ok(_context.Products.Include(p => p.Category));

		// GET: /api/products/{id}
		public IHttpActionResult GetProduct(int id)
		{
			Product product = _context.Products
				.Include(p => p.Category)
				.SingleOrDefault(p => p.Id == id);

			if (product != null)
			{
				return Ok(product);
			}

			return NotFound();
		}

		//POST: /api/products
		[HttpPost]
		public IHttpActionResult CreateProduct(Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Products.Add(product);

			_context.SaveChanges();

			Category category = _context.Categories.SingleOrDefault(c => c.Id == product.CategoryId);
			product.Category = category;

			return Created($"{base.Request.RequestUri}/{product.Id}", product);
		}

		//PUT: /api/products/{id}
		[HttpPut]
		public IHttpActionResult UpdateProduct(int id, Product product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Product productFormDb = _context.Products.SingleOrDefault(p => p.Id == product.Id);
			if (productFormDb == null)
			{
				return NotFound();
			}

			productFormDb.Name = product.Name;
			productFormDb.CategoryId = product.CategoryId;
			productFormDb.Price = product.Price;

			_context.SaveChanges();

			//return Ok(productFormDb);
			return StatusCode(System.Net.HttpStatusCode.NoContent);
		}

		// DELETE: /api/products/{id}
		[HttpDelete]
		public IHttpActionResult DeleteProduct(int id)
		{
			Product product = _context.Products.SingleOrDefault(p => p.Id == id);
			if (product == null)
			{
				return Ok(product);
			}

			_context.Products.Remove(product);

			_context.SaveChanges();

			return StatusCode(System.Net.HttpStatusCode.NoContent);
		}
	}
}
