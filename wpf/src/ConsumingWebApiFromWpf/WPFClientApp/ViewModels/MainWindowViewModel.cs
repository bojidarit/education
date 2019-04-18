namespace WPFClientApp.ViewModels
{
	using AutoMapper;
	using Catel.Data;
	using Catel.MVVM;
	using Catel.Services;
	using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Text;
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
		private static string _apiProductsPath = "api/products/";
		private static string _apiCategoriesPath = "api/categories";

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

			if (categories != null)
			{
				this.Categories = new ObservableCollection<IdNameModel>(
					categories.Select(c => new IdNameModel(c.Id, c.Name)));

				IEnumerable<Product> products = await GetAllProductAsync();
				if (products != null)
				{
					this.Products = new ObservableCollection<ProductModel>(
						products.Select(p => Mapper.Map<Product, ProductModel>(p)));
				}
			}

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
				Product product = Mapper.Map<Product>(vm.WorkModel);
				Uri uri = await CreateProductAsync(product);
				if (uri != null)
				{
					Product newOne = await GetProductAsync(uri);
					this.Products.Add(Mapper.Map<ProductModel>(newOne));
				}
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
					Product product = Mapper.Map<Product>(vm.WorkModel);
					bool output = await UpdateProductAsync(product);

					if (!output)
					{
						//TODO: Revert changes...
					}
				}
			}
		}

		#endregion //EditCommand

		#region DeleteCommand

		public Command<object> DeleteCommand { get; private set; }

		private async void OnDeleteCommandExecute(object item)
		{
			ProductModel model = item as ProductModel;
			if (model != null)
			{
				if (await this.ShowMessage($"Do you really want to delete product '{model.Name}'?", "Please Confirm",
					MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
				{
					int statusCode = await DeleteProductAsync(model.Id);
					if (statusCode >= 0)
					{
						HttpStatusCode httpStatus = (HttpStatusCode)statusCode;
						await this.ShowMessage($"Status Code '{httpStatus}'", "DELETE HTTP Request", MessageButton.OK);

						// Remove product only when the delete is successful
						this.Products.Remove(model);
					}

					//OnLoadCommandExecute();
				}
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
				this.ShowError(ex).GetAwaiter().GetResult();
			}
		}

		#region GET requests

		private async Task<IEnumerable<Category>> GetAllCategoriesAsync()
		{
			IEnumerable<Category> categories = null;
			string path = MakeRequestUri(_apiCategoriesPath);

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
				await HandleHttpException(ex, path);
			}

			return categories;
		}

		private async Task<IEnumerable<Product>> GetAllProductAsync()
		{
			IEnumerable<Product> products = null;
			string path = MakeRequestUri(_apiProductsPath);

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
				await HandleHttpException(ex, path);
			}

			return products;
		}

		private async Task<Product> GetProductAsync(int productId)
		{
			Product product = null;
			string path = MakeRequestUri(_apiProductsPath, productId.ToString());

			try
			{
				HttpResponseMessage response = await _client.GetAsync(path);
				if (response.IsSuccessStatusCode)
				{
					string data = await response.Content.ReadAsStringAsync();
					product = JsonConvert.DeserializeObject<Product>(data);
				}
			}
			catch (Exception ex)
			{
				await HandleHttpException(ex, path);
			}

			return product;
		}

		private async Task<Product> GetProductAsync(Uri uri)
		{
			Product product = null;

			try
			{
				HttpResponseMessage response = await _client.GetAsync(uri.PathAndQuery);
				if (response.IsSuccessStatusCode)
				{
					string data = await response.Content.ReadAsStringAsync();
					product = JsonConvert.DeserializeObject<Product>(data);
				}
			}
			catch (Exception ex)
			{
				await HandleHttpException(ex, uri.PathAndQuery);
			}

			return product;
		}

		#endregion //GET request

		/// <summary>
		/// Makes a POST request to create new Product
		/// </summary>
		/// <returns>return URI of the created resource</returns>
		private async Task<Uri> CreateProductAsync(Product product)
		{
			HttpResponseMessage response = null;
			bool failed = false;

			try
			{
				response = await _client.PostAsJsonAsync(_apiProductsPath, product);
				// throws an exception if the status code falls outside the range 200–299
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				failed = true;
				await HandleHttpException(ex, MakeRequestUri(_apiProductsPath));
			}

			return failed ? null : response.Headers.Location;
		}

		public async Task<bool> UpdateProductAsync(Product product)
		{
			bool result = true;
			string path = MakeRequestUri(_apiProductsPath, product.Id.ToString());

			try
			{
				HttpResponseMessage response = await _client.PutAsJsonAsync(path, product);
				response.EnsureSuccessStatusCode();

				// ??? De-serialize the updated product from the response body.
				//string json = await response.Content.ReadAsStringAsync();
				//result = JsonConvert.DeserializeObject<Product>(json);
			}
			catch (Exception ex)
			{
				result = false;
				await HandleHttpException(ex, path);
			}

			return result;
		}

		private async Task<int> DeleteProductAsync(int productId)
		{
			int statusCode = -1;
			string path = MakeRequestUri(_apiProductsPath, productId.ToString());

			try
			{
				HttpResponseMessage response = await _client.DeleteAsync(path);

				// throws an exception if the status code falls outside the range 200–299
				response.EnsureSuccessStatusCode();

				statusCode = (int)response.StatusCode;
			}
			catch (Exception ex)
			{
				await HandleHttpException(ex, MakeRequestUri(_apiProductsPath));
			}

			return statusCode;
		}

		private string MakeRequestUri(string apiPath, string parameter = "") =>
			Flurl.Url.Combine(_client.BaseAddress.ToString(), apiPath, parameter);

		private async Task HandleHttpException(Exception exception, string requestUri)
		{
			StringBuilder stringBuilder = new StringBuilder($"Request Uri: {requestUri} {Environment.NewLine}");
			GetInnerExceptions(exception, stringBuilder);
			await this.ShowError(stringBuilder.ToString(), exception.GetType().Name);
		}

		private void GetInnerExceptions(Exception exception, StringBuilder stringBuilder)
		{
			stringBuilder.Append($"{exception.Message} {Environment.NewLine}");
			if (exception.InnerException != null)
			{
				GetInnerExceptions(exception.InnerException, stringBuilder);
			}
		}

		#endregion //Methods
	}
}
