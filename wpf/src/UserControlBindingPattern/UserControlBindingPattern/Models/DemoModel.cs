namespace UserControlBindingPattern.Models
{
	public class DemoModel : NotifyPropertyChangedBase
	{
		private string someValue;
		public string SomeValue
		{
			get
			{
				return someValue;
			}
			set
			{
				someValue = value;
				OnPropertyChanged();
			}
		}
	}
}
