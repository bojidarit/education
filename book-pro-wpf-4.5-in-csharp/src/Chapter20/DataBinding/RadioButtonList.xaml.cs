using System.Windows;

namespace DataBinding
{
	/// <summary>
	/// Interaction logic for RadioButtonList.xaml
	/// </summary>

	public partial class RadioButtonList : Window
	{

		public RadioButtonList()
		{
			InitializeComponent();

			lstProducts.ItemsSource = App.StoreDb.GetProducts();
		}

		private void cmdGetSelectedItem(object sender, RoutedEventArgs e)
		{
			if (lstProducts.SelectedItem == null)
			{
				MessageBox.Show("Please, select an item.", "Info");
				return;
			}

			MessageBox.Show(lstProducts.SelectedItem.ToString(), "Selected Item");
		}
	}
}