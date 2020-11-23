namespace Influx2Demo.Client
{
	using Influx2Demo.Logic.DataStructures.Enumerations;
	using System.Windows;
	using System.Windows.Input;

	public class DlgBase : Window
	{
		#region Constructor

		public DlgBase(string title, InfluxDbType dbType)
			: base()
		{
			WindowStartupLocation = WindowStartupLocation.CenterScreen;
			KeyUp += this.DlgBase_KeyUp;

			var type = $"[{this.GetType().Name}]";
			Title = string.IsNullOrEmpty(title) ? type : $"{title} (DB Type: {dbType}) {type}";
		}

		#endregion


		#region Properties

		public bool IsInBusyState
		{
			get { return (bool)GetValue(IsInBusyStateProperty); }
			set { SetValue(IsInBusyStateProperty, value); }
		}
		public static readonly DependencyProperty IsInBusyStateProperty =
			DependencyProperty.Register(
				"IsInBusyState", 
				typeof(bool), 
				typeof(DlgBase), 
				new FrameworkPropertyMetadata(
					false, 
					new PropertyChangedCallback(OnIsInBusyStateChanged)));

		private static void OnIsInBusyStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var dlg = obj as SchemaDlg;
			if (dlg != null)
			{
				dlg.OnBusyStateChanged();
			}
		}

		#endregion


		#region Public Methods

		protected virtual void OnBusyStateChanged()
		{
		}

		#endregion


		#region Helpers

		private void CloseDialog()
		{
			if (!IsInBusyState)
			{
				Close();
			}
		}

		#endregion


		#region Event Handlers

		private void DlgBase_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				CloseDialog();
			}
			else if (e.Key == Key.F1)
			{
				Clipboard.SetText(this.GetType().Name);
			}
		}

		protected void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
			CloseDialog();
		}

		#endregion
	}
}
