using System;
using System.Globalization;
using System.Windows.Data;

namespace S9BEditor;

public class EBoolToStringConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string text = (string)value;
		bool flag = text == "eTrue";
		return flag;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if ((bool)value)
		{
			return "eTrue";
		}
		return "eFalse";
	}
}
