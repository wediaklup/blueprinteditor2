using System;
using System.Globalization;
using System.Windows.Data;

namespace S9BEditor;

public class FloatingPointSignChangeConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		double num = (double)value;
		num = 0.0 - num;
		return System.Convert.ChangeType(num, targetType, CultureInfo.InvariantCulture);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return Convert(value, targetType, parameter, culture);
	}
}
