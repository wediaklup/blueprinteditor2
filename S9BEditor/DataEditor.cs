using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace S9BEditor
{
	public class DataEditor : ItemsControl
	{
		private ScrollViewer mMainScrollView;

		public static readonly DependencyProperty GridLineBrushProperty;

		public static readonly DependencyProperty NameColumnBrushProperty;

		public static readonly DependencyProperty ValueColumnBrushProperty;

		public static readonly DependencyProperty TypeColumnBrushProperty;

		public static readonly DependencyProperty TypeColumnVisibleProperty;

		public static readonly DependencyProperty IndentsChildrenProperty;

		public static readonly DependencyProperty ContainerHeaderBackgroundProperty;

		public static readonly DependencyProperty ContainerHeaderForegroundProperty;

		public static readonly DependencyProperty ContainerHeaderForegroundSelectedProperty;

		public static readonly DependencyProperty BringIntoViewProperty;

		public static readonly DependencyProperty ActualNameColumnWidthProperty;

		public static readonly DependencyProperty ActualValueColumnWidthProperty;

		public static readonly DependencyProperty ActualTypeColumnWidthProperty;

		public static readonly DependencyProperty ActualNameColumnSeparatorWidthProperty;

		public static readonly DependencyProperty ActualValueColumnSeparatorWidthProperty;

		public static readonly DependencyProperty ActualTypeColumnSeparatorWidthProperty;

		public static readonly DependencyProperty NameColumnWidthProperty;

		public static readonly DependencyProperty NameColumnSeparatorWidthProperty;

		public static readonly DependencyProperty TypeColumnWidthProperty;

		public static readonly DependencyProperty TypeColumnSeparatorWidthProperty;

		public static readonly DependencyProperty ValueColumnWidthProperty;

		public static readonly DependencyProperty ValueColumnSeparatorWidthProperty;

		public Brush GridLineBrush
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (Brush)((DependencyObject)this).GetValue(GridLineBrushProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(GridLineBrushProperty, (object)value);
			}
		}

		public Brush NameColumnBrush
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (Brush)((DependencyObject)this).GetValue(NameColumnBrushProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(NameColumnBrushProperty, (object)value);
			}
		}

		public Brush ValueColumnBrush
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (Brush)((DependencyObject)this).GetValue(ValueColumnBrushProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ValueColumnBrushProperty, (object)value);
			}
		}

		public Brush TypeColumnBrush
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (Brush)((DependencyObject)this).GetValue(TypeColumnBrushProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(TypeColumnBrushProperty, (object)value);
			}
		}

		public bool TypeColumnVisible
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(TypeColumnVisibleProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(TypeColumnVisibleProperty, (object)value);
			}
		}

		public object SelectedValue
		{
			get
			{
				if (((Control)this).Template != null)
				{
					object obj = ((FrameworkTemplate)((Control)this).Template).FindName("PART_treeView", (FrameworkElement)(object)this);
					TreeView val = (TreeView)((obj is TreeView) ? obj : null);
					if (val != null)
					{
						return val.SelectedValue;
					}
				}
				return null;
			}
		}

		public double ActualNameColumnWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(ActualNameColumnWidthProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(ActualNameColumnWidthProperty, (object)value);
			}
		}

		public double ActualValueColumnWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(ActualValueColumnWidthProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(ActualValueColumnWidthProperty, (object)value);
			}
		}

		public double ActualTypeColumnWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(ActualTypeColumnWidthProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(ActualTypeColumnWidthProperty, (object)value);
			}
		}

		public double ActualNameColumnSeparatorWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(ActualNameColumnSeparatorWidthProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(ActualNameColumnSeparatorWidthProperty, (object)value);
			}
		}

		public double ActualValueColumnSeparatorWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(ActualValueColumnSeparatorWidthProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(ActualValueColumnSeparatorWidthProperty, (object)value);
			}
		}

		public double ActualTypeColumnSeparatorWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(ActualTypeColumnSeparatorWidthProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(ActualTypeColumnSeparatorWidthProperty, (object)value);
			}
		}

		public double NameColumnWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(NameColumnWidthProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(NameColumnWidthProperty, (object)value);
			}
		}

		public double NameColumnSeparatorWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(NameColumnSeparatorWidthProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(NameColumnSeparatorWidthProperty, (object)value);
			}
		}

		public double TypeColumnWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(TypeColumnWidthProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(TypeColumnWidthProperty, (object)value);
			}
		}

		public double TypeColumnSeparatorWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(TypeColumnSeparatorWidthProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(TypeColumnSeparatorWidthProperty, (object)value);
			}
		}

		public double ValueColumnWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(ValueColumnWidthProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ValueColumnWidthProperty, (object)value);
			}
		}

		public double ValueColumnSeparatorWidth
		{
			get
			{
				return (double)((DependencyObject)this).GetValue(ValueColumnSeparatorWidthProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ValueColumnSeparatorWidthProperty, (object)value);
			}
		}

		static DataEditor()
		{
			PLogger.Write("static DataEditor()");
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Expected O, but got Unknown
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Expected O, but got Unknown
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Expected O, but got Unknown
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Expected O, but got Unknown
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Expected O, but got Unknown
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Expected O, but got Unknown
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Expected O, but got Unknown
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Expected O, but got Unknown
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Expected O, but got Unknown
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Expected O, but got Unknown
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Expected O, but got Unknown
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Expected O, but got Unknown
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Expected O, but got Unknown
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Expected O, but got Unknown
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Expected O, but got Unknown
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Expected O, but got Unknown
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Expected O, but got Unknown
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Expected O, but got Unknown
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Expected O, but got Unknown
			//IL_043f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0449: Expected O, but got Unknown
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Expected O, but got Unknown
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Expected O, but got Unknown
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Expected O, but got Unknown
			//IL_0509: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Expected O, but got Unknown
			GridLineBrushProperty = DependencyProperty.Register("GridLineBrush", typeof(Brush), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)204, (byte)204, (byte)204))));
			NameColumnBrushProperty = DependencyProperty.Register("NameColumnBrush", typeof(Brush), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)238, (byte)238, (byte)238))));
			ValueColumnBrushProperty = DependencyProperty.Register("ValueColumnBrush", typeof(Brush), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue))));
			TypeColumnBrushProperty = DependencyProperty.Register("TypeColumnBrush", typeof(Brush), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)238, (byte)238, (byte)238))));
			TypeColumnVisibleProperty = DependencyProperty.Register("TypeColumnVisible", typeof(bool), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)true));
			IndentsChildrenProperty = DependencyProperty.RegisterAttached("IndentsChildren", typeof(bool), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)false));
			ContainerHeaderBackgroundProperty = DependencyProperty.RegisterAttached("ContainerHeaderBackground", typeof(Brush), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)187, (byte)187, (byte)187))));
			ContainerHeaderForegroundProperty = DependencyProperty.RegisterAttached("ContainerHeaderForeground", typeof(Brush), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)0, (byte)0, (byte)0))));
			ContainerHeaderForegroundSelectedProperty = DependencyProperty.RegisterAttached("ContainerHeaderForegroundSelected", typeof(Brush), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte)0, (byte)0, (byte)0))));
			BringIntoViewProperty = DependencyProperty.RegisterAttached("BringIntoView", typeof(bool), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)false, new PropertyChangedCallback(onBringIntoView)));
			ActualNameColumnWidthProperty = DependencyProperty.Register("ActualNameColumnWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)200.0));
			ActualValueColumnWidthProperty = DependencyProperty.Register("ActualValueColumnWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)300.0));
			ActualTypeColumnWidthProperty = DependencyProperty.Register("ActualTypeColumnWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)200.0));
			ActualNameColumnSeparatorWidthProperty = DependencyProperty.Register("ActualNameColumnSeparatorWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)1.0));
			ActualValueColumnSeparatorWidthProperty = DependencyProperty.Register("ActualValueColumnSeparatorWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)1.0));
			ActualTypeColumnSeparatorWidthProperty = DependencyProperty.Register("ActualTypeColumnSeparatorWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)1.0));
			NameColumnWidthProperty = DependencyProperty.Register("NameColumnWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)200.0));
			NameColumnSeparatorWidthProperty = DependencyProperty.Register("NameColumnSeparatorWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)1.0));
			TypeColumnWidthProperty = DependencyProperty.Register("TypeColumnWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)200.0));
			TypeColumnSeparatorWidthProperty = DependencyProperty.Register("TypeColumnSeparatorWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)1.0));
			ValueColumnWidthProperty = DependencyProperty.Register("ValueColumnWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)300.0));
			ValueColumnSeparatorWidthProperty = DependencyProperty.Register("ValueColumnSeparatorWidth", typeof(double), typeof(DataEditor), (PropertyMetadata)new UIPropertyMetadata((object)1.0));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataEditor), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(DataEditor)));
		}

		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			if (oldTemplate != null && mMainScrollView != null)
			{
				mMainScrollView.ScrollChanged -= new ScrollChangedEventHandler(scrollViewer_ScrollChanged);
				mMainScrollView = null;
			}
			base.OnTemplateChanged(oldTemplate, newTemplate);
		}

		public override void OnApplyTemplate()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			base.OnApplyTemplate();
			if (base.Template != null)
			{
				object obj = ((FrameworkTemplate)((Control)this).Template).FindName("PART_scrollViewer", (FrameworkElement)(object)this);
				mMainScrollView = (ScrollViewer)((obj is ScrollViewer) ? obj : null);
				if (mMainScrollView != null)
				{
					mMainScrollView.ScrollChanged += new ScrollChangedEventHandler(scrollViewer_ScrollChanged);
				}
			}
		}

		private void scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (((Control)this).Template != null)
			{
				object obj = ((FrameworkTemplate)((Control)this).Template).FindName("PART_columnHeadersScrollViewer", (FrameworkElement)(object)this);
				ScrollViewer val = (ScrollViewer)((obj is ScrollViewer) ? obj : null);
				if (mMainScrollView != null)
				{
					val.ScrollToHorizontalOffset(mMainScrollView.HorizontalOffset);
				}
			}
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			if (((DependencyPropertyChangedEventArgs)(e)).Property == TypeColumnVisibleProperty)
			{
				double num = (TypeColumnVisible ? TypeColumnWidth : 0.0);
				if (num != ActualTypeColumnWidth)
				{
					ActualTypeColumnWidth = num;
				}
				num = (TypeColumnVisible ? TypeColumnSeparatorWidth : 0.0);
				if (num != ActualTypeColumnSeparatorWidth)
				{
					ActualTypeColumnSeparatorWidth = num;
				}
			}
			else if (((DependencyPropertyChangedEventArgs)(e)).Property == TypeColumnWidthProperty)
			{
				double num = (TypeColumnVisible ? TypeColumnWidth : 0.0);
				if (num != ActualTypeColumnWidth)
				{
					ActualTypeColumnWidth = num;
				}
			}
			else if (((DependencyPropertyChangedEventArgs)(e)).Property == TypeColumnSeparatorWidthProperty)
			{
				double num = (TypeColumnVisible ? TypeColumnSeparatorWidth : 0.0);
				if (num != ActualTypeColumnSeparatorWidth)
				{
					ActualTypeColumnSeparatorWidth = num;
				}
			}
			else if (((DependencyPropertyChangedEventArgs)(e)).Property == NameColumnWidthProperty)
			{
				double num = NameColumnWidth;
				if (num != ActualNameColumnWidth)
				{
					ActualNameColumnWidth = num;
				}
			}
			else if (((DependencyPropertyChangedEventArgs)(e)).Property == NameColumnSeparatorWidthProperty)
			{
				double num = NameColumnSeparatorWidth;
				if (num != ActualNameColumnSeparatorWidth)
				{
					ActualNameColumnSeparatorWidth = num;
				}
			}
			else if (((DependencyPropertyChangedEventArgs)(e)).Property == ValueColumnWidthProperty)
			{
				double num = ValueColumnWidth;
				if (num != ActualValueColumnWidth)
				{
					ActualValueColumnWidth = num;
				}
			}
			else if (((DependencyPropertyChangedEventArgs)(e)).Property == ValueColumnSeparatorWidthProperty)
			{
				double num = ValueColumnSeparatorWidth;
				if (num != ActualValueColumnSeparatorWidth)
				{
					ActualValueColumnSeparatorWidth = num;
				}
			}
			base.OnPropertyChanged(e);
		}

		public static bool GetIndentsChildren(DependencyObject obj)
		{
			PLogger.Write("DataEditor.GetIndentsChildren");
			return (bool)obj.GetValue(IndentsChildrenProperty);
		}

		public static void SetIndentsChildren(DependencyObject obj, bool value)
		{
			PLogger.Write("DataEditor.SetIndentsChildren");
			obj.SetValue(IndentsChildrenProperty, (object)value);
		}

		public static Brush GetContainerHeaderBackground(DependencyObject obj)
		{
			PLogger.Write("DataEditor.GetContainerHeaderBackground");
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			return (Brush)obj.GetValue(ContainerHeaderBackgroundProperty);
		}

		public static void SetContainerHeaderBackground(DependencyObject obj, Brush value)
		{
			PLogger.Write("DataEditor.SetContainerHeaderBackground");
			obj.SetValue(ContainerHeaderBackgroundProperty, (object)value);
		}

		public static Brush GetContainerHeaderForeground(DependencyObject obj)
		{
			PLogger.Write("DataEditor.GetContainerHeaderForeground");
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			return (Brush)obj.GetValue(ContainerHeaderForegroundProperty);
		}

		public static void SetContainerHeaderForeground(DependencyObject obj, Brush value)
		{
			PLogger.Write("DataEditor.SetContainerHeaderForeground");
			obj.SetValue(ContainerHeaderForegroundProperty, (object)value);
		}

		public static Brush GetContainerHeaderForegroundSelected(DependencyObject obj)
		{
			PLogger.Write("DataEditor.GetContainerHeaderForegroundSelected");
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			return (Brush)obj.GetValue(ContainerHeaderForegroundSelectedProperty);
		}

		public static void SetContainerHeaderForegroundSelected(DependencyObject obj, Brush value)
		{
			PLogger.Write("DataEditor.SetContainerHeaderForegroundSelected");
			obj.SetValue(ContainerHeaderForegroundSelectedProperty, (object)value);
		}

		public static bool GetBringIntoView(DependencyObject obj)
		{
			PLogger.Write("DataEditor.GetBringIntoView");
			return (bool)obj.GetValue(BringIntoViewProperty);
		}

		public static void SetBringIntoView(DependencyObject obj, bool value)
		{
			PLogger.Write("DataEditor.SetBringIntoView");
			obj.SetValue(BringIntoViewProperty, (object)value);
		}

		private static void onBringIntoView(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PLogger.Write("DataEditor.onBringIntoView");
			if (!(bool)((DependencyPropertyChangedEventArgs)(e)).NewValue)
			{
				return;
			}
			FrameworkElement val = (FrameworkElement)(object)((o is FrameworkElement) ? o : null);
			if (val != null)
			{
				TreeViewItem val2 = (TreeViewItem)(object)((val is TreeViewItem) ? val : null);
				if (val2 != null)
				{
					val2.IsSelected = true;
				}
				DependencyObject val3 = (DependencyObject)(object)val;
				while (!(val3 is TreeView))
				{
					val3 = VisualTreeHelper.GetParent(val3);
					val2 = (TreeViewItem)(object)((val3 is TreeViewItem) ? val3 : null);
					if (val2 != null)
					{
						val2.IsExpanded = true;
					}
				}
				val.BringIntoView();
			}
			o.SetValue(BringIntoViewProperty, (object)false);
		}
	}
}
