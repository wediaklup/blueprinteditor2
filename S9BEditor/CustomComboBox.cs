using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace S9BEditor
{
	internal class CustomComboBox : Control
	{
		public static readonly DependencyProperty ValueProperty;

		public static readonly DependencyProperty ValueTemplateProperty;

		public static readonly DependencyProperty IsDropDownOpenProperty;

		public static readonly DependencyProperty IsReadOnlyProperty;

		public static readonly DependencyProperty IsEditableProperty;

		public static readonly DependencyProperty DropDownContentProperty;

		public static readonly DependencyProperty DropDownContentTemplateProperty;

		private Popup mPopup;

		public object DropDownContent
		{
			get
			{
				return ((DependencyObject)this).GetValue(DropDownContentProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(DropDownContentProperty, value);
			}
		}

		public object Value
		{
			get
			{
				return ((DependencyObject)this).GetValue(ValueProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ValueProperty, value);
			}
		}

		public DataTemplate ValueTemplate
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (DataTemplate)((DependencyObject)this).GetValue(ValueTemplateProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ValueTemplateProperty, (object)value);
			}
		}

		public bool IsDropDownOpen
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsDropDownOpenProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsDropDownOpenProperty, (object)value);
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsReadOnlyProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsReadOnlyProperty, (object)value);
			}
		}

		public bool IsEditable
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsEditableProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsEditableProperty, (object)value);
			}
		}

		public DataTemplate DropDownContentTemplate
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (DataTemplate)((DependencyObject)this).GetValue(DropDownContentTemplateProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(DropDownContentTemplateProperty, (object)value);
			}
		}

		static CustomComboBox()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Expected O, but got Unknown
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Expected O, but got Unknown
			ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(CustomComboBox), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			ValueTemplateProperty = DependencyProperty.Register("ValueTemplate", typeof(DataTemplate), typeof(CustomComboBox), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(CustomComboBox), (PropertyMetadata)new UIPropertyMetadata(new PropertyChangedCallback(onIsDropDownOpenChanged)));
			IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(CustomComboBox), (PropertyMetadata)new UIPropertyMetadata((object)false));
			IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(CustomComboBox), (PropertyMetadata)new UIPropertyMetadata((object)false));
			DropDownContentProperty = DependencyProperty.Register("DropDownContent", typeof(object), typeof(CustomComboBox), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			DropDownContentTemplateProperty = DependencyProperty.Register("DropDownContentTemplate", typeof(DataTemplate), typeof(CustomComboBox), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomComboBox), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(CustomComboBox)));
			EventManager.RegisterClassHandler(typeof(CustomComboBox), Mouse.LostMouseCaptureEvent, (Delegate)new MouseEventHandler(OnLostMouseCapture));
			EventManager.RegisterClassHandler(typeof(CustomComboBox), Mouse.MouseDownEvent, (Delegate)new MouseButtonEventHandler(OnMouseButtonDown), true);
		}

		public override void OnApplyTemplate()
		{
			if (mPopup != null)
			{
				mPopup = null;
			}
			DependencyObject templateChild = ((FrameworkElement)this).GetTemplateChild("PART_Popup");
			mPopup = (Popup)(object)((templateChild is Popup) ? templateChild : null);
			_ = mPopup;
			((FrameworkElement)this).OnApplyTemplate();
		}

		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			CustomComboBox customComboBox = (CustomComboBox)sender;
			if (!((UIElement)customComboBox).IsKeyboardFocusWithin)
			{
				((UIElement)customComboBox).Focus();
			}
			if ((object)Mouse.Captured == customComboBox && ((RoutedEventArgs)e).OriginalSource == customComboBox)
			{
				Mouse.Capture((IInputElement)null);
			}
		}

		private static void onIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CustomComboBox customComboBox = (CustomComboBox)(object)d;
			if ((bool)((DependencyPropertyChangedEventArgs)(e)).NewValue)
			{
				Mouse.Capture((IInputElement)(object)customComboBox, (CaptureMode)2);
			}
			else if ((object)Mouse.Captured == customComboBox)
			{
				Mouse.Capture((IInputElement)null);
			}
		}

		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			CustomComboBox customComboBox = (CustomComboBox)sender;
			if ((object)Mouse.Captured == customComboBox)
			{
				return;
			}
			if (((RoutedEventArgs)e).OriginalSource == customComboBox)
			{
				if (Mouse.Captured != null)
				{
					IInputElement captured = Mouse.Captured;
					if (S9BEUtil.IsPopupDescendant((DependencyObject)(object)customComboBox, (DependencyObject)(object)((captured is DependencyObject) ? captured : null)))
					{
						return;
					}
				}
				customComboBox.IsDropDownOpen = false;
				return;
			}
			object originalSource = ((RoutedEventArgs)e).OriginalSource;
			if (S9BEUtil.IsPopupDescendant((DependencyObject)(object)customComboBox, (DependencyObject)((originalSource is DependencyObject) ? originalSource : null)))
			{
				if (customComboBox.IsDropDownOpen && Mouse.Captured == null && Native.GetCapture() == IntPtr.Zero)
				{
					Mouse.Capture((IInputElement)(object)customComboBox, (CaptureMode)2);
					((RoutedEventArgs)e).Handled = true;
				}
			}
			else
			{
				customComboBox.IsDropDownOpen = false;
			}
		}
	}
}
