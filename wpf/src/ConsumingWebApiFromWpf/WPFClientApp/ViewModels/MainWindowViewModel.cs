namespace WPFClientApp.ViewModels
{
	using AutoMapper;
	using Catel.Data;
	using Catel.MVVM;
	using Catel.Services;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using WPFClientApp.Dtos;
	using WPFClientApp.Extensions;
	using WPFClientApp.Models;
	using WPFClientApp.WebApiClient;

	public class MainWindowViewModel : ViewModelBase, IDisposable
	{
		#region Fields

		private Uri _baseUri = null;
		private WebApiHttpClient _webApiClient = null;
		private WebApiHttpClient WebApiClient
		{
			get
			{
				if (_baseUri == null)
				{
					this.ShowError("The base URI is null!", "Invalid argument").GetAwaiter().GetResult();
				}
				else if (_webApiClient == null)
				{
					_webApiClient = new WebApiHttpClient(_baseUri,
						(sender, e) => this.ShowError(e.HierarchyExceptionMessages, e.ExceptionType).GetAwaiter().GetResult());
				}

				return _webApiClient;
			}
		}

		#endregion //Fields

		public MainWindowViewModel()
		{
			LoadCommand = new Command(OnLoadCommandExecute, OnLoadCommandCanExecute);
			NewCommand = new Command(OnNewCommandExecute, OnNewCommandCanExecute);
			EditCommand = new Command<object>(OnEditCommandExecute, OnEditCommandCanExecute);
			DeleteCommand = new Command<object>(OnDeleteCommandExecute);

			// TODO: Let the user enter the location...
			this.Location = @"http://localhost:50118/";
		}

		public void Dispose()
		{
			_webApiClient?.Dispose();
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
			if (!string.IsNullOrWhiteSpace(this.Location) &&
				this.Location.ValidateUrl())
			{
				_baseUri = new Uri(this.Location);
				this.CanChangeLocation = false;
				ViewModelCommandManager.InvalidateCommands(true);
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
			return !this.IsBusy && _baseUri != null;
		}

		private async void OnLoadCommandExecute()
		{
			this.IsBusy = true;

			var categories = await WebApiClient.GetAsync<IEnumerable<Category>>(Constants.ApiCategoriesPath);

			if (categories != null)
			{
				this.Categories = new ObservableCollection<IdNameModel>(
					categories.Select(c => new IdNameModel(c.Id, c.Name)));

				IEnumerable<Product> products = await WebApiClient.GetAsync<IEnumerable<Product>>(Constants.ApiProductsPath);
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
			var vm = new ManageProductViewModel(this.Categories, WebApiClient);
			var result = await this.ShowDialogAsync(vm);

			if (result ?? false)
			{
				if (vm.Uri != null)
				{
					this.IsBusy = true;

					Product newOne = await WebApiClient.GetAsync<Product>(vm.Uri);
					this.Products.Add(Mapper.Map<ProductModel>(newOne));

					this.IsBusy = false;
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
				var vm = new ManageProductViewModel(this.Categories, WebApiClient, model);
				var result = await this.ShowDialogAsync(vm);
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
					this.IsBusy = true;

					bool output = await WebApiClient.DeleteAsync(Constants.ApiProductsPath, model.Id);
					if (output)
					{
						this.Products.Remove(model);
					}

					this.IsBusy = false;
				}
			}
		}

		#endregion //DeleteCommand

		#endregion //Commands
	}
}
