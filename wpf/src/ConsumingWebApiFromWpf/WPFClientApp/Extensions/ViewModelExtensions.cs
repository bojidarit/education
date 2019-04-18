namespace WPFClientApp.Extensions
{
	using Catel.IoC;
	using Catel.MVVM;
	using Catel.Services;
	using System;
	using System.Threading.Tasks;

	public static class ViewModelExtensions
	{
		public async static Task<bool?> ShowDialogAsync(this ViewModelBase viewModelParent, ViewModelBase viewModelDialog)
		{
			bool? result = null;

			try
			{
				var dependencyResolver = viewModelParent.GetDependencyResolver();
				var uiVisualizerService = dependencyResolver.Resolve<IUIVisualizerService>();
				result = await uiVisualizerService.ShowDialogAsync(viewModelDialog);
			}
			catch (Exception ex)
			{
				await viewModelParent.ShowError(ex);
			}

			return result;
		}

		public static IMessageService GetMessageService(this ViewModelBase viewModel)
		{
			var dependencyResolver = viewModel.GetDependencyResolver();
			return dependencyResolver.Resolve<IMessageService>();
		}

		public async static Task<MessageResult> ShowMessage(this ViewModelBase viewModel,
			string message, string caption = "", MessageButton button = MessageButton.OK,
			MessageImage icon = MessageImage.None) =>
			await viewModel.GetMessageService().ShowAsync(message, caption, button, icon);

		public async static Task<MessageResult> ShowError(this ViewModelBase viewModel, Exception exception) =>
			await viewModel.GetMessageService().ShowErrorAsync(exception);

		public async static Task<MessageResult> ShowError(this ViewModelBase viewModel, 
			string message, string caption = "") =>
			await viewModel.GetMessageService().ShowErrorAsync(message, caption);
	}
}
