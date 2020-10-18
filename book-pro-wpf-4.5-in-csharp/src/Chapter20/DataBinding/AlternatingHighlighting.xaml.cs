namespace DataBinding
{
	using StoreDatabase;
	using System.Collections.Generic;
	using System.Windows;
	using WPFControls;

	/// <summary>
	/// Interaction logic for AlternatingHighlighting.xaml
	/// </summary>
	public partial class AlternatingHighlighting : Dialog
	{
		public AlternatingHighlighting()
		{
			InitializeComponent();
		}

		private ICollection<Product> products;

		private void cmdGetProducts_Click(object sender, RoutedEventArgs e)
		{
			products = App.StoreDb.GetProducts();
			lstProducts.ItemsSource = products;
		}
	}
}
