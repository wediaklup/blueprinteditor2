using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace S9BEditor
{
	internal class UriToBitmapImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			Uri uri = value as Uri;
			if (uri != null)
			{
				BitmapImage val = new BitmapImage(uri);
				Image val2 = new Image();
				val2.Source = (ImageSource)(object)val;
				((FrameworkElement)val2).MaxWidth = ((BitmapSource)val).PixelWidth;
				((FrameworkElement)val2).MaxHeight = ((BitmapSource)val).PixelHeight;
				return val2;
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
