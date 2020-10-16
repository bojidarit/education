using StoreDatabase;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace DataBinding
{
	/// <summary>
	/// Interaction logic for NavigateCollection.xaml
	/// </summary>

	public partial class NavigateCollection : System.Windows.Window
    {
        private ICollection<Product> products;
        private ListCollectionView view;

        public NavigateCollection()
        {
            InitializeComponent();
            
            products = App.StoreDb.GetProducts();

            this.DataContext = products;
            view = (ListCollectionView)CollectionViewSource.GetDefaultView(this.DataContext);
            view.CurrentChanged += new EventHandler(view_CurrentChanged);

            lstProducts.ItemsSource = products;            
        }

        private void cmdNext_Click(object sender, RoutedEventArgs e)
        {    
            view.MoveCurrentToNext();          
        }
        private void cmdPrev_Click(object sender, RoutedEventArgs e)
        {
            view.MoveCurrentToPrevious();
        }

        private void lstProducts_SelectionChanged(object sender, RoutedEventArgs e)
        {
           // view.MoveCurrentTo(lstProducts.SelectedItem);
        }

        private void view_CurrentChanged(object sender, EventArgs e)
        {
            lblPosition.Text = "Record " + (view.CurrentPosition + 1).ToString() +
                " of " + view.Count.ToString();
            cmdPrev.IsEnabled = view.CurrentPosition > 0;
            cmdNext.IsEnabled = view.CurrentPosition < view.Count - 1; 
        }

		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
            Close();
		}
	}
}