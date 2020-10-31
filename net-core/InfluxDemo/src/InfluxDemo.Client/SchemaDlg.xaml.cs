namespace InfluxDemo.Client
{
	using InfluxDemo.Client.Database;

	/// <summary>
	/// Interaction logic for SchemaDlg.xaml
	/// </summary>
	public partial class SchemaDlg : DlgBase
	{
		#region Constructor

		public SchemaDlg(DbType dbType)
			:base("Show DB Schema", dbType)
		{
			InitializeComponent();
			Loaded += this.SchemaDlg_Loaded;
		}

		public static SchemaDlg Create(DbType dbType) => new SchemaDlg(dbType);

		#endregion


		#region Event Handlers

		private async void SchemaDlg_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			
		}

		#endregion


		#region Helpers



		#endregion
	}
}
