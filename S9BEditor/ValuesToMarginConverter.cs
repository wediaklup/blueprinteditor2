using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace S9BEditor;

internal class ValuesToMarginConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (values.Length == 4)
		{
			double[] array = new double[4];
			for (int i = 0; i < 4; i++)
			{
				if (values[i] == null || values[i] == DependencyProperty.UnsetValue)
				{
					array[i] = 0.0;
				}
				else
				{
					array[i] = (double)values[i];
				}
			}
			return (object)new Thickness(array[0], array[1], array[2], array[3]);
		}
		return null;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Thickness val = (Thickness)value;
		if (targetTypes.Length == 4 && targetTypes[0] == typeof(double) && targetTypes[1] == typeof(double) && targetTypes[2] == typeof(double) && targetTypes[3] == typeof(double))
		{
			return new object[4]
			{
				((Thickness)(val)).Left,
				((Thickness)(val)).Top,
				((Thickness)(val)).Right,
				((Thickness)(val)).Bottom
			};
		}
		return null;
	}
}
