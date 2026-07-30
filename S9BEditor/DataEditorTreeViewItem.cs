using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace S9BEditor
{
	internal class DataEditorTreeViewItem : TreeViewItem
	{
		private bool mSuppressNextBringIntoView;

		public static readonly DependencyProperty FooterProperty;

		public static readonly DependencyProperty FooterTemplateProperty;

		public object Footer
		{
			get
			{
				return ((DependencyObject)this).GetValue(FooterProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(FooterProperty, value);
			}
		}

		public DataTemplate FooterTemplate
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (DataTemplate)((DependencyObject)this).GetValue(FooterTemplateProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(FooterTemplateProperty, (object)value);
			}
		}

		public DataEditorTreeViewItem()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			((TreeViewItem)this)._002Ector();
			((FrameworkElement)this).RequestBringIntoView += new RequestBringIntoViewEventHandler(DataEditorTreeViewItem_RequestBringIntoView);
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			mSuppressNextBringIntoView = true;
			((TreeViewItem)this).OnGotFocus(e);
		}

		private void DataEditorTreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			if (mSuppressNextBringIntoView)
			{
				((RoutedEventArgs)e).Handled = true;
			}
			mSuppressNextBringIntoView = false;
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return (DependencyObject)(object)new DataEditorTreeViewItem();
		}

		private static TreeViewItem findFirstFocusedTreeViewItem(UIElement start)
		{
			int childrenCount = VisualTreeHelper.GetChildrenCount((DependencyObject)(object)start);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild((DependencyObject)(object)start, i);
				UIElement val = (UIElement)(object)((child is UIElement) ? child : null);
				if (val != null && val.IsKeyboardFocusWithin)
				{
					TreeViewItem val2 = (TreeViewItem)(object)((val is TreeViewItem) ? val : null);
					if (val2 != null)
					{
						return val2;
					}
					val2 = findFirstFocusedTreeViewItem(val);
					if (val2 != null)
					{
						return val2;
					}
				}
			}
			if (start is DataEditorTreeViewItem result)
			{
				return (TreeViewItem)(object)result;
			}
			return null;
		}

		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((UIElement)this).OnIsKeyboardFocusWithinChanged(e);
			if (((UIElement)this).IsKeyboardFocusWithin && (object)findFirstFocusedTreeViewItem((UIElement)(object)this) == this && ((UIElement)this).Focusable)
			{
				((TreeViewItem)this).IsSelected = true;
			}
		}

		protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
		{
			object originalSource = ((RoutedEventArgs)e).OriginalSource;
			UIElement val = (UIElement)((originalSource is UIElement) ? originalSource : null);
			if (val != null && ((Visual)val).IsDescendantOf((DependencyObject)(object)this) && ((UIElement)this).Focusable)
			{
				((TreeViewItem)this).IsSelected = true;
				((UIElement)this).Focus();
			}
			((UIElement)this).OnPreviewMouseRightButtonDown(e);
		}

		protected override void OnSelected(RoutedEventArgs e)
		{
			if (((UIElement)this).IsKeyboardFocusWithin)
			{
				DependencyObject templateChild = ((FrameworkElement)this).GetTemplateChild("PART_ValueContentPresenter");
				FrameworkElement val = (FrameworkElement)(object)((templateChild is FrameworkElement) ? templateChild : null);
				if (val != null)
				{
					((UIElement)val).Focus();
				}
				else if (((UIElement)this).Focusable)
				{
					((UIElement)this).Focus();
				}
			}
			((TreeViewItem)this).OnSelected(e);
		}

		static DataEditorTreeViewItem()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			FooterProperty = DependencyProperty.Register("Footer", typeof(object), typeof(DataEditorTreeViewItem), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(DataEditorTreeViewItem), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		}
	}
}
