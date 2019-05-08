namespace WPFSimpleHttpClient.Models
{
	using Catel.Data;

	public class IdNameModel : EntityBase
	{
		public IdNameModel() { }

		public IdNameModel(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public int Id
		{
			get { return GetValue<int>(IdProperty); }
			set { SetValue(IdProperty, value); }
		}
		public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int), null);

		public string Name
		{
			get { return GetValue<string>(NameProperty); }
			set { SetValue(NameProperty, value); }
		}
		public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string), null);

		public override string ToString() =>
			$"Id = {this.Id}; Name = '{this.Name}'";
	}
}
