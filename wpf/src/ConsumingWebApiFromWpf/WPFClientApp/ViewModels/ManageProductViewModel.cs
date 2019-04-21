namespace WPFClientApp.ViewModels
{
	using AutoMapper;
	using Catel.Data;
	using Catel.MVVM;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using WPFClientApp.Models;
	using WPFClientApp.WebApiClient;

	public class ManageProductViewModel : ViewModelBase
	{
		private bool _isNew;
		private WebApiHttpClient _webApiClient = null;

		/// <summary>
		/// Constructor for adding NEW product
		/// </summary>
		public ManageProductViewModel(IEnumerable<IdNameModel> categories,
			WebApiHttpClient webApiClient)
		{
			_isNew = true;
			Init("Create Product", categories, webApiClient);
			this.WorkModel = new ProductModel()
			{
				Id = -1,
				// BUG: If we do not set value that is now whole number, 
				// CATEL wont let one enter the floating point (comma) 
				Price = 0.1M
			};
		}

		/// <summary>
		/// Constructor for edit EXISTING product
		/// </summary>
		public ManageProductViewModel(IEnumerable<IdNameModel> categories,
			WebApiHttpClient webApiClient, ProductModel product)
		{
			this._isNew = false;
			Init("Edit Product", categories, webApiClient);
			this.WorkModel = product;
		}

		#region Properties

		#region WorkModel

		[Model]
		public ProductModel WorkModel
		{
			get { return GetValue<ProductModel>(WorkModelProperty); }
			set { SetValue(WorkModelProperty, value); }
		}

		public static readonly PropertyData WorkModelProperty =
			RegisterProperty("WorkModel", typeof(ProductModel), null, (sender, e) => ((ManageProductViewModel)sender).OnWorkModelChanged());

		private void OnWorkModelChanged()
		{
			if (this.WorkModel != null)
			{
				this.WorkModel.PropertyChanged += this.WorkModel_PropertyChanged;
			}
		}

		private void WorkModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			ViewModelCommandManager.InvalidateCommands(true);
		}

		#endregion //WorkModel

		public IEnumerable<IdNameModel> Categories
		{
			get { return GetValue<IEnumerable<IdNameModel>>(CategoriesProperty); }
			set { SetValue(CategoriesProperty, value); }
		}
		public static readonly PropertyData CategoriesProperty = RegisterProperty("Categories", typeof(IEnumerable<IdNameModel>), null);

		public Uri Uri
		{
			get { return GetValue<Uri>(UriProperty); }
			set { SetValue(UriProperty, value); }
		}
		public static readonly PropertyData UriProperty = RegisterProperty(nameof(Uri), typeof(Uri), null);

		public bool IsBusy
		{
			get { return GetValue<bool>(IsBusyProperty); }
			set { SetValue(IsBusyProperty, value); }
		}
		public static readonly PropertyData IsBusyProperty = RegisterProperty(nameof(IsBusy), typeof(bool), false);

		#endregion //Properties

		#region Commands

		#region OkCommand

		public Command OkCommand { get; private set; }

		private bool OnOkCommandCanExecute()
		{
			return !this.IsBusy && this.WorkModel.IsDirty && (this.WorkModel.CategoryId > 0);
		}

		private async void OnOkCommandExecute()
		{
			bool result = await SaveData();

			if (result)
			{
				await this.SaveAndCloseViewModelAsync();
			}
		}

		#endregion //OkCommand

		#region CancelCommand

		public Command CancelCommand { get; private set; }

		private bool OnCancelCommandCanExecute()
		{
			return !this.IsBusy;
		}

		private async void OnCancelCommandExecute()
		{
			await this.CancelAndCloseViewModelAsync();
		}

		#endregion //CancelCommand

		#endregion //Commands

		#region Methods

		private string GetTitle(string title) =>
			$"{(!string.IsNullOrWhiteSpace(title) ? title + " " : string.Empty)}[{this.GetType().Name}]";

		private void Init(string title, IEnumerable<IdNameModel> categories, WebApiHttpClient webApiClient)
		{
			OkCommand = new Command(OnOkCommandExecute, OnOkCommandCanExecute);
			CancelCommand = new Command(OnCancelCommandExecute, OnCancelCommandCanExecute);

			_webApiClient = webApiClient;

			this.Title = GetTitle(title);
			this.Categories = categories;
		}

		private async Task<bool> SaveData()
		{
			this.IsBusy = true;

			bool result = _isNew ? await AddNew() : await SaveExisting();

			this.IsBusy = false;

			return result;
		}

		private async Task<bool> AddNew()
		{
			Dtos.Product product = Mapper.Map<Dtos.Product>(this.WorkModel);
			this.Uri = await _webApiClient.CreateAsync(Constants.ApiProductsPath, product);

			return (this.Uri != null);
		}

		private async Task<bool> SaveExisting()
		{
			Dtos.Product product = Mapper.Map<Dtos.Product>(this.WorkModel);
			bool output = await _webApiClient.UpdateAsync(Constants.ApiProductsPath, product, product.Id);

			return output;
		}

		#endregion //Methods
	}
}
