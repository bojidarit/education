namespace WPFClientApp.Dtos
{
	public class Category
	{
		public Category() { }

		public Category(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public Category(Models.IdNameModel model)
			: this(model.Id, model.Name)
		{ }

		public int Id { get; set; }
		public string Name { get; set; }
	}
}
