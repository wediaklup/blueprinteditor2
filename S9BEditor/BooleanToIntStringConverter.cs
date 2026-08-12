using System;
using System.Globalization;
using System.Windows.Data;

namespace S9BEditor
{
	public class BooleanToIntStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = (string)value;
			bool flag = text == "1";
			return flag;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return "1";
			}
			return "0";
		}
	}
}
