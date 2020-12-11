namespace UserControlBindingPattern.Models
{
	using System;

	public class SizeModel : NotifyPropertyChangedBase
	{
		private double? height;
		public double? Height
		{
			get
			{
				return height;
			}

			set
			{
				height = value;
				OnPropertyChanged();
			}
		}

		private double? width;
		public double? Width
		{
			get
			{
				return width;
			}

			set
			{
				width = value;
				OnPropertyChanged();
			}
		}

		public override string ToString() =>
			$"Height = {PropertyToString(Height)}{Environment.NewLine}Width = {PropertyToString(Width)}";

		private string PropertyToString(double? value) =>
			value.HasValue ? value.Value.ToString("f2") : "<NULL>";
	}
}
