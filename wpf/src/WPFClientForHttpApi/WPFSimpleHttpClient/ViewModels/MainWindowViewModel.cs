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
	using WPFSimpleHttpClient.Dtos;

	public class MainWindowViewModel : ViewModelBase
	{
		#region Fields

		private Uri _baseUri = null;
		private const string _testLibrary = "oblp_users";

		#endregion //Fields

		public MainWindowViewModel()
		{
			ExecuteCommand = new Command(OnExecuteCommandExecute, OnExecuteCommandCanExecute);
			ExecuteValueCommand = new Command(OnExecuteValueCommandExecute, OnExecuteValueCommandCanExecute);

			string uriFromConfig = ConfigurationManager.AppSettings.Get("BaseUri");
			this.Location = uriFromConfig;
			SetBaseUri(uriFromConfig);

			LoadLibraries();

			this.Params = new ObservableCollection<string>(new string[10]);

			int loadMethods = 0;
			Int32.TryParse(ConfigurationManager.AppSettings.Get("LoadMethods"), out loadMethods);

			if (loadMethods > 0)
			{
				LoadMethodsAsync();
			}
			else
			{
				this.IsMethodEditable = true;
			}

			LoadValueTypes();
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
							(sender, e) => this.ShowError(e.HierarchyExceptionMessages, e.ExceptionType).GetAwaiter().GetResult(),
							(sender, e) => this.Message = e.RequestUri.ToString());
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

		#region Libraries

		public ObservableCollection<string> Libraries
		{
			get { return GetValue<ObservableCollection<string>>(LibrariesProperty); }
			set { SetValue(LibrariesProperty, value); }
		}
		public static readonly PropertyData LibrariesProperty = RegisterProperty("Libraries", typeof(ObservableCollection<string>), null);

		public string SelectedLibrary
		{
			get { return GetValue<string>(SelectedLibraryProperty); }
			set { SetValue(SelectedLibraryProperty, value); }
		}

		public static readonly PropertyData SelectedLibraryProperty =
			RegisterProperty("SelectedLibrary", typeof(string), null,
				(sender, e) => ((MainWindowViewModel)sender).OnSelectedLibraryChanged());

		private void OnSelectedLibraryChanged()
		{
			base.ViewModelCommandManager.InvalidateCommands(true);
		}

		#endregion //Libraries

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
		public static readonly PropertyData SelectedMethodProperty =
			RegisterProperty(nameof(SelectedMethod), typeof(string), null,
				(sender, e) => ((MainWindowViewModel)sender).OnSelectedMethodChanged());

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

		#region Value Types

		public ObservableCollection<string> ValueTypes
		{
			get { return GetValue<ObservableCollection<string>>(ValueTypesProperty); }
			set { SetValue(ValueTypesProperty, value); }
		}
		public static readonly PropertyData ValueTypesProperty = RegisterProperty("ValueTypes", typeof(ObservableCollection<string>), null);

		public string SelectedValueType
		{
			get { return GetValue<string>(SelectedValueTypeProperty); }
			set { SetValue(SelectedValueTypeProperty, value); }
		}

		public static readonly PropertyData SelectedValueTypeProperty =
			RegisterProperty("SelectedValueType", typeof(string), null,
				(sender, e) => ((MainWindowViewModel)sender).OnSelectedValueTypeChanged());

		private void OnSelectedValueTypeChanged()
		{
			base.ViewModelCommandManager.InvalidateCommands(true);
		}

		#endregion //Value Types

		public string Message
		{
			get { return GetValue<string>(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}
		public static readonly PropertyData MessageProperty = RegisterProperty("Message", typeof(string), null);

		#endregion //Properties

		#region Commands

		public Command ExecuteCommand { get; private set; }

		private bool OnExecuteCommandCanExecute()
		{
			return !this.IsBusy && !string.IsNullOrWhiteSpace(this.SelectedLibrary)
				&& !string.IsNullOrWhiteSpace(this.SelectedMethod);
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

		#region ExecuteValueCommand

		public Command ExecuteValueCommand { get; private set; }

		private bool OnExecuteValueCommandCanExecute()
		{
			return OnExecuteCommandCanExecute() && !string.IsNullOrWhiteSpace(this.SelectedValueType);
		}

		private async void OnExecuteValueCommandExecute()
		{
			this.IsBusy = true;

			Tuple<bool, string> result = await SendHttpRequestForSingleValue();

			this.IsBusy = false;

			if (result.Item1)
			{
				await this.ShowDialogAsync(new PureDataViewModel(result.Item2));
			}
		}

		#endregion //ExecuteValueCommand

		#endregion //Commands

		#region Methods

		private async Task GetString()
		{
			string result = string.Empty;

			HttpData<string> data = await this.HttpApiClient.GetRawDataAsync(
				this.SelectedLibrary,
				this.SelectedMethod,
				this.PrepareParameters());

			var task = this.ShowDialogAsync(new PureDataViewModel(data));
		}

		private async Task GetTable()
		{
			this.Items = null;

			DataTable data = await this.HttpApiClient.GetDataTableAsync(
				this.SelectedLibrary,
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

		/// <summary>
		/// Value types - Loading static content
		/// </summary>
		private void LoadValueTypes()
		{
			this.ValueTypes = new ObservableCollection<string>() {
				typeof(string).Name,
				typeof(int).Name,
				typeof(decimal).Name,
				typeof(DateTime).Name
			};
		}

		/// <summary>
		/// Libraries - Loading static content
		/// </summary>
		private void LoadLibraries()
		{
			this.Libraries = new ObservableCollection<string>()
			{
				"oBLF_ABCENCE",
				"oBLP_OTPCALC",
				"oBLP_APPOINTMENT",
				"oBLP_AUTOTEXT",
				"oBLF_BACKUP",
				"oBLF_BEDARF",
				"oBLF_ZULAGEN",
				"oBLF_CHIPSYS",
				"oBLP_CONTROLSTAT",
				"oBLF_DPLAN",
				"oBLP_DPLANAUTO",
				"oBLF_DPLANPROP",
				"oBLF_BEREIT",
				"oBLF_EDITHRS",
				"oBLP_ENGINE",
				"oBLF_EXPORT",
				"oBLP_EXPPLANS",
				"oBLP_FAVORITES",
				"oBLP_FORMULAS",
				"oBLP_INSTITUTE",
				"oBLP_INTEGRATION",
				"oBLP_LEGRUL",
				"oBLP_LICENSE",
				"oBLF_LISTEDIT",
				"oBLP_LISTTYPES",
				"oBLP_LOHNEW",
				"oBLP_OPTHIST",
				"oBLP_OPTIONS",
				"oBLP_PARALLELSTAT",
				"oBLF_PAUSEADD",
				"oBLF_PERSONS",
				"oBLP_PROCADD",
				"oBLP_RECALCS",
				"oBLP_DPREP",
				"oBLF_DIENSTE",
				"oBLP_MANDANTS",
				"oBLP_STATISTICS",
				"oBLF_STDCYCLE",
				"oBLF_STNDOCS",
				"oBLP_SYSSTATS",
				"oBLF_UPLAN",
				"oBLP_USERS",
				"oBLP_VIEWS",
				"oBLF_WORKMODEL"
			};

			this.SelectedLibrary = this.Libraries
				.Where(l => string.Compare(_testLibrary, l, true) == 0)
				.FirstOrDefault();
		}

		private async Task<Tuple<bool, string>> SendHttpRequestForSingleValue()
		{
			string result = string.Empty;
			bool ok = true;

			if (this.SelectedValueType == typeof(decimal).Name || this.SelectedValueType == typeof(int).Name)
			{
				HttpData<NumberDto[]> dDto = await this.HttpApiClient.GetDataAsync<NumberDto>(this.SelectedLibrary, this.SelectedMethod, this.PrepareParameters());
				ok = dDto.IsSuccessStatusCode;
				if (dDto.CheckHttpData())
				{
					decimal number = dDto.Content[0].Result;
					result = (this.SelectedValueType == typeof(int).Name) ? number.ToString("F0") : number.ToString("F9");
				}
			}
			else if (this.SelectedValueType == typeof(DateTime).Name)
			{
				HttpData<DateTimeDto[]> dtDto = await this.HttpApiClient.GetDataAsync<DateTimeDto>(this.SelectedLibrary, this.SelectedMethod, this.PrepareParameters());
				ok = dtDto.IsSuccessStatusCode;
				if (dtDto.CheckHttpData())
				{
					result = dtDto.Content[0].ToString();
				}
			}
			else
			{
				HttpData<StringDto[]> sDto = await this.HttpApiClient.GetDataAsync<StringDto>(this.SelectedLibrary, this.SelectedMethod, this.PrepareParameters());
				ok = sDto.IsSuccessStatusCode;
				if (sDto.CheckHttpData())
				{
					result = sDto.Content[0].ToString();
				}
			}

			return Tuple.Create(ok, result);
		}

		#endregion //Methods
	}
}
