namespace WPFClientApp.ViewModels
{
	using Catel.Data;
	using Catel.MVVM;
	using Flurl;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Net.Http;
	using System.Threading.Tasks;
	using WPFClientApp.Dtos;

	public class MainWindowViewModel : ViewModelBase
	{
		/// <summary>
		/// MS Docs: Call a Web API From a .NET Client (C#)
		/// https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
		/// </summary>
		private static HttpClient _client = new HttpClient();
		private static string _apiPath = "api/products/";

		public MainWindowViewModel()
		{
			LoadCommand = new Command(OnLoadCommandExecute, OnLoadCommandCanExecute);
			DeleteCommand = new Command<object>(OnDeleteCommandExecute);
		}

		#region Properties

		public override string Title => "WPFClientApp";

		public bool IsBusy
		{
			get { return GetValue<bool>(IsBusyProperty); }
			set { SetValue(IsBusyProperty, value); }
		}
		public static readonly PropertyData IsBusyProperty = RegisterProperty(nameof(IsBusy), typeof(bool), false);

		public bool HttpClientInitialized
		{
			get { return GetValue<bool>(HttpClientInitializedProperty); }
			set { SetValue(HttpClientInitializedProperty, value); }
		}
		public static readonly PropertyData HttpClientInitializedProperty = 
			RegisterProperty(nameof(HttpClientInitialized), typeof(bool), false);

		public bool CanChangeLocation
		{
			get { return GetValue<bool>(CanChangeLocationProperty); }
			set { SetValue(CanChangeLocationProperty, value); }
		}
		public static readonly PropertyData CanChangeLocationProperty = 
			RegisterProperty(nameof(CanChangeLocation), typeof(bool), true);

		#region Location Base URL

		public string Location
		{
			get { return GetValue<string>(LocationProperty); }
			set { SetValue(LocationProperty, value); }
		}
		public static readonly PropertyData LocationProperty =
			RegisterProperty(nameof(Location), typeof(string), null,
				(sender, e) => ((MainWindowViewModel)sender).OnLocationChanged());

		private void OnLocationChanged()
		{
			if (!string.IsNullOrWhiteSpace(this.Location))
			{
				InitializeHttpClient();
			}
		}

		#endregion //Location Base URL

		public ObservableCollection<Product> Products
		{
			get { return GetValue<ObservableCollection<Product>>(ProductsProperty); }
			set { SetValue(ProductsProperty, value); }
		}
		public static readonly PropertyData ProductsProperty =
			RegisterProperty(nameof(Products), typeof(ObservableCollection<Product>), null);

		public string Message
		{
			get { return GetValue<string>(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}
		public static readonly PropertyData MessageProperty = RegisterProperty(nameof(Message), typeof(string), null);

		#endregion //Properties

		#region Commands

		#region LoadCommand

		public Command LoadCommand { get; private set; }

		private bool OnLoadCommandCanExecute()
		{
			return !this.IsBusy && this.HttpClientInitialized;
		}

		private async void OnLoadCommandExecute()
		{
			this.IsBusy = true;
			IEnumerable<Product> products = await GetAllProductAsync();
			this.Products = new ObservableCollection<Product>(products);
			this.IsBusy = false;
			this.CanChangeLocation = false;
		}

		#endregion //LoadCommand

		#region DeleteCommand

		public Command<object> DeleteCommand { get; private set; }

		private void OnDeleteCommandExecute(object item)
		{
			Product model = item as Product;
			if (model != null)
			{
				// TODO: Handle command logic here
			}
		}

		#endregion //DeleteCommand

		#endregion //Commands

		#region Methods

		/// <summary>
		/// This instance has already started one or more requests. 
		/// Properties can only be modified before sending the first request.
		/// </summary>
		private void InitializeHttpClient()
		{
			try
			{
				_client.BaseAddress = new Uri(this.Location);
				_client.DefaultRequestHeaders.Accept.Clear();
				_client.DefaultRequestHeaders.Accept.Add(
					new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

				this.HttpClientInitialized = true;
			}
			catch (Exception ex)
			{
				this.Message = ex.Message;
			}
		}

		private async Task<IEnumerable<Product>> GetAllProductAsync()
		{
			IEnumerable<Product> products = null;
			string path = Url.Combine(_client.BaseAddress.ToString(), _apiPath);

			this.Message = path;

			try
			{
				HttpResponseMessage response = await _client.GetAsync(path);
				if (response.IsSuccessStatusCode)
				{
					string data = await response.Content.ReadAsStringAsync();
					products = JsonConvert.DeserializeObject<IEnumerable<Product>>(data);
				}
			}
			catch (Exception ex)
			{
				this.Message = ex.Message;
			}

			return products;
		}

		private async Task<Product> GetProductAsync(int productId)
		{
			Product product = null;
			string path = Url.Combine(_client.BaseAddress.ToString(), _apiPath, productId.ToString());

			this.Message = path;

			HttpResponseMessage response = await _client.GetAsync(path);
			if (response.IsSuccessStatusCode)
			{
				string data = await response.Content.ReadAsStringAsync();
				product = JsonConvert.DeserializeObject<Product>(data);
			}

			return product;
		}

		#endregion //Methods
	}
}
