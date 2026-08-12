using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace S9BEditor
{
	public class IsNotNullVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (object)(Visibility)((value == null) ? 2 : 0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
