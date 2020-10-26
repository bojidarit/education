namespace JwtDemo.Interface
{
	using Newtonsoft.Json;
	using System;
	using System.Text;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			KeyUp += this.MainWindow_KeyUp;
		}

		#region Event Handlers

		private void MainWindow_KeyUp(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Escape)
			{
				Application.Current.Shutdown();
			}
		}

		private void TextBoxJwtEnc_TextChanged(object sender, TextChangedEventArgs e)
		{
			TranslateJwt(sender as TextBox);
		}

		private void TextBoxJwtEnc_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter || e.Key == Key.Return)
			{
				TranslateJwt(sender as TextBox);
			}
		}

		#endregion

		#region Helpers

		private void TranslateJwt(TextBox textBox)
		{
			var jwt = textBox?.Text;
			SetData(jwt);
		}

		private void SetData(string jwtEnc)
		{
			txtHeaderEnc.Text = string.Empty;
			txtPayloadEnc.Text = string.Empty;
			txtSignatureEnc.Text = string.Empty;

			if (string.IsNullOrEmpty(jwtEnc))
			{
				return;
			}

			var array = jwtEnc.Split('.');
			if (array.Length >= 1)
			{
				txtHeaderEnc.Text = array[0];
				txtHeaderDec.Text = DecodeBase64(array[0]);
			}
			if (array.Length >= 2)
			{
				txtPayloadEnc.Text = array[1];
				txtPayloadDec.Text = DecodeBase64(array[1]);
			}
			if (array.Length >= 3)
			{
				txtSignatureEnc.Text = array[2];
				// TODO: Decode ...
				txtSignatureDec.Text = array[2];
			}
		}

		private string DecodeBase64(string encodedString)
		{
			string decodedString = encodedString;

			try
			{
				byte[] data = Convert.FromBase64String(encodedString);
				decodedString = Encoding.UTF8.GetString(data);

				var jsonObj = JsonConvert.DeserializeObject(decodedString);
				var json = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
				return json;
			}
			catch(Exception ex)
			{
				return $"{ex.GetType().FullName}{Environment.NewLine}{ex.Message}";
			}
		}

		#endregion
	}
}
