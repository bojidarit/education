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
		private InfluxDbType dbType;

		public MainWindow()
		{
			InitializeComponent();
			KeyUp += this.MainWindow_KeyUp;

			comboDbType.SelectedIndex = 0;
			comboDbType.SelectionChanged += ComboDbType_SelectionChanged;
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
				var result = await Influx.GetHealthAsync(useCloud);
				MessageBox.Show(AddTypeInfo(result.ToJson(), result), dbType.ToString());
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
			dbType = (InfluxDbType)comboDbType.SelectedIndex;
			useCloud = (comboDbType.SelectedIndex == (int)InfluxDbType.Cloud);

			var visibility = useCloud ? Visibility.Collapsed : Visibility.Visible;
			buttonLegacyPing.Visibility = visibility;
			buttonShowSchema.Visibility = visibility;
		}

		private void ShowSchemaButton_Click(object sender, RoutedEventArgs e)
		{
			SchemaDlg.Create(dbType).ShowDialog();
		}

		private async void PingButton_Click(object sender, RoutedEventArgs e)
		{
			if (useCloud)
			{
				MessageBox.Show("Use only for OSS.", "Not Supported");
				return;
			}

			panelMain.IsEnabled = false;
			try
			{
				var result = await FluxLegacy.PingAsync();
				MessageBox.Show(AddTypeInfo(result, result), dbType.ToString());
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

		#endregion
	}
}
