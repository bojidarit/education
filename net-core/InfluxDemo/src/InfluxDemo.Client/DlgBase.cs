namespace InfluxDemo.Client
{
	using InfluxDemo.Client.Database;
	using System.Windows;
	using System.Windows.Input;

	public class DlgBase : Window
	{
		public DlgBase(string title, InfluxDbType dbType)
			: base()
		{
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			KeyUp += this.DlgBase_KeyUp;

			var type = $"[{this.GetType().Name}]";
			Title = string.IsNullOrEmpty(title) ? type : $"{title} (DB Type: {dbType}) {type}";
		}

		private void DlgBase_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Close();
			}
			else if (e.Key == Key.F1)
			{
				Clipboard.SetText(this.GetType().Name);
			}
		}
	}
}
