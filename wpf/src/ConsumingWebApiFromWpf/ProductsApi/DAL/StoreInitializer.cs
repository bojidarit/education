using ProductsApi.Models;

namespace ProductsApi.DAL
{
	public class StoreInitializer : System.Data.Entity.CreateDatabaseIfNotExists<StoreContext>
	{
		protected override void Seed(StoreContext context)
		{
			Category[] categories = new Category[]
			{
				new Category{ Name = "Vegetables" },
				new Category{ Name = "Fruits" }
			};

			context.Categories.AddRange(categories);
			context.SaveChanges();
		}
	}
}