namespace InfluxDemo.Client
{
	using InfluxDemo.Client.Database;
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private bool useCloud = false;
		private string dbType;

		public MainWindow()
		{
			InitializeComponent();
			KeyUp += this.MainWindow_KeyUp;
		}

		private string AddTypeInfo(string data, object obj) =>
			$"{obj.GetType().FullName}" +
			$"{Environment.NewLine}{data}";

		#region Event Handlers

		private void MainWindow_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Application.Current.Shutdown();
			}
		}

		private async void HeathButton_Click(object sender, RoutedEventArgs e)
		{
			panelMain.IsEnabled = false;
			try
			{
				var result = await Influx.GetHealth(useCloud);
				MessageBox.Show(AddTypeInfo(result.ToJson(), result), dbType);
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

		private async void ReadyButton_Click(object sender, RoutedEventArgs e)
		{
			panelMain.IsEnabled = false;
			try
			{
				var result = await Influx.GetReady(useCloud);
				if (result == null)
				{
					MessageBox.Show("No data.", dbType);
				}
				else
				{
					MessageBox.Show(AddTypeInfo(result.ToJson(), result), dbType);
				}
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

		private void ComboDbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var cbItem = comboDbType.SelectedItem as ComboBoxItem;
			dbType = cbItem?.Content?.ToString();
			useCloud = (comboDbType.SelectedIndex == 1);
		}

		#endregion
	}
}
