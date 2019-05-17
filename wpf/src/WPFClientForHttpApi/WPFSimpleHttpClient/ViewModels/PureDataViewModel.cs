namespace WPFSimpleHttpClient.ViewModels
{
	using Catel.Data;
	using Catel.MVVM;
	using Newtonsoft.Json;
	using WPFSimpleHttpClient.HttpClientWrapper;

	public class PureDataViewModel : ViewModelBase
	{
		public PureDataViewModel(string data)
		{
			Init();
			this.Data = data;
		}

		public PureDataViewModel(HttpData<string> httpData)
		{
			Init();
			if (!string.IsNullOrWhiteSpace(httpData?.Content))
			{
				this.Data = httpData.ContentType.MediaType.Contains("json") ? FormatJson(httpData.Content) : httpData.Content;
			}
		}

		#region Properties

		public override string Title => "Pure Data";

		public string Data
		{
			get { return GetValue<string>(DataProperty); }
			set { SetValue(DataProperty, value); }
		}
		public static readonly PropertyData DataProperty = RegisterProperty("Data", typeof(string), null);

		#endregion //Properties

		#region Commands

		#region CopyCommand

		public Command CopyCommand { get; private set; }

		private void OnCopyCommandExecute()
		{
			System.Windows.Clipboard.SetText(this.Data);
		}

		#endregion //CopyCommand

		#region OkCommand

		public Command OkCommand { get; private set; }

		private async void OnOkCommandExecute()
		{
			await this.SaveAndCloseViewModelAsync();
		}

		#endregion //OkCommand

		#endregion //Commands

		#region Methods

		private static string FormatJson(string json)
		{
			dynamic parsedJson = JsonConvert.DeserializeObject(json);
			return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
		}

		private void Init()
		{
			OkCommand = new Command(OnOkCommandExecute);
			CopyCommand = new Command(OnCopyCommandExecute);
		}

		#endregion //Methods
	}
}
