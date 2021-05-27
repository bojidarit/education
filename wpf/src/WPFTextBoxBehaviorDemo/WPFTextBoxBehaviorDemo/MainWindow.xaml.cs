namespace WPFTextBoxBehaviorDemo
{
	using System;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;

	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var modes = Enum.GetValues(typeof(TextBoxInputMode)).OfType<TextBoxInputMode>();
			comboType.ItemsSource = modes;
			comboType.SelectedItem = modes
				.Where(i => i == TextBoxInputMode.DigitInput)
				.FirstOrDefault();

			textInput.Focus();
		}

		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			textInput.Text = string.Empty;
		}
	}
}
