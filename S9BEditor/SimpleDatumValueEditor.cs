using System.Windows;
using System.Windows.Controls;
using S9BEditor.ViewModels;

namespace S9BEditor
{
	internal class SimpleDatumValueEditor : Control
	{
		public static readonly DependencyProperty DatumProperty;

		public VMTypeDatumBase Datum
		{
			get
			{
				return (VMTypeDatumBase)((DependencyObject)this).GetValue(DatumProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(DatumProperty, (object)value);
			}
		}

		static SimpleDatumValueEditor()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			DatumProperty = DependencyProperty.Register("Datum", typeof(VMTypeDatumBase), typeof(SimpleDatumValueEditor), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		}
	}
}
