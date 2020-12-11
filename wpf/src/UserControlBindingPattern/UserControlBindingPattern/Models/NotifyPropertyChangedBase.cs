namespace UserControlBindingPattern.Models
{
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	// This class implements INotifyPropertyChanged
	// to support one-way and two-way bindings
	// (such that the UI element updates when the source
	// has been changed dynamically)
	// Source: https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-implement-property-change-notification
	public class NotifyPropertyChangedBase : INotifyPropertyChanged
	{
		// Declare the event
		public event PropertyChangedEventHandler PropertyChanged;

		// The OnPropertyChanged method to raise the event
		// The calling member's name will be used as the parameter.
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
