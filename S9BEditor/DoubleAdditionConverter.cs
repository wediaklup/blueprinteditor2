using System;
using System.Globalization;
using System.Windows.Data;

namespace S9BEditor;

internal class DoubleAdditionConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		double num = 0.0;
		for (int i = 0; i < values.Length; i++)
		{
			num += (double)values[i];
		}
		return num;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
