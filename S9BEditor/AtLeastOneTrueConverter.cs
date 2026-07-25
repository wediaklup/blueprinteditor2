using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace S9BEditor;

internal class AtLeastOneTrueConverter : IMultiValueConverter
{
	public static AtLeastOneTrueConverter Instance { get; private set; }

	static AtLeastOneTrueConverter()
	{
		Instance = new AtLeastOneTrueConverter();
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = false;
		for (int i = 0; i < values.Length; i++)
		{
			if ((bool)values[i])
			{
				flag = true;
				break;
			}
		}
		if (parameter != null)
		{
			if (parameter is IList { Count: 2 } list)
			{
				return list[(!flag) ? 1 : 0];
			}
			return null;
		}
		return flag;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
