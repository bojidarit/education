namespace WPFControls
{
	using System.Windows;
	using System.Windows.Input;

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
	}
}
