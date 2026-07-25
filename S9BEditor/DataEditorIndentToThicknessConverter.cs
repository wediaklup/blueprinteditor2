using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace S9BEditor;

public class DataEditorIndentToThicknessConverter : IValueConverter
{
	public int IndentWidth { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (targetType == typeof(Thickness))
		{
			TreeViewItem val = (TreeViewItem)((value is TreeViewItem) ? value : null);
			int num = 0;
			while (true)
			{
				ItemsControl obj = ItemsControl.ItemsControlFromItemContainer((DependencyObject)(object)val);
				if ((val = (TreeViewItem)(object)((obj is TreeViewItem) ? obj : null)) == null)
				{
					break;
				}
				if ((bool)((DependencyObject)val).GetValue(DataEditor.IndentsChildrenProperty))
				{
					num++;
				}
			}
			Thickness val2 = default(Thickness);
			((Thickness)(val2))._002Ector((double)(num * IndentWidth), 0.0, 0.0, 0.0);
			if (parameter != null && parameter.GetType() == typeof(string))
			{
				string[] array = ((string)parameter).Split(new char[1] { ',' });
				if (array.Length == 4)
				{
					((Thickness)(val2)).Left = ((Thickness)(val2)).Left + double.Parse(array[0], CultureInfo.InvariantCulture);
					((Thickness)(val2)).Top = ((Thickness)(val2)).Top + double.Parse(array[1], CultureInfo.InvariantCulture);
					((Thickness)(val2)).Right = ((Thickness)(val2)).Right + double.Parse(array[2], CultureInfo.InvariantCulture);
					((Thickness)(val2)).Bottom = ((Thickness)(val2)).Bottom + double.Parse(array[3], CultureInfo.InvariantCulture);
				}
			}
			return val2;
		}
		if (targetType == typeof(double))
		{
			TreeViewItem val3 = (TreeViewItem)((value is TreeViewItem) ? value : null);
			int num2 = 0;
			while (true)
			{
				ItemsControl obj2 = ItemsControl.ItemsControlFromItemContainer((DependencyObject)(object)val3);
				if ((val3 = (TreeViewItem)(object)((obj2 is TreeViewItem) ? obj2 : null)) == null)
				{
					break;
				}
				if ((bool)((DependencyObject)val3).GetValue(DataEditor.IndentsChildrenProperty))
				{
					num2++;
				}
			}
			double num3 = num2 * IndentWidth;
			if (parameter != null && parameter.GetType() == typeof(string))
			{
				num3 += double.Parse((string)parameter, CultureInfo.InvariantCulture);
			}
			return num3;
		}
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
