namespace WPFClientApp.ViewModels
{
	using AutoMapper;
	using Catel.Data;
	using Catel.MVVM;
	using System.Collections.Generic;
	using WPFClientApp.Dtos;
	using WPFClientApp.Models;

	public class ManageProductViewModel : ViewModelBase
	{
		/// <summary>
		/// Constructor for adding NEW product
		/// </summary>
		public ManageProductViewModel(IEnumerable<IdNameModel> categories)
		{
			Init("Create Product", categories);
		}

		/// <summary>
		/// Constructor for edit EXISTING product
		/// </summary>
		public ManageProductViewModel(IEnumerable<IdNameModel> categories, ProductModel product)
		{
			Init("Edit Product", categories);
			this.WorkModel = product;
		}

		#region Properties

		[Model]
		public ProductModel WorkModel
		{
			get { return GetValue<ProductModel>(WorkModelProperty); }
			set { SetValue(WorkModelProperty, value); }
		}
		public static readonly PropertyData WorkModelProperty = RegisterProperty("WorkModel", typeof(ProductModel), null);

		public IEnumerable<IdNameModel> Categories
		{
			get { return GetValue<IEnumerable<IdNameModel>>(CategoriesProperty); }
			set { SetValue(CategoriesProperty, value); }
		}
		public static readonly PropertyData CategoriesProperty = RegisterProperty("Categories", typeof(IEnumerable<IdNameModel>), null);

		#endregion //Properties

		#region Commands

		#region OkCommand

		public Command OkCommand { get; private set; }

		private bool OnOkCommandCanExecute()
		{
			return this.WorkModel.IsDirty;
		}

		private async void OnOkCommandExecute()
		{
			await this.SaveAndCloseViewModelAsync();
		}

		#endregion //OkCommand

		#region CancelCommand

		public Command CancelCommand { get; private set; }

		private async void OnCancelCommandExecute()
		{
			await this.CancelAndCloseViewModelAsync();
		}

		#endregion //CancelCommand

		#endregion //Commands

		#region Methods

		private string GetTitle(string title) =>
			$"{(!string.IsNullOrWhiteSpace(title) ? title + " " : string.Empty)}[{this.GetType().Name}]";

		private void Init(string title, IEnumerable<IdNameModel> categories)
		{
			OkCommand = new Command(OnOkCommandExecute, OnOkCommandCanExecute);
			CancelCommand = new Command(OnCancelCommandExecute);

			this.Title = GetTitle(title);
			this.Categories = categories;
		}

		#endregion //Methods
	}
}
