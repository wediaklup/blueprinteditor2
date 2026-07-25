using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace S9BEditor;

internal class BooleanToHiddenVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if ((string)parameter == "!")
		{
			value = !(bool)value;
		}
		return (object)(Visibility)(!(bool)value);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if ((string)parameter == "!")
		{
			value = (((int)(Visibility)value != 0) ? ((object)(Visibility)0) : ((object)(Visibility)1));
		}
		return (int)(Visibility)value == 0;
	}
}
