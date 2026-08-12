using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace S9BEditor
{
	public class DataTextBox : TextBox
	{
		public static readonly DependencyProperty DataUnitsProperty;

		private bool mIsMouseDown;

		public string DataUnits
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(DataUnitsProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(DataUnitsProperty, (object)value);
			}
		}

		static DataTextBox()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			DataUnitsProperty = DependencyProperty.Register("DataUnits", typeof(string), typeof(DataTextBox), (PropertyMetadata)new UIPropertyMetadata((object)""));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataTextBox), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(DataTextBox)));
		}

		protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			if ((int)e.ChangedButton == 0 && e.ClickCount == 3)
			{
				((TextBoxBase)this).SelectAll();
			}
			mIsMouseDown = true;
			base.OnPreviewMouseDown(e);
		}

		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			mIsMouseDown = false;
			base.OnPreviewMouseUp(e);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
		}

		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			if (!(e.OldFocus is ContextMenu) && !mIsMouseDown)
			{
				((TextBoxBase)this).SelectAll();
			}
			base.OnGotKeyboardFocus(e);
		}
	}
}
