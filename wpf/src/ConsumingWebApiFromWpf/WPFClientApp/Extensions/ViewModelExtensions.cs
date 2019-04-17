namespace WPFClientApp.Extensions
{
	using Catel.IoC;
	using Catel.MVVM;
	using Catel.Services;
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
			catch (System.Exception ex)
			{
				//TODO: Log error...
			}

			return result;
		}
	}
}
