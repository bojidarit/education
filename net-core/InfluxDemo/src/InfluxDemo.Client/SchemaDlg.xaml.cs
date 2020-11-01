namespace InfluxDemo.Client
{
	using InfluxDemo.Client.Database;
	using System;
	using System.Threading.Tasks;
	using System.Windows;

	/// <summary>
	/// Interaction logic for SchemaDlg.xaml
	/// </summary>
	public partial class SchemaDlg : DlgBase
	{
		private DbType dbType;

		#region Constructor

		public SchemaDlg(DbType dbType)
			: base($"Show DB Schema from [{dbType}]", dbType)
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
			var query = "SHOW DATABASES";
			var result = await ExecuteQueryAsync(query);

			if (result != null)
			{
				MessageBox.Show(result, query);
			}
		}

		#endregion


		#region Helpers

		private async Task<string> ExecuteQueryAsync(string query)
		{
			try
			{
				var content = await InfluxRest.QueryRawAsync(query);
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
