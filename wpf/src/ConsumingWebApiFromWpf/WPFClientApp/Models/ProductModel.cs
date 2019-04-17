namespace WPFClientApp.Models
{
	using Catel.Data;
	using System.Runtime.Serialization;

	public class ProductModel : EntityBase
	{
		public ProductModel() { }

		public ProductModel(SerializationInfo info, StreamingContext context)
			: base(info, context) { }

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

		public decimal Price
		{
			get { return GetValue<decimal>(PriceProperty); }
			set { SetValue(PriceProperty, value); }
		}
		public static readonly PropertyData PriceProperty = RegisterProperty("Price", typeof(decimal), null);

		public int CategoryId
		{
			get { return GetValue<int>(CategoryIdProperty); }
			set { SetValue(CategoryIdProperty, value); }
		}
		public static readonly PropertyData CategoryIdProperty = RegisterProperty("CategoryId", typeof(int), null);
	}
}
