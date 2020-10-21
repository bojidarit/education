namespace SQLiteDapperApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using SQLiteDapperApi.Dtos;
	using SQLiteDapperApi.Models;
	using SQLiteDapperApi.Repositories;
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Threading.Tasks;

	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		#region Field & Constructor

		private readonly IProductRepository productRepository;

		public ProductController(IProductRepository productRepository) =>
			this.productRepository = productRepository;

		#endregion


		#region REST Methods

		// GET: api/product
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var products = await productRepository.Get();
			return Ok(products.Select(p => ProductDto.Create(p)));
		}

		// GET: api/product/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var product = await productRepository.GetById(id);
			if (product == null)
			{
				return NotFound($"There is no product with id = {id}.");
			}

			return Ok(product);
		}

		// POST: api/product
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] ProductDto dto)
		{
			if (dto == null)
			{
				return NoContent();
			}

			if (string.IsNullOrWhiteSpace(dto.Name))
			{
				return BadRequest($"Property name is mandatory.");
			}

			var error = $"Issue with product creation.";
			try
			{
				var product = await productRepository.GetByName(dto.Name);
				if (product != null)
				{
					return BadRequest($"Product with name = '{dto.Name}' already exists.");
				}

				var affectedRows = await productRepository.Create(ProductModel.Create(dto));
				Debug.WriteLine($"Create product {dto}. Rows Affected = {affectedRows}.");
				if (affectedRows <= 0)
				{
					return BadRequest(error);
				}

				product = await productRepository.GetByName(dto.Name);
				if (product == null)
				{
					return BadRequest(error);
				}

				var uri = Helper.CombineRequestPath(this.Request, product.Id.ToString());
				return Created(uri, dto);
			}
			catch (Exception ex)
			{
				var message = $"{error}{Environment.NewLine}" +
					$"{ex.Message} ({ex.GetType().Name})";

				return BadRequest(message);
			}
		}

		// PUT: api/[controller]/{rowid}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateById([FromBody] ProductDto product, int id)
		{
			if (product == null)
			{
				return BadRequest("One must supplye product JSON in the request body.");
			}

			var result = await SearchById(id);
			if (result != null)
			{
				return result;
			}

			var rowsAffected = await productRepository.Update(id, product.Name, product.Description);
			Debug.WriteLine($"Update product {product} by id = {id}. Rows Affected = {rowsAffected}.");

			var uri = Helper.GetHttpRequestPath(this.Request);
			return Accepted(uri);
		}

		// DELETE: api/[controller]/{rowid}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteById(int id)
		{
			var result = await SearchById(id);
			if (result != null)
			{
				return result;
			}

			var rowsAffected = await productRepository.Delete(id);
			Debug.WriteLine($"Delete product by id = {id}. Rows Affected = {rowsAffected}.");

			return NoContent();
		}

		#endregion

		#region Helpers

		private async Task<IActionResult> SearchById(int id)
		{
			var product = await productRepository.GetById(id);

			if (product == null)
			{
				return NotFound($"There is no product with id = {id}.");
			}

			return null;
		}

		#endregion
	}
}
