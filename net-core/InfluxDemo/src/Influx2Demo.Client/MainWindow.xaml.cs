namespace Influx2Demo.Client
{
	using Influx2Demo.Logic.DataStructures.Enumerations;
	using InfluxDB.Client.Api.Domain;
	using System;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Fields

		private InfluxDbType dbType = InfluxDbType.OSS;

		#endregion

		public MainWindow()
		{
			InitializeComponent();
			KeyUp += this.MainWindow_KeyUp;

			comboDbType.ItemsSource = Enum.GetValues(typeof(InfluxDbType));
			comboDbType.SelectedItem = dbType;
			comboDbType.SelectionChanged += this.ComboDbType_SelectionChanged;
		}

		#region Event Handlers

		private void ComboDbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			dbType = (InfluxDbType)comboDbType.SelectedItem;
		}

		private void MainWindow_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Application.Current.Shutdown();
			}
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private async void HeathButton_Click(object sender, RoutedEventArgs e)
		{
			HealthCheck health = null;
			Ready ready = null;

			busyMain.IsBusy = true;
			try
			{
				var api = Helper.CreateInfluxApi(dbType);
				health = await api.GetHealthAsync();
				ready = await api.GetReadyAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.GetType().Name);
				return;
			}
			finally
			{
				busyMain.IsBusy = false;
			}

			var sb = new StringBuilder();
			if (health != null)
			{
				sb.Append($"[{health.GetType().Name}]{Environment.NewLine}");
				sb.Append($"{health.ToJson()}{Environment.NewLine}{Environment.NewLine}");
			}

			if (ready != null)
			{
				sb.Append($"[{ready.GetType().Name}]{Environment.NewLine}");
				sb.Append($"{ready.ToJson()}{Environment.NewLine}");
			}

			MessageBox.Show(sb.ToString(), dbType.ToString());
		}

		private void ShowSchemaButton_Click(object sender, RoutedEventArgs e)
		{
			var dlg = SchemaDlg.Create(dbType);
			dlg.ShowDialog();
		}

		#endregion
	}
}
