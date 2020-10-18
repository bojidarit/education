namespace WPFControls
{
	using System;
	using System.Reflection;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using WPFControls.Extensions;

	public class WindowEx : Window
	{
		public WindowEx()
		{
			KeyUp += Window_KeyUp;
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
		}

		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Application.Current.Shutdown();
			}
		}

		protected void ButtonClick(object sender, RoutedEventArgs e)
		{
			// Get the current button.
			Button cmd = (Button)e.OriginalSource;

			// Create an instance of the window named
			// by the current button.
			Type type = this.GetType();
			Assembly assembly = type.Assembly;

			try
			{
				Window win = (Window)assembly.CreateInstance(
					type.Namespace + "." + cmd.Content);

				// Show the window.
				win.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Format(), "Missing window");
			}
		}
	}
}
