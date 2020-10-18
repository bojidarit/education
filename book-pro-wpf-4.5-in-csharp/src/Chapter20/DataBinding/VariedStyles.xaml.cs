namespace DataBinding
{
	using StoreDatabase;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Windows;
	using WPFControls;

	/// <summary>
	/// Interaction logic for VariedStyles.xaml
	/// </summary>
	public partial class VariedStyles : Dialog
	{
		public VariedStyles()
		{
			InitializeComponent();
		}

		private ICollection<Product> products;
		private string oldCategoryName;

		private void cmdGetProducts_Click(object sender, RoutedEventArgs e)
		{
			products = App.StoreDb.GetProducts();
			lstProducts.ItemsSource = products;
			lstProducts.SelectedItem = products.FirstOrDefault();
		}

		private void cmdApplyChange_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(oldCategoryName))
			{
				oldCategoryName = ((ObservableCollection<Product>)products)[1].CategoryName;
				((ObservableCollection<Product>)products)[1].CategoryName = "Travel";
			}
			else
			{
				((ObservableCollection<Product>)products)[1].CategoryName = oldCategoryName;
				oldCategoryName = null;
			}

			// The brute-force approach to apply style selector for all items

			//StyleSelector selector = lstProducts.ItemContainerStyleSelector;
			//lstProducts.ItemContainerStyleSelector = null;
			//lstProducts.ItemContainerStyleSelector = selector;

			// The better way to apply style selection
			lstProducts.Items.Refresh();

			// TODO: You may choose to run this code automatically in response to certain changes 
			// by handling events such as PropertyChanged...
		}
	}
}