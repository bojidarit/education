namespace Influx2Demo.Client
{
	using Influx2Demo.Logic;
	using Influx2Demo.Logic.DataStructures;
	using Influx2Demo.Logic.DataStructures.Enumerations;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;

	/// <summary>
	/// Interaction logic for SchemaDlg.xaml
	/// </summary>
	public partial class SchemaDlg : DlgBase
	{
		#region Fields and Constants

		private const int DefaultLimit = 100;

		private InfluxDbType dbType;
		private List<BucketDto> buckets;
		private List<string> measurements;
		private List<string> tagKeys;
		private List<string> fieldKeys;

		#endregion


		#region Construction

		public SchemaDlg(InfluxDbType dbType)
			: base("Show DB Schema", dbType)
		{
			InitializeComponent();
			Loaded += this.SchemaDlg_Loaded;
			this.dbType = dbType;
			textBoxLimit.Text = DefaultLimit.ToString();
		}


		public static SchemaDlg Create(InfluxDbType dbType) => new SchemaDlg(dbType);

		#endregion


		#region Overrides

		protected override void OnBusyStateChanged()
		{
			busyMain.IsBusy = IsInBusyState;
		}

		private void ClearMeasureSelection()
		{
			listMeasurements.ItemsSource = null;
			listTags.ItemsSource = null;
			listFields.ItemsSource = null;
			textBoxLineProtocol.Text = string.Empty;
		}

		private async Task<string> LoadBuckets(InfluxApi influxApi = null)
		{
			var api = (influxApi != null) ? influxApi : Helper.CreateInfluxApi(dbType);
			buckets = await api.GetBuckets(true);
			if (buckets == null)
			{
				return null;
			}

			listBuckets.ItemsSource = buckets;
			var bucket = buckets.FirstOrDefault();
			listBuckets.SelectedItem = bucket;

			return bucket?.Name;
		}

		private async Task<bool> LoadMeasurements(string bucketName, InfluxApi influxApi = null)
		{
			ClearMeasureSelection();
			if (string.IsNullOrEmpty(bucketName))
			{
				return false;
			}

			var api = (influxApi != null) ? influxApi : Helper.CreateInfluxApi(dbType);
			measurements = await api.GetMeasurements(bucketName);
			if (measurements == null)
			{
				return false;
			}

			listMeasurements.ItemsSource = measurements;
			var measure = measurements.FirstOrDefault();
			listMeasurements.SelectedItem = measure;

			var result = await LoadMeasurementDetails(bucketName, measure);
			return result;
		}

		private async Task<bool> LoadMeasurementDetails(string bucketName, string measure, InfluxApi influxApi = null)
		{
			var api = (influxApi != null) ? influxApi : Helper.CreateInfluxApi(dbType);

			tagKeys = await api.GetTagKeys(bucketName, measure);
			listTags.ItemsSource = tagKeys;

			fieldKeys = await api.GetFieldKeys(bucketName, measure);
			listFields.ItemsSource = fieldKeys;

			var tags = (tagKeys != null && tagKeys.Any()) ? $" {string.Join(",", tagKeys)}" : string.Empty;
			var fields = (fieldKeys != null && fieldKeys.Any()) ? $" {string.Join(",", fieldKeys)}" : string.Empty;
			textBoxLineProtocol.Text = $"{measure}{tags}{fields}";

			return true;
		}

		#endregion


		#region Helpers

		private bool GetMainParams(out BucketDto bucket, out string measure, out int limit)
		{
			measure = null;
			limit = DefaultLimit;
			bucket = listBuckets.SelectedItem as BucketDto;
			if (bucket == null)
			{
				return false;
			}

			measure = listMeasurements.SelectedItem?.ToString();
			if (string.IsNullOrEmpty(measure))
			{
				return false;
			}

			if (!int.TryParse(textBoxLimit.Text, out limit))
			{
				limit = DefaultLimit;
			}

			return true;
		}

		private void ShowCsvResult(string csv, string measure, string func)
		{
			textBoxSampleData.Text = csv;
			if (string.IsNullOrEmpty(csv))
			{
				tabItemTable.Header = "Sample data";
				return;
			}

			var trimmedCsv = DataParser.TrimTopCsv(csv, 3);
			var dataTable = DataParser.MakeDataTableFromCsv(trimmedCsv);
			Helper.UpgradeInfluxCsvTable(dataTable);

			tabItemTable.Header = $"[{measure}]:[{func}] {dataTable.DefaultView.Count} row(s)";

			dataGridData.ItemsSource = (dataTable == null) ? null : dataTable.DefaultView;
		}

		private async Task LoadSampleData(string measure, string flux, string func)
		{
			IsInBusyState = true;
			try
			{
				var api = Helper.CreateInfluxApi(dbType);
				textBoxQuery.Text = flux;

				var csv = await api.FluxQueryRawAsync(flux);
				ShowCsvResult(csv, measure, func);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.GetType().Name);
				return;
			}
			finally
			{
				IsInBusyState = false;
			}
		}

		private async Task GetSingleRecord(string funcName, bool isFuncSelector)
		{
			if (!GetMainParams(out var bucket, out var measure, out var limit))
			{
				MessageBox.Show("Bad bucket and/or measure.", "Input parameters");
				return;
			}

			var fieldKey = listFields.SelectedItem?.ToString();

			var flux = InfluxApi.GetSingleRecordFlux(bucket?.Name, measure, funcName, isFuncSelector, fieldKey);
			await LoadSampleData(measure, flux, funcName);
		}

		#endregion


		#region Event Handlers

		private async void SchemaDlg_Loaded(object sender, RoutedEventArgs e)
		{
			IsInBusyState = true;
			try
			{
				var api = Helper.CreateInfluxApi(dbType);
				var bucket = await LoadBuckets(api);
				listBuckets.SelectionChanged += this.ListBuckets_SelectionChanged;

				if (!string.IsNullOrEmpty(bucket))
				{
					var result = await LoadMeasurements(bucket);
					listMeasurements.SelectionChanged += this.ListMeasurements_SelectionChanged;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.GetType().Name);
				return;
			}
			finally
			{
				IsInBusyState = false;
			}
		}

		private async void ListBuckets_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			ClearMeasureSelection();

			var bucket = listBuckets.SelectedItem as BucketDto;
			if (bucket == null)
			{
				return;
			}

			IsInBusyState = true;
			try
			{
				listMeasurements.SelectionChanged -= this.ListMeasurements_SelectionChanged;
				await LoadMeasurements(bucket?.Name);
				listMeasurements.SelectionChanged += this.ListMeasurements_SelectionChanged;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.GetType().Name);
				return;
			}
			finally
			{
				IsInBusyState = false;
			}
		}

		private async void ListMeasurements_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (!GetMainParams(out var bucket, out var measure, out var limit))
			{
				ClearMeasureSelection();
			}

			IsInBusyState = true;
			try
			{
				await LoadMeasurementDetails(bucket?.Name, measure);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.GetType().Name);
				return;
			}
			finally
			{
				IsInBusyState = false;
			}
		}

		private async void ButtonLoadSampleData_Click(object sender, RoutedEventArgs e)
		{
			if (!GetMainParams(out var bucket, out var measure, out var limit))
			{
				MessageBox.Show("Bad bucket and/or measure.", "Input parameters");
				return;
			}

			var fieldKey = listFields.SelectedItem?.ToString();
			var flux = InfluxApi.GetSampleDataFlux(bucket?.Name, measure, limit, fieldKey);
			await LoadSampleData(measure, flux, "all");
		}

		private async void ButtonGetFirst_Click(object sender, RoutedEventArgs e)
		{
			await GetSingleRecord("first", true);
		}

		private async void ButtonGetLast_Click(object sender, RoutedEventArgs e)
		{
			await GetSingleRecord("last", true);
		}

		private async void ButtonGetCount_Click(object sender, RoutedEventArgs e)
		{
			await GetSingleRecord("count", false);
		}

		#endregion
	}
}
