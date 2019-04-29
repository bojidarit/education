namespace WPFSimpleHttpClient.ViewModels
{
	using Catel.MVVM;
	using Extensions;
	using System;
	using HttpClientWrapper;
	using System.Configuration;
	using Catel.Data;
	using System.Collections.Generic;
	using Newtonsoft.Json.Linq;
	using Newtonsoft.Json;
	using System.Data;
	using System.Linq;

	public class MainWindowViewModel : ViewModelBase
	{
		#region Fields

		private Uri _baseUri = null;

		#endregion //Fields

		public MainWindowViewModel()
		{
			ExecuteCommand = new Command(OnExecuteCommandExecute, OnExecuteCommandCanExecute);

			string uriFromConfig = ConfigurationManager.AppSettings.Get("BaseUri");
			this.Location = uriFromConfig;
			SetBaseUri(uriFromConfig);

		}

		#region Properties

		public override string Title => "WPF Simple HTTP Client";

		#region IsBusy

		public bool IsBusy
		{
			get { return GetValue<bool>(IsBusyProperty); }
			set { SetValue(IsBusyProperty, value); }
		}
		public static readonly PropertyData IsBusyProperty =
			RegisterProperty(nameof(IsBusy), typeof(bool), null,
				(sender, e) => ((MainWindowViewModel)sender).OnIsBusyChanged());

		private void OnIsBusyChanged()
		{
			var pleaseWait = this.GetPleaseWaitService();
			if (this.IsBusy)
			{
				pleaseWait.Show("Please Wait...");
			}
			else
			{
				pleaseWait.Hide();
			}
		}

		#endregion //IsBusy

		#region HttpApiClient

		private HttpApiClient _httpApiClient = null;
		private HttpApiClient HttpApiClient
		{
			get
			{
				if (_httpApiClient == null)
				{
					if (_baseUri == null)
					{
						var task = this.ShowError("The base URI is null!", "Invalid argument");
					}
					else
					{
						_httpApiClient = new HttpApiClient(_baseUri,
							(sender, e) => this.ShowError(e.HierarchyExceptionMessages, e.ExceptionType).GetAwaiter().GetResult());
					}
				}

				return _httpApiClient;
			}
		}

		#endregion //HttpApiClient

		#region Location

		public string Location
		{
			get { return GetValue<string>(LocationProperty); }
			set { SetValue(LocationProperty, value); }
		}
		public static readonly PropertyData LocationProperty = RegisterProperty(nameof(Location), typeof(string), null);

		#endregion //Location

		public DataView Items
		{
			get { return GetValue<DataView>(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}
		public static readonly PropertyData ItemsProperty = RegisterProperty(nameof(Items), typeof(DataView), null);

		#endregion //Properties

		#region Commands

		public Command ExecuteCommand { get; private set; }

		private bool OnExecuteCommandCanExecute()
		{
			return !this.IsBusy;
		}	

		private async void OnExecuteCommandExecute()
		{
			this.IsBusy = true;

			DataTable data = await this.HttpApiClient.GetDataTableAsync("client", "getUser", new object[] { });

			if (data != null)
			{
				this.Items = data.DefaultView;
			}

			this.IsBusy = false;
		}

		#endregion //Commands

		#region Methods

		private void SetBaseUri(string uri)
		{
			if (!string.IsNullOrWhiteSpace(uri) &&
				uri.ValidateUrl())
			{
				_baseUri = new Uri(uri);
			}
		}

		#endregion //Methods
	}
}
