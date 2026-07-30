using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace S9BEditor
{
	internal class OverflowTabControl : TabControl
	{
		private OverflowPanel mOverflowPanel;

		private bool mMovingSelToFirst;

		public static readonly DependencyProperty TabMenuItemsContainerStyleProperty;

		public static readonly ICommand SelectTabCommand;

		public static readonly DependencyProperty HasOverflowItemsProperty;

		public static readonly DependencyProperty OverflowAppendModeProperty;

		public Style TabMenuItemsContainerStyle
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (Style)((DependencyObject)this).GetValue(TabMenuItemsContainerStyleProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(TabMenuItemsContainerStyleProperty, (object)value);
			}
		}

		public bool HasOverflowItems
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(HasOverflowItemsProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(HasOverflowItemsProperty, (object)value);
			}
		}

		public OverflowAppendMode OverflowAppendMode
		{
			get
			{
				return (OverflowAppendMode)((DependencyObject)this).GetValue(OverflowAppendModeProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(OverflowAppendModeProperty, (object)value);
			}
		}

		static OverflowTabControl()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			TabMenuItemsContainerStyleProperty = DependencyProperty.Register("TabMenuItemsContainerStyle", typeof(Style), typeof(OverflowTabControl), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			SelectTabCommand = (ICommand)new RoutedUICommand("Select Tab", "SelectTab", typeof(OverflowTabControl));
			HasOverflowItemsProperty = DependencyProperty.Register("HasOverflowItems", typeof(bool), typeof(OverflowTabControl), (PropertyMetadata)new UIPropertyMetadata((object)false));
			OverflowAppendModeProperty = DependencyProperty.Register("OverflowAppendMode", typeof(OverflowAppendMode), typeof(OverflowTabControl), (PropertyMetadata)new UIPropertyMetadata((object)OverflowAppendMode.AppendBack));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(OverflowTabControl), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(OverflowTabControl)));
		}

		public OverflowTabControl()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			((TabControl)this)._002Ector();
			((UIElement)this).CommandBindings.Add(new CommandBinding(SelectTabCommand, new ExecutedRoutedEventHandler(tabMenuItemSelected)));
		}

		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			if (mOverflowPanel != null)
			{
				mOverflowPanel.OverflowStatesChanged -= overflowStatesChanged;
			}
			mOverflowPanel = null;
			((Control)this).OnTemplateChanged(oldTemplate, newTemplate);
		}

		public override void OnApplyTemplate()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			if (((Control)this).Template != null)
			{
				mOverflowPanel = ((FrameworkTemplate)((Control)this).Template).FindName("PART_TabItemsOverflowPanel", (FrameworkElement)(object)this) as OverflowPanel;
				if (mOverflowPanel != null)
				{
					mOverflowPanel.OverflowStatesChanged += overflowStatesChanged;
					Binding val = new Binding("HasOverflowItems");
					val.Source = mOverflowPanel;
					((FrameworkElement)this).SetBinding(HasOverflowItemsProperty, (BindingBase)(object)val);
				}
			}
			((TabControl)this).OnApplyTemplate();
		}

		private void overflowStatesChanged(object sender, OverflowStatesChangedEventArgs e)
		{
			if (((Selector)this).SelectedItem == null)
			{
				return;
			}
			DependencyObject val = ((ItemsControl)this).ItemContainerGenerator.ContainerFromItem(((Selector)this).SelectedItem);
			foreach (object changedItem in e.ChangedItems)
			{
				if (changedItem == val)
				{
					ensureSelectedVisible();
					break;
				}
			}
		}

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			ensureSelectedVisible();
			((TabControl)this).OnSelectionChanged(e);
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			ensureSelectedVisible();
			((TabControl)this).OnItemsChanged(e);
		}

		private void ensureSelectedVisible()
		{
			if (!mMovingSelToFirst && ((Selector)this).SelectedItem != null)
			{
				object selectedItem = ((Selector)this).SelectedItem;
				DependencyObject obj = ((ItemsControl)this).ItemContainerGenerator.ContainerFromItem(selectedItem);
				TabItem val = (TabItem)(object)((obj is TabItem) ? obj : null);
				if (val != null && OverflowPanel.GetIsOnOverflow((DependencyObject)(object)val) && val.IsSelected)
				{
					mMovingSelToFirst = true;
					mOverflowPanel.BringItemToFront((UIElement)(object)val);
					mMovingSelToFirst = false;
				}
			}
		}

		private void tabMenuItemSelected(object sender, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter != null && ((CollectionView)((ItemsControl)this).Items).Contains(e.Parameter))
			{
				((Selector)this).SelectedItem = e.Parameter;
			}
		}
	}
}
