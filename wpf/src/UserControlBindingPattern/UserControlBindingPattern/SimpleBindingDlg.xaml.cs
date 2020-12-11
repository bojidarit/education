namespace UserControlBindingPattern
{
	using System;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Input;
	using UserControlBindingPattern.Models;

	/// <summary>
	/// Interaction logic for SimpleBindingDlg.xaml
	/// </summary>
	public partial class SimpleBindingDlg : Window
	{
		public SimpleBindingDlg(string title)
		{
			InitializeComponent();

			Title = title;

			var model = new DemoModel { SomeValue = "Write something here" };
			DataContext = model;
			Debug.WriteLine($"{Environment.NewLine}*** model.SomeValue = '{model.SomeValue}'{Environment.NewLine}");

			KeyUp += this.SimpleBindingDlg_KeyUp;
		}

		private void SimpleBindingDlg_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				e.Handled = true;
				Close();
			}
		}
	}
}
