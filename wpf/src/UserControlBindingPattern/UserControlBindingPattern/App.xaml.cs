namespace UserControlBindingPattern
{
	using System.Globalization;
	using System.Threading;
	using System.Windows;
	using System.Windows.Markup;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			SetCulture(new CultureInfo("en-GB"));

			FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
				new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
		}

		private void SetCulture(CultureInfo culture = null)
		{
			Thread.CurrentThread.CurrentCulture = culture ?? CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = culture ?? CultureInfo.InvariantCulture;
		}
	}
}
