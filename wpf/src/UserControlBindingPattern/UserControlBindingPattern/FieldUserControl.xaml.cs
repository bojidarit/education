namespace UserControlBindingPattern
{
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// Interaction logic for FieldUserControl.xaml
	/// </summary>
	public partial class FieldUserControl : UserControl
	{
		public FieldUserControl()
		{
			InitializeComponent();
		}

		#region Dependency Properties

		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}
		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register("Label", typeof(string), typeof(FieldUserControl), new PropertyMetadata(null));

		public double? Value
		{
			get { return (double?)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double?), typeof(FieldUserControl), new PropertyMetadata(null));

		#endregion
	}
}
