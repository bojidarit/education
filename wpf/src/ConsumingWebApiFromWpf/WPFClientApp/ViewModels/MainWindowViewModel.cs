namespace WPFClientApp.ViewModels
{
	using AutoMapper;
	using Catel.Data;
	using Catel.MVVM;
	using Flurl;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;
	using WPFClientApp.Dtos;
	using WPFClientApp.Extensions;
	using WPFClientApp.Models;

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
			NewCommand = new Command(OnNewCommandExecute, OnNewCommandCanExecute);
			EditCommand = new Command<object>(OnEditCommandExecute, OnEditCommandCanExecute);
			DeleteCommand = new Command<object>(OnDeleteCommandExecute);

			this.Location = @"http://localhost:50118/";
		}

		#region Properties

		public override string Title => "WPFClientApp";

		public ObservableCollection<IdNameModel> Categories
		{
			get { return GetValue<ObservableCollection<IdNameModel>>(CategoriesProperty); }
			set { SetValue(CategoriesProperty, value); }
		}
		public static readonly PropertyData CategoriesProperty = RegisterProperty("Categories", typeof(ObservableCollection<IdNameModel>), null);

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

		public ObservableCollection<ProductModel> Products
		{
			get { return GetValue<ObservableCollection<ProductModel>>(ProductsProperty); }
			set { SetValue(ProductsProperty, value); }
		}
		public static readonly PropertyData ProductsProperty =
			RegisterProperty(nameof(Products), typeof(ObservableCollection<ProductModel>), null);

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

			var categories = await GetAllCategoriesAsync();
			this.Categories = new ObservableCollection<IdNameModel>(
				categories.Select(c => new IdNameModel(c.Id, c.Name)));

			IEnumerable<Product> products = await GetAllProductAsync();
			this.Products = new ObservableCollection<ProductModel>(products.Select(p => Mapper.Map<Product, ProductModel>(p)));

			this.IsBusy = false;
			this.CanChangeLocation = false;
		}

		#endregion //LoadCommand

		#region NewCommand

		public Command NewCommand { get; private set; }

		private bool OnNewCommandCanExecute()
		{
			return !this.IsBusy && this.Categories != null;
		}

		private async void OnNewCommandExecute()
		{
			var vm = new ManageProductViewModel(this.Categories);
			var result = await this.ShowDialogAsync(vm);

			if (result ?? false)
			{
				// TODO: Save changes using Products WEB API...
			}
		}

		#endregion //NewCommand

		#region EditCommand

		public Command<object> EditCommand { get; private set; }

		private bool OnEditCommandCanExecute(object item)
		{
			return !this.IsBusy;
		}

		private async void OnEditCommandExecute(object item)
		{
			ProductModel model = item as ProductModel;
			if (model != null)
			{
				var vm = new ManageProductViewModel(this.Categories, model);
				var result = await this.ShowDialogAsync(vm);

				if (result ?? false)
				{
					// TODO: Save changes using Products WEB API...
				}
			}
		}

		#endregion //EditCommand

		#region DeleteCommand

		public Command<object> DeleteCommand { get; private set; }

		private void OnDeleteCommandExecute(object item)
		{
			ProductModel model = item as ProductModel;
			if (model != null)
			{
				// TODO: Save changes using Products WEB API...
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

		private async Task<IEnumerable<Category>> GetAllCategoriesAsync()
		{
			IEnumerable<Category> categories = null;
			string path = Url.Combine(_client.BaseAddress.ToString(), "api/categories");

			this.Message = path;

			try
			{
				HttpResponseMessage response = await _client.GetAsync(path);
				if (response.IsSuccessStatusCode)
				{
					string data = await response.Content.ReadAsStringAsync();
					categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(data);
				}
			}
			catch (Exception ex)
			{
				this.Message = ex.Message;
			}

			return categories;
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
