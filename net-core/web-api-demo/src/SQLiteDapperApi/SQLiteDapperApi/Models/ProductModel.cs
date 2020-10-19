using SQLiteDapperApi.Dtos;

namespace SQLiteDapperApi.Models
{
	public class ProductModel
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public override string ToString()
		{
			var desc = string.IsNullOrEmpty(Description) 
				? string.Empty
				: $", Description='{Description}'";

			return $"{{Id={Id}, Name='{Name}'{desc}}}";
		}

		public static ProductModel Create(ProductDto dto)
		{
			var model = new ProductModel();
			model.Id = -1;
			model.Name = dto.Name;
			model.Description = dto.Description;

			return model;
		}
	}
}
