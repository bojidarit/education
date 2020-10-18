namespace DataBinding
{
	using StoreDatabase;
	using System;
	using System.Windows;
	using WPFControls;

	/// <summary>
	/// Interaction logic for CheckBoxList.xaml
	/// </summary>
	public partial class CheckBoxList : Dialog
	{
		public CheckBoxList()
		{
			InitializeComponent();

			lstProducts.ItemsSource = App.StoreDb.GetProducts();
		}

		private void cmdGetSelectedItems(object sender, RoutedEventArgs e)
		{
			if (lstProducts.SelectedItem == null)
			{
				MessageBox.Show("Please, make your selection.", "Info");
				return;
			}

			if (lstProducts.SelectedItems.Count > 0)
			{
				string items = "Selection:";
				foreach (Product product in lstProducts.SelectedItems)
				{
					items += $"{Environment.NewLine}   * {product}";
				}
				MessageBox.Show(items, "Info");
			}
		}
	}
}