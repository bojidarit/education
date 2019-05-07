namespace WPFSimpleHttpClient.ViewModels
{
	using Catel.MVVM;
	using Extensions;
	using System;
	using HttpClientWrapper;
	using System.Configuration;
	using Catel.Data;
	using System.Data;
	using System.Collections.ObjectModel;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using System.Linq;

	public class MainWindowViewModel : ViewModelBase
	{
		#region Fields

		private Uri _baseUri = null;
		private readonly string _testLibrary = "oblp_users";	//"Users";

		#endregion //Fields

		public MainWindowViewModel()
		{
			ExecuteCommand = new Command(OnExecuteCommandExecute, OnExecuteCommandCanExecute);

			string uriFromConfig = ConfigurationManager.AppSettings.Get("BaseUri");
			this.Location = uriFromConfig;
			SetBaseUri(uriFromConfig);

			this.Params = new ObservableCollection<string>(new string[10]);

			int loadMethods = 0;
			Int32.TryParse(ConfigurationManager.AppSettings.Get("LoadMethods"), out loadMethods);

			if (loadMethods > 0)
			{
				LoadMethodsAsync();
			}
			else
			{
				//this.Methods = new ObservableCollection<string>();
				this.IsMethodEditable = true;
			}
		}

		#region Properties

		public override string Title => "WPF Simple HTTP Client";

		public bool IsMethodEditable
		{
			get { return GetValue<bool>(IsMethodEditableProperty); }
			set { SetValue(IsMethodEditableProperty, value); }
		}
		public static readonly PropertyData IsMethodEditableProperty = RegisterProperty("IsMethodEditable", typeof(bool), false);

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

		#region Methods To Execute

		public ObservableCollection<string> Methods
		{
			get { return GetValue<ObservableCollection<string>>(MethodsProperty); }
			set { SetValue(MethodsProperty, value); }
		}
		public static readonly PropertyData MethodsProperty = RegisterProperty(nameof(Methods), typeof(ObservableCollection<string>), null);

		public string SelectedMethod
		{
			get { return GetValue<string>(SelectedMethodProperty); }
			set { SetValue(SelectedMethodProperty, value); }
		}
		public static readonly PropertyData SelectedMethodProperty = RegisterProperty(nameof(SelectedMethod), typeof(string), null, (sender, e) => ((MainWindowViewModel)sender).OnSelectedMethodChanged());

		private void OnSelectedMethodChanged()
		{
			base.ViewModelCommandManager.InvalidateCommands(true);
		}

		#endregion //Methods To Execute

		public ObservableCollection<string> Params
		{
			get { return GetValue<ObservableCollection<string>>(ParamsProperty); }
			set { SetValue(ParamsProperty, value); }
		}
		public static readonly PropertyData ParamsProperty = RegisterProperty(nameof(Params), typeof(ObservableCollection<string>), null);

		public bool IsPureString
		{
			get { return GetValue<bool>(IsPureStringProperty); }
			set { SetValue(IsPureStringProperty, value); }
		}
		public static readonly PropertyData IsPureStringProperty = RegisterProperty(nameof(IsPureString), typeof(bool), false);

		#endregion //Properties

		#region Commands

		public Command ExecuteCommand { get; private set; }

		private bool OnExecuteCommandCanExecute()
		{
			return !this.IsBusy && !string.IsNullOrWhiteSpace(this.SelectedMethod);
		}

		private async void OnExecuteCommandExecute()
		{
			this.IsBusy = true;

			if (this.IsPureString)
			{
				await GetString();
			}
			else
			{
				await GetTable();
			}

			this.IsBusy = false;
		}

		#endregion //Commands

		#region Methods

		private async Task GetString()
		{
			HttpData data = await this.HttpApiClient.GetRawDataAsync(
				_testLibrary,
				this.SelectedMethod,
				this.PrepareParameters());

			await this.ShowMessage(data.Content ?? string.Empty, "Pure data");
		}

		private async Task GetTable()
		{
			DataTable data = await this.HttpApiClient.GetDataTableAsync(
				_testLibrary,
				this.SelectedMethod,
				this.PrepareParameters());

			if (data != null)
			{
				this.Items = data.DefaultView;
			}
		}

		private void SetBaseUri(string uri)
		{
			if (!string.IsNullOrWhiteSpace(uri) &&
				uri.ValidateUrl())
			{
				_baseUri = new Uri(uri);
			}
		}

		private void LoadMethodsAsync()
		{
			IEnumerable<string> methods = null;
			Task task = Task.Run(async () => methods = await this.HttpApiClient.GetMethodsAsync(_testLibrary));
			task.ContinueWith(t =>
			{
				if (methods != null && methods.Any())
				{
					this.Methods = new ObservableCollection<string>(methods);
					this.SelectedMethod = this.Methods.First();
				}
				else
				{
					this.IsMethodEditable = true;
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private object[] PrepareParameters()
		{
			List<string> result = new List<string>(
				this.Params.Where(p => !string.IsNullOrWhiteSpace(p)));

			return result.ToArray();
		}

		#endregion //Methods
	}
}
