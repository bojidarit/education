namespace DataBinding
{
	using StoreDatabase;
	using System.Collections.Generic;
	using System.Linq;
	using WPFControls;

	/// <summary>
	/// Interaction logic for DataTemplateList.xaml
	/// </summary>
	public partial class DataTemplateList : Dialog
	{
		public DataTemplateList()
		{
			InitializeComponent();

			LoadProducts();
			KeyUp += DataTemplateList_KeyUp;
		}

		private void DataTemplateList_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.F5)
			{
				LoadProducts();
			}
		}

		private ICollection<Product> products;

		private void LoadProducts()
		{
			products = App.StoreDb.GetProducts();
			lstProducts.ItemsSource = products;
			lstProducts.SelectedItem = products.FirstOrDefault();
		}
	}
}