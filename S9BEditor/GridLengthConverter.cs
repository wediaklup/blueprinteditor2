using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace S9BEditor
{
	public class GridLengthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return (object)new GridLength((double)value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			GridLength val = (GridLength)value;
			if (((GridLength)(val)).IsAbsolute)
			{
				return ((GridLength)(val)).Value;
			}
			return double.NaN;
		}
	}
}
