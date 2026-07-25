using System;
using System.Globalization;
using System.Windows.Data;

namespace S9BEditor;

public class SingleElementArrayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return new object[1] { value };
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
