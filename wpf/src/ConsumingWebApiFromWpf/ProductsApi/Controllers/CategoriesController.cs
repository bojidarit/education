namespace ProductsApi.Controllers
{
	using ProductsApi.DAL;
	using ProductsApi.Models;
	using System.Linq;
	using System.Web.Http;
	using System.Data.Entity;

	public class CategoriesController : ApiController
	{
		StoreContext _context;

		public CategoriesController()
		{
			_context = new StoreContext();
		}

		protected override void Dispose(bool disposing)
		{
			_context.Dispose();
		}

		// GET: /api/categories
		public IHttpActionResult GetCategories()
		{
			// Patch: StoreInitializer do not run for new databases, so seed some categories
			if (!_context.Categories.Any())
			{
				Category[] categories = new Category[]
					{
						new Category{ Name = "Vegetables" },
						new Category{ Name = "Fruits" }
					};

				_context.Categories.AddRange(categories);
				_context.SaveChanges();
			}

			return Ok(_context.Categories);
		}

		// GET: /api/categories
		public IHttpActionResult GetCategory(int id)
		{
			Category category = _context.Categories.SingleOrDefault(c => c.Id == id);

			if (category != null)
			{
				return Ok(category);
			}

			return NotFound();
		}

		//DELETE: /api/categories/{id}
		public IHttpActionResult DeleteCategoty(int id)
		{
			Category category = _context.Categories.SingleOrDefault(c => c.Id == id);

			if (category == null)
			{
				return NotFound();
			}

			_context.Categories.Remove(category);

			_context.SaveChanges();

			return StatusCode(System.Net.HttpStatusCode.NoContent);
		}

		// POST: /api/categories
		[HttpPost]
		public IHttpActionResult CreateCategory(Category category)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Categories.Add(category);

			_context.SaveChanges();

			return Created($"{base.Request.RequestUri}/{category.Id}", category);
		}
	}
}
