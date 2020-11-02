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

		private const string SchemaMeasurements = "MEASUREMENTS";
		private const string SchemaTagKeys = "TAG KEYS";
		private const string SchemaFieldKeys = "FIELD KEYS";

		private DbType dbType;
		private List<string> databases;
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
			await LoadDatabases();
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
			var db = listDatabases.SelectedItem?.ToString();
			var measurement = listMeasurements.SelectedItem?.ToString();

			if (string.IsNullOrEmpty(db) || string.IsNullOrEmpty(measurement))
			{
				return;
			}

			int limit = int.TryParse(textBoxLimit.Text, out var num) ? num : 10;
			var result = await RunRawQuery($"SELECT * FROM {measurement} LIMIT {limit}", db);
			textBoxSampleData.Text = result;

			LoadDataGridFromCsv(result);

			//using (TextReader textReader = new StringReader(result))
			//{
			//	using (var csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture))
			//	{
			//		var records = csvReader.GetRecords()
			//	}
			//}
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
				var result = await InfluxRest.QueryRawAsync(query, db);
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

		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		#endregion


		#region Helpers

		private async Task LoadDatabases()
		{
			var query = "SHOW DATABASES";
			var csv = await ExecuteQueryAsync(query);
			databases = string.IsNullOrEmpty(csv)
				? new List<string>()
				: ExtractDataList(csv, nameof(databases));

			listDatabases.ItemsSource = databases;
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

			var query = $"SHOW {SchemaTagKeys} FROM {measurement}";
			var csv = await ExecuteQueryAsync(query, database);

			if (string.IsNullOrEmpty(csv))
			{
				tagKeys = new List<TagKeyItem>();
			}
			else
			{
				var csvLines = Helper.SplitByLine(csv).Skip(1);
				tagKeys = csvLines.Select(l => TagKeyItem.Create(l)).ToList();
			}

			listTags.ItemsSource = tagKeys;
		}

		private async Task LoadFieldKeys(string database, string measurement)
		{
			if (string.IsNullOrEmpty(database) || string.IsNullOrEmpty(measurement))
			{
				listFields.ItemsSource = null;
				return;
			}

			var query = $"SHOW {SchemaFieldKeys} FROM {measurement}";
			var csv = await ExecuteQueryAsync(query, database);

			if (string.IsNullOrEmpty(csv))
			{
				fieldKeys = new List<FieldKeyItem>();
			}
			else
			{
				var csvLines = Helper.SplitByLine(csv).Skip(1);
				fieldKeys = csvLines.Select(l => FieldKeyItem.Create(l)).ToList();
			}

			listFields.ItemsSource = fieldKeys;
		}

		private async Task LoadMeasurements(string database)
		{
			if (string.IsNullOrEmpty(database))
			{
				return;
			}

			var query = $"SHOW {SchemaMeasurements}";
			var csv = await ExecuteQueryAsync(query, database);
			measurements = string.IsNullOrEmpty(csv)
				? new List<string>()
				: ExtractDataList(csv, nameof(measurements));

			listMeasurements.ItemsSource = measurements;
		}

		private List<string> ExtractDataList(string csv, string title)
		{
			Func<string, string> func = (line) =>
			{
				var data = string.Empty;
				var idx = line.LastIndexOf(",");
				if (idx >= 0)
				{
					data = line.Substring(idx + 1);
				}

				return data;
			};

			var lines = Helper.SplitByLine(csv).Where(l => l.StartsWith(title));
			var dbs = lines
				.Select(l => func(l))
				.Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith("_"));

			return dbs.ToList();
		}

		private async Task<string> ExecuteQueryAsync(string query, string db = null)
		{
			try
			{
				var content = await InfluxRest.QueryRawAsync(query, db);
				return content;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, query);
			}

			return null;
		}

		#endregion
	}
}
