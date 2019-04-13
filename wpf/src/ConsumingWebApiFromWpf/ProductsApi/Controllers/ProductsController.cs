namespace ProductsApi.Controllers
{
	using ProductsApi.Models;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http;

	/// <summary>
	/// Demo controller from  MS Docs article:
	/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api
	/// </summary>
	public class ProductsController : ApiController
    {
		private List<Product> _products = new List<Product>()
		{
			new Product { Id = 1, Name = "Cabbage", Category = "Vegetables", Price = 1.10M },
			new Product { Id = 2, Name = "Bananas", Category = "Fruits", Price = 2.20M },
			new Product { Id = 3, Name = "Avocado", Category = "Vegetables", Price = 3.30M },
		};

		// GET: /api/products
		public IHttpActionResult GetProducts() =>
			Ok(_products);

		// GET: /api/products/{id}
		public IHttpActionResult GetProduct(int id)
		{
			Product product = _products.SingleOrDefault(p => p.Id == id);
			if (product != null)
			{
				return Ok(product);
			}

			return NotFound();
		}

		// DELETE: /api/products/{id}
		[HttpDelete]
		public IHttpActionResult DeleteProduct(int id)
		{
			Product product = _products.SingleOrDefault(p => p.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			_products.Remove(product);  // Just the value in memory...

			return Ok(_products);
			//return StatusCode(System.Net.HttpStatusCode.NoContent);
		}
	}
}
