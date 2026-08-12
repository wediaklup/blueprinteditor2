using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace S9BEditor
{
	public class DropDownButton : ToggleButton
	{
		public static readonly DependencyProperty DropDownProperty;

		public ContextMenu DropDown
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (ContextMenu)((DependencyObject)this).GetValue(DropDownProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(DropDownProperty, (object)value);
			}
		}

		protected override void OnClick()
		{
			if (DropDown != null)
			{
				DropDown.PlacementTarget = (UIElement)(object)this;
				DropDown.Placement = (PlacementMode)2;
				DropDown.IsOpen = true;
			}
		}

		static DropDownButton()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(DropDownButton), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		}
	}
}
