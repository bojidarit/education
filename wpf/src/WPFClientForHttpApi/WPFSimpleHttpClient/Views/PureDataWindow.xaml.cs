namespace WPFSimpleHttpClient.Views
{
	public partial class PureDataWindow
	{
		public PureDataWindow()
			: base(Catel.Windows.DataWindowMode.Custom) // Custom mode for dialogs discards the default OK/Cancel buttons
		{
			InitializeComponent();
		}
	}
}
