namespace InfluxDemo.Client
{
	using InfluxDemo.Client.Database;
	using System;
	using System.Windows;
	using System.Windows.Input;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			KeyUp += this.MainWindow_KeyUp;
		}

		private void MainWindow_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Application.Current.Shutdown();
			}
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			panelMain.IsEnabled = false;
			try
			{
				var result = await InfluxClient.GetHealth();
				MessageBox.Show(result.ToJson(), result.GetType().FullName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.GetType().Name);
			}
			finally
			{
				panelMain.IsEnabled = true;
			}
		}
	}
}
