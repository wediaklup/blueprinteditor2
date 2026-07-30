using System;
using System.Globalization;
using System.Windows.Data;

namespace S9BEditor
{
	internal class FormatStringConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			string format = parameter as string;
			return string.Format(CultureInfo.CurrentCulture, format, values);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
