namespace CefWebDemo
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class EnvironmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            Environment.Is64BitProcess? "x64" : "x86";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
