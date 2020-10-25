using SQLiteDapperApi.Models;

namespace SQLiteDapperApi.Dtos
{
	public class ProductDto
	{
		#region Properties

		public string Name { get; set; }

		public string Description { get; set; }

		#endregion


		#region Methods

		public override string ToString() => $"{{Name='{Name}', Description='{Description}'}}";

		public static ProductDto Create(ProductModel model)
		{
			var dto = new ProductDto();
			dto.Name = model.Name;
			dto.Description = model.Description;

			return dto;
		}

		#endregion
	}
}
