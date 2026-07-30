using System.Windows;
using System.Windows.Controls;

namespace S9BEditor
{
	public class FixedWidthGridViewColumn : GridViewColumn
	{
		public static readonly DependencyProperty FixedWidthProperty;

		public double FixedWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(FixedWidthProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(FixedWidthProperty, (object)value);
			}
		}

		static FixedWidthGridViewColumn()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			FixedWidthProperty = DependencyProperty.Register("FixedWidth", typeof(double), typeof(FixedWidthGridViewColumn), (PropertyMetadata)new FrameworkPropertyMetadata((object)double.NaN, new PropertyChangedCallback(OnFixedWidthChanged)));
			GridViewColumn.WidthProperty.OverrideMetadata(typeof(FixedWidthGridViewColumn), (PropertyMetadata)new FrameworkPropertyMetadata((PropertyChangedCallback)null, new CoerceValueCallback(OnCoerceWidth)));
		}

		private static object OnCoerceWidth(DependencyObject o, object baseValue)
		{
			if (o is FixedWidthGridViewColumn fixedWidthGridViewColumn)
			{
				return fixedWidthGridViewColumn.FixedWidth;
			}
			return baseValue;
		}

		private static void OnFixedWidthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (o is FixedWidthGridViewColumn fixedWidthGridViewColumn)
			{
				((DependencyObject)fixedWidthGridViewColumn).CoerceValue(GridViewColumn.WidthProperty);
			}
		}
	}
}
