using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace S9BEditor
{
	public class TreeViewImageItem : TreeViewItem
	{
		public static readonly DependencyProperty ImageSourceProperty;

		public static readonly DependencyProperty IconSourceUriProperty;

		public static readonly DependencyProperty ImageSizeProperty;

		public static readonly DependencyProperty EditTextProperty;

		public static readonly DependencyProperty IsEditableProperty;

		private DispatcherTimer mClickedTimer;

		public static readonly DependencyProperty IsEditingProperty;

		private TextBox mEditBox;

		public ImageSource ImageSource
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (ImageSource)((DependencyObject)this).GetValue(ImageSourceProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ImageSourceProperty, (object)value);
			}
		}

		public Uri IconSourceUri
		{
			get
			{
				return (Uri)((DependencyObject)this).GetValue(IconSourceUriProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IconSourceUriProperty, (object)value);
				OnIconSourceUriChanged(EventArgs.Empty);
			}
		}

		public Size ImageSize
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (Size)((DependencyObject)this).GetValue(ImageSizeProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((DependencyObject)this).SetValue(ImageSizeProperty, (object)value);
				OnImageSizeChanged(EventArgs.Empty);
			}
		}

		public string EditText
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(EditTextProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(EditTextProperty, (object)value);
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

		public bool IsEditing
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(IsEditingProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(IsEditingProperty, (object)value);
			}
		}

		public event EventHandler IconSourceUriChanged;

		private event EventHandler ImageSizeChanged;

		static TreeViewImageItem()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Expected O, but got Unknown
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(TreeViewImageItem), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			IconSourceUriProperty = DependencyProperty.Register("IconSourceUri", typeof(Uri), typeof(TreeViewImageItem), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			ImageSizeProperty = DependencyProperty.Register("ImageSize", typeof(Size), typeof(TreeViewImageItem), (PropertyMetadata)new UIPropertyMetadata((object)new Size(16.0, 16.0)));
			EditTextProperty = DependencyProperty.Register("EditText", typeof(string), typeof(TreeViewImageItem), (PropertyMetadata)new UIPropertyMetadata((object)string.Empty));
			IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(TreeViewImageItem), (PropertyMetadata)new UIPropertyMetadata((object)true));
			IsEditingProperty = DependencyProperty.Register("IsEditing", typeof(bool), typeof(TreeViewImageItem), (PropertyMetadata)new UIPropertyMetadata((object)false));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewImageItem), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(TreeViewImageItem)));
		}

		protected virtual void OnIconSourceUriChanged(EventArgs eventArgs)
		{
			if (IconSourceUriChanged != null)
			{
				IconSourceUriChanged(this, eventArgs);
			}
			ImageSource = null;
			updateFromIconSourceUri();
		}

		private void updateFromIconSourceUri()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			if (!(IconSourceUri != null))
			{
				return;
			}
			IconBitmapDecoder val = new IconBitmapDecoder(IconSourceUri, (BitmapCreateOptions)1, (BitmapCacheOption)0);
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			int index = 0;
			for (int i = 0; i < ((BitmapDecoder)val).Frames.Count; i++)
			{
				double num3 = ((BitmapSource)((BitmapDecoder)val).Frames[i]).PixelWidth;
				Size imageSize = ImageSize;
				if (num3 == ((Size)(imageSize)).Width)
				{
					double num4 = ((BitmapSource)((BitmapDecoder)val).Frames[i]).PixelHeight;
					Size imageSize2 = ImageSize;
					if (num4 == ((Size)(imageSize2)).Height)
					{
						index = i;
						break;
					}
				}
				if (((BitmapSource)((BitmapDecoder)val).Frames[i]).PixelWidth >= num)
				{
					continue;
				}
				double num5 = ((BitmapSource)((BitmapDecoder)val).Frames[i]).PixelWidth;
				Size imageSize3 = ImageSize;
				if (num5 >= ((Size)(imageSize3)).Width && ((BitmapSource)((BitmapDecoder)val).Frames[i]).PixelHeight < num2)
				{
					double num6 = ((BitmapSource)((BitmapDecoder)val).Frames[i]).PixelHeight;
					Size imageSize4 = ImageSize;
					if (num6 >= ((Size)(imageSize4)).Height)
					{
						index = i;
					}
				}
			}
			ImageSource = (ImageSource)(object)((BitmapDecoder)val).Frames[index];
		}

		protected virtual void OnImageSizeChanged(EventArgs e)
		{
			if (ImageSizeChanged != null)
			{
				ImageSizeChanged(this, e);
			}
			updateFromIconSourceUri();
		}

		protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
		{
			((TreeViewItem)this).OnMouseLeftButtonDown(e);
			((UIElement)this).OnMouseRightButtonDown(e);
		}

		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (IsEditing)
			{
				bool flag = false;
				if (mEditBox != null)
				{
					HitTestResult val = VisualTreeHelper.HitTest((Visual)(object)this, ((MouseEventArgs)e).GetPosition((IInputElement)(object)this));
					for (DependencyObject val2 = val.VisualHit; val2 != null; val2 = VisualTreeHelper.GetParent(val2))
					{
						if ((object)val2 == mEditBox)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					deleteClickTimer();
					IsEditing = false;
				}
			}
			((UIElement)this).OnPreviewMouseLeftButtonDown(e);
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			if (((TreeViewItem)this).IsSelected && IsEditable)
			{
				bool flag = false;
				if (mEditBox != null)
				{
					HitTestResult val = VisualTreeHelper.HitTest((Visual)(object)this, ((MouseEventArgs)e).GetPosition((IInputElement)(object)this));
					for (DependencyObject val2 = val.VisualHit; val2 != null; val2 = VisualTreeHelper.GetParent(val2))
					{
						if ((object)val2 == mEditBox)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					deleteClickTimer();
					if (e.ClickCount == 1)
					{
						mClickedTimer = new DispatcherTimer();
						mClickedTimer.Tick += dt_Tick;
						mClickedTimer.Interval = new TimeSpan(0, 0, 0, 0, 700);
						mClickedTimer.Start();
					}
				}
			}
			((TreeViewItem)this).OnMouseLeftButtonDown(e);
		}

		private void dt_Tick(object sender, EventArgs e)
		{
			if (((TreeViewItem)this).IsSelected)
			{
				IsEditing = true;
			}
			deleteClickTimer();
		}

		private void deleteClickTimer()
		{
			if (mClickedTimer != null)
			{
				mClickedTimer.Tick -= dt_Tick;
				mClickedTimer.Stop();
				mClickedTimer = null;
			}
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			if (((DependencyPropertyChangedEventArgs)(e)).Property == TreeViewItem.IsSelectedProperty)
			{
				if (((DependencyPropertyChangedEventArgs)(e)).NewValue != ((DependencyPropertyChangedEventArgs)(e)).OldValue)
				{
					deleteClickTimer();
				}
				IsEditing = false;
			}
			else if (((DependencyPropertyChangedEventArgs)(e)).Property == IsEditingProperty && ((DependencyPropertyChangedEventArgs)(e)).NewValue != ((DependencyPropertyChangedEventArgs)(e)).OldValue && mEditBox != null)
			{
				if ((bool)((DependencyPropertyChangedEventArgs)(e)).NewValue)
				{
					((TextBoxBase)mEditBox).SelectAll();
					((UIElement)mEditBox).Visibility = (Visibility)0;
					if (((UIElement)this).IsKeyboardFocused)
					{
						((UIElement)mEditBox).Focus();
					}
				}
				else
				{
					((UIElement)mEditBox).Visibility = (Visibility)1;
				}
			}
			((FrameworkElement)this).OnPropertyChanged(e);
		}

		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			if (mEditBox != null)
			{
				((UIElement)mEditBox).LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(mEditBox_LostKeyboardFocus);
				((UIElement)mEditBox).PreviewKeyDown -= new KeyEventHandler(mEditBox_PreviewKeyDown);
				mEditBox = null;
			}
			((Control)this).OnTemplateChanged(oldTemplate, newTemplate);
		}

		public override void OnApplyTemplate()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			ControlTemplate template = ((Control)this).Template;
			if (template != null)
			{
				object obj = ((FrameworkTemplate)template).FindName("PART_EditBox", (FrameworkElement)(object)this);
				mEditBox = (TextBox)((obj is TextBox) ? obj : null);
				((UIElement)mEditBox).LostKeyboardFocus += new KeyboardFocusChangedEventHandler(mEditBox_LostKeyboardFocus);
				((UIElement)mEditBox).PreviewKeyDown += new KeyEventHandler(mEditBox_PreviewKeyDown);
				if (IsEditing)
				{
					((TextBoxBase)mEditBox).SelectAll();
					((UIElement)mEditBox).Visibility = (Visibility)0;
					((UIElement)mEditBox).Focus();
				}
			}
			((FrameworkElement)this).OnApplyTemplate();
		}

		private void mEditBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)e.Key == 6)
			{
				((RoutedEventArgs)e).Handled = true;
				IsEditing = false;
			}
		}

		private void mEditBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			IInputElement newFocus = e.NewFocus;
			DependencyObject val = (DependencyObject)(object)((newFocus is DependencyObject) ? newFocus : null);
			if (val == null || !S9BEUtil.IsPopupDescendant((DependencyObject)(object)mEditBox, val))
			{
				IsEditing = false;
			}
		}
	}
}
