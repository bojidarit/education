namespace UserControlBindingPattern
{
	using System;
	using System.Threading;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using UserControlBindingPattern.Models;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private SizeModel model;

		public MainWindow()
		{
			InitializeComponent();
			KeyUp += this.MainWindow_KeyUp;

			//model = new SizeModel { Height = 100.0, Width = 200.0 };
			model = new SizeModel();
			DataContext = model;
		}

		#region Event Handlers

		private void CloseApp()
		{
			Application.Current.Shutdown(0);
		}

		private void MainWindow_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				CloseApp();
			}
			else if (e.Key == Key.F1)
			{
				MessageBox.Show(
					$"Current culture is '{Thread.CurrentThread.CurrentUICulture.NativeName}'" +
					$"{Environment.NewLine}Today is {DateTime.Today.ToShortDateString()}" +
					$"{Environment.NewLine}{model}",
					model.GetType().Name,
					MessageBoxButton.OK,
					MessageBoxImage.Information);
			}
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			CloseApp();
		}

		private void SimpleBinding_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var dlg = new SimpleBindingDlg(button?.ToolTip.ToString());
			dlg.ShowDialog();
		}

		#endregion
	}
}
