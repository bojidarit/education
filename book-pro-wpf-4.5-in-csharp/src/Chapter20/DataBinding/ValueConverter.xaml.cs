using StoreDatabase;
using StoreDatabase.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataBinding
{
	/// <summary>
	/// Interaction logic for EditProductObject.xaml
	/// </summary>

	public partial class ValueConverter : System.Windows.Window
    {
        private Product product;

        public ValueConverter()
        {
            InitializeComponent();
        }

        private void cmdGetProduct_Click(object sender, RoutedEventArgs e)
        {
            int ID;
            if (Int32.TryParse(txtID.Text, out ID))
            {
                try
                {
                    product = App.StoreDb.GetProduct(ID);
                    gridProductDetails.DataContext = product;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Format(), "Error contacting database.");
                }                
            }
            else
            {
                MessageBox.Show("Invalid ID.");
            }
        }

        private void cmdUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            // Make sure update has taken place.
            FocusManager.SetFocusedElement(this, (Button)sender);

            MessageBox.Show(product.UnitCost.ToString());
        }
    }
}