namespace DataBinding
{
	using System;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>

	public partial class Menu : Window
	{

		public Menu()
		{
			InitializeComponent();

			KeyUp += Window_KeyUp;
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			// Get the current button.
			Button cmd = (Button)e.OriginalSource;

			// Create an instance of the window named
			// by the current button.
			Type type = this.GetType();
			Assembly assembly = type.Assembly;
			Window win = (Window)assembly.CreateInstance(
				type.Namespace + "." + cmd.Content);

			// Show the window.
			win.ShowDialog();
		}

		private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Application.Current.Shutdown();
			}
		}
	}
}