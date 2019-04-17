namespace WPFClientApp.Converters
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Windows.Data;

	[ValueConversion(typeof(int), typeof(string))]
	public class CollectionViewSourceConverter : BaseConverter, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = string.Empty;

			CollectionViewSource cvs = parameter as CollectionViewSource;
			if (cvs != null && cvs.Source != null)
			{
				int id = 0;
				if (Int32.TryParse(value.ToString(), out id))
				{
					IEnumerable<Models.IdNameModel> collection = cvs.Source as IEnumerable<Models.IdNameModel>;
					if (collection != null)
					{
						result = collection.Where(i => i.Id == id).FirstOrDefault()?.Name;
					}
				}
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
