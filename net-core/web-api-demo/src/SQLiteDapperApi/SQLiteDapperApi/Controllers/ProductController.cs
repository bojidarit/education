namespace SQLiteDapperApi.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using SQLiteDapperApi.Dtos;
	using SQLiteDapperApi.Models;
	using SQLiteDapperApi.Repositories;
	using System;
	using System.Linq;
	using System.Threading.Tasks;

	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductRepository productRepository;

		public ProductController(IProductRepository productRepository) =>
			this.productRepository = productRepository;

		// GET: api/product
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var products = await productRepository.Get();
			return Ok(products.Select(p => ProductDto.Create(p)));
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

			//var product = await productRepository.GetByName(dto.Name);
			//if (product != null)
			//{
			//	return BadRequest($"Item with name = '{dto.Name}' already exists.");
			//}

			var error = $"Issue with product creation.";
			var affectedRows = await productRepository.Create(ProductModel.Create(dto));
			if (affectedRows <= 0)
			{
				return BadRequest(error);
			}

			try
			{
				var product = await productRepository.GetByName(dto.Name);
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

		// DELETE: api/[controller]/{rowid}
	}
}
