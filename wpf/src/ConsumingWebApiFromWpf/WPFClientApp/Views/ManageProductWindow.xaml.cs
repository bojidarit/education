namespace WPFClientApp.Views
{
	public partial class ManageProductWindow
	{
		public ManageProductWindow()
			: base(Catel.Windows.DataWindowMode.Custom)	// Custom mode for dialogs discards the default OK/Cancel buttons
		{
			InitializeComponent();
		}
	}
}
