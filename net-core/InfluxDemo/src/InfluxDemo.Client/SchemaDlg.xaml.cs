namespace InfluxDemo.Client
{
	using InfluxDemo.Client.Database;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for SchemaDlg.xaml
	/// </summary>
	public partial class SchemaDlg : DlgBase
	{
		#region Fields and Constants

		private DbType dbType;
		private List<string> measurements;
		private List<TagKeyItem> tagKeys;
		private List<FieldKeyItem> fieldKeys;

		#endregion


		#region Constructor

		public SchemaDlg(DbType dbType)
			: base($"Show DB Schema", dbType)
		{
			InitializeComponent();
			Loaded += this.SchemaDlg_Loaded;
			this.dbType = dbType;
		}

		public static SchemaDlg Create(DbType dbType) => new SchemaDlg(dbType);

		#endregion


		#region Event Handlers

		private async void SchemaDlg_Loaded(object sender, RoutedEventArgs e)
		{
			var databases = await InfluxRest.GetDatabases();
			if (databases == null)
			{
				return;
			}

			listDatabases.ItemsSource = databases;
			var db = databases.FirstOrDefault();
			listDatabases.SelectedItem = db;

			if (db != null)
			{
				await LoadDatabaseSchema(db);
			}

			listDatabases.SelectionChanged += ListDatabases_SelectionChanged;
			listMeasurements.SelectionChanged += this.ListMeasurements_SelectionChanged;
		}

		private async void ListDatabases_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var db = listDatabases.SelectedItem.ToString();
			if (string.IsNullOrEmpty(db))
			{
				return;
			}

			listMeasurements.SelectionChanged -= this.ListMeasurements_SelectionChanged;
			await LoadDatabaseSchema(db);
			listMeasurements.SelectionChanged += this.ListMeasurements_SelectionChanged;
		}

		private async void ListMeasurements_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var db = listDatabases.SelectedItem?.ToString();
			var measurement = listMeasurements.SelectedItem?.ToString();

			await LoadMeasurementData(db, measurement);
		}

		private async void ButtonLoadSampleData_Click(object sender, RoutedEventArgs e)
		{
			if (!GetQueryParameters(out var db, out var measurement))
			{
				return;
			}

			int limit = int.TryParse(textBoxLimit.Text, out var num) ? num : 10;
			var csvResult = await RunRawQuery($"SELECT * FROM \"{measurement}\" LIMIT {limit}", db);
			LoadSampleData(csvResult);
		}

		private async void ButtonGetFirst_Click(object sender, RoutedEventArgs e)
		{
			if (!GetQueryParameters(out var db, out var measurement))
			{
				return;
			}

			var csvResult = await RunRawQuery($"SELECT FIRST(*) FROM \"{measurement}\"", db);
			LoadSampleData(csvResult);
		}

		private async void ButtonGetLast_Click(object sender, RoutedEventArgs e)
		{
			if (!GetQueryParameters(out var db, out var measurement))
			{
				return;
			}

			var csvResult = await RunRawQuery($"SELECT LAST(*) FROM \"{measurement}\"", db);
			LoadSampleData(csvResult);
		}

		private async void ButtonGetCount_Click(object sender, RoutedEventArgs e)
		{
			if (!GetQueryParameters(out var db, out var measurement))
			{
				return;
			}

			var csvResult = await RunRawQuery($"SELECT COUNT(*) FROM \"{measurement}\"", db);
			LoadSampleData(csvResult);
		}

		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ButtonLoadCsvHelper_Click(object sender, RoutedEventArgs e)
		{

		}

		#endregion


		#region Helpers

		private async Task LoadDatabases()
		{
			//var query = "SHOW DATABASES";
			//var csv = await InfluxRest.GetRawAsync(query);
			//databases = string.IsNullOrEmpty(csv)
			//	? new List<string>()
			//	: ExtractDataList(csv, nameof(databases));

		}

		private async Task LoadDatabaseSchema(string database)
		{
			if (string.IsNullOrEmpty(database))
			{
				return;
			}

			await LoadMeasurements(database);
			var measurement = measurements.FirstOrDefault();
			listMeasurements.SelectedItem = measurement;
			await LoadMeasurementData(database, measurement);
		}

		private async Task LoadMeasurementData(string database, string measurement)
		{
			if (!string.IsNullOrEmpty(database) && !string.IsNullOrEmpty(measurement))
			{
				await LoadFieldKeys(database, measurement);
				await LoadTagKeys(database, measurement);
			}
			else
			{
				listFields.ItemsSource = null;
				listTags.ItemsSource = null;
			}
		}

		private async Task LoadTagKeys(string database, string measurement)
		{
			if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(measurement))
			{
				listTags.ItemsSource = null;
				return;
			}

			tagKeys = await InfluxRest.GetTagKeys(database, measurement);
			listTags.ItemsSource = tagKeys;
		}

		private async Task LoadFieldKeys(string database, string measurement)
		{
			if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(measurement))
			{
				listFields.ItemsSource = null;
				return;
			}

			fieldKeys = await InfluxRest.GetFieldKeys(database, measurement);
			listFields.ItemsSource = fieldKeys;
		}

		private async Task LoadMeasurements(string database)
		{
			if (string.IsNullOrEmpty(database))
			{
				return;
			}

			measurements = await InfluxRest.GetMeasurements(database);
			listMeasurements.ItemsSource = measurements;
		}

		private void LoadDataGridFromCsv(string csv)
		{
			gridMain.IsEnabled = false;
			try
			{
				var dataTable = Helper.GetDataTabletFromCsvString(csv);
				dataGridData.ItemsSource = dataTable.DefaultView;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.GetType().FullName);
			}
			finally
			{
				gridMain.IsEnabled = true;
			}
		}

		private async Task<string> RunRawQuery(string query, string db)
		{
			gridMain.IsEnabled = false;
			try
			{
				var result = await InfluxRest.GetRawAsync(query, db);
				return result;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.GetType().FullName);
			}
			finally
			{
				gridMain.IsEnabled = true;
			}
			return null;
		}

		private void LoadSampleData(string csv)
		{
			textBoxSampleData.Text = csv;
			LoadDataGridFromCsv(csv);
		}

		private bool GetQueryParameters(out string db, out string measurement)
		{
			db = listDatabases.SelectedItem?.ToString();
			measurement = listMeasurements.SelectedItem?.ToString();

			if (string.IsNullOrEmpty(db) || string.IsNullOrEmpty(measurement))
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}
