using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;

namespace S9BEditor;

public class CustomWindow : NativeWindow
{
	private ButtonBase mMinimizeButton;

	private ButtonBase mMaximizeButton;

	private ButtonBase mCloseButton;

	private FrameworkElement mClientAreaGrid;

	private TransparentWindow mBorderWin;

	public static readonly DependencyProperty BorderWidthProperty;

	public static readonly DependencyProperty BorderHeightProperty;

	public static readonly DependencyProperty TitleBarHeightProperty;

	public static readonly DependencyProperty ActualBorderWidthProperty;

	public static readonly DependencyProperty ActualTitleBarHeightProperty;

	public static readonly DependencyProperty ActualBorderHeightProperty;

	public static readonly DependencyProperty IsNonClientElementProperty;

	public static readonly DependencyProperty CaptionMarginProperty;

	public static readonly DependencyProperty NonClientContentProperty;

	public static readonly DependencyProperty NonClientContentTemplateProperty;

	public static readonly DependencyProperty HasDialogBackgroundProperty;

	public double? BorderWidth
	{
		get
		{
			return (double?)((DependencyObject)this).GetValue(BorderWidthProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(BorderWidthProperty, (object)value);
			updateActualBorderWidth();
		}
	}

	public double? BorderHeight
	{
		get
		{
			return (double?)((DependencyObject)this).GetValue(BorderHeightProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(BorderHeightProperty, (object)value);
			updateActualBorderHeight();
		}
	}

	public double? TitleBarHeight
	{
		get
		{
			return (double?)((DependencyObject)this).GetValue(TitleBarHeightProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(TitleBarHeightProperty, (object)value);
			updateActualTitleBarHeight();
		}
	}

	public double ActualBorderWidth
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(ActualBorderWidthProperty);
		}
		private set
		{
			((DependencyObject)this).SetValue(ActualBorderWidthProperty, (object)value);
		}
	}

	public double ActualTitleBarHeight
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(ActualTitleBarHeightProperty);
		}
		private set
		{
			((DependencyObject)this).SetValue(ActualTitleBarHeightProperty, (object)value);
		}
	}

	public double ActualBorderHeight
	{
		get
		{
			return (double)((DependencyObject)this).GetValue(ActualBorderHeightProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ActualBorderHeightProperty, (object)value);
		}
	}

	public Thickness CaptionMargin
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return (Thickness)((DependencyObject)this).GetValue(CaptionMarginProperty);
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			((DependencyObject)this).SetValue(CaptionMarginProperty, (object)value);
		}
	}

	public object NonClientContent
	{
		get
		{
			return ((DependencyObject)this).GetValue(NonClientContentProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(NonClientContentProperty, value);
		}
	}

	public DataTemplate NonClientContentTemplate
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			return (DataTemplate)((DependencyObject)this).GetValue(NonClientContentTemplateProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(NonClientContentTemplateProperty, (object)value);
		}
	}

	public bool HasDialogBackground
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(HasDialogBackgroundProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(HasDialogBackgroundProperty, (object)value);
		}
	}

	static CustomWindow()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Expected O, but got Unknown
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		BorderWidthProperty = DependencyProperty.Register("BorderWidth", typeof(double?), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		BorderHeightProperty = DependencyProperty.Register("BorderHeight", typeof(double?), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		TitleBarHeightProperty = DependencyProperty.Register("TitleBarHeight", typeof(double?), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		ActualBorderWidthProperty = DependencyProperty.Register("ActualBorderWidth", typeof(double), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((object)0.0));
		ActualTitleBarHeightProperty = DependencyProperty.Register("ActualTitleBarHeight", typeof(double), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((object)0.0));
		ActualBorderHeightProperty = DependencyProperty.Register("ActualBorderHeight", typeof(double), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((object)0.0));
		IsNonClientElementProperty = DependencyProperty.RegisterAttached("IsNonClientElement", typeof(bool), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((object)false));
		CaptionMarginProperty = DependencyProperty.Register("CaptionMargin", typeof(Thickness), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((object)new Thickness(0.0, 0.0, 0.0, 0.0)));
		NonClientContentProperty = DependencyProperty.Register("NonClientContent", typeof(object), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		NonClientContentTemplateProperty = DependencyProperty.Register("NonClientContentTemplate", typeof(DataTemplate), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		HasDialogBackgroundProperty = DependencyProperty.Register("HasDialogBackground", typeof(bool), typeof(CustomWindow), (PropertyMetadata)new UIPropertyMetadata((object)false));
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(CustomWindow)));
	}

	public CustomWindow()
	{
		((FrameworkElement)this).Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.IetfLanguageTag);
	}

	protected override void OnSourceInitialized(EventArgs e)
	{
		base.OnSourceInitialized(e);
		updateActualBorderHeight();
		updateActualBorderWidth();
		updateActualTitleBarHeight();
		mBorderWin = null;
	}

	private void mBorderWin_SourceInitialized(object sender, EventArgs e)
	{
		int num = Native.GetWindowLong(mBorderWin.Handle, -20).ToInt32();
		Native.SetWindowLong(mBorderWin.Handle, -20, num | 0x20);
	}

	protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
	{
		((FrameworkElement)this).OnRenderSizeChanged(sizeInfo);
		updateShadowBorder();
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		((Window)this).OnLocationChanged(e);
		updateShadowBorder();
	}

	private void updateShadowBorder()
	{
		double num = (double)((FrameworkElement)this).Style.Resources[(object)"ShadowWidth"];
		if (mBorderWin != null)
		{
			((Window)mBorderWin).Left = ((Window)this).Left - num;
			((Window)mBorderWin).Top = ((Window)this).Top - num;
			((FrameworkElement)mBorderWin).Height = ((FrameworkElement)this).Height + num * 2.0;
			((FrameworkElement)mBorderWin).Width = ((FrameworkElement)this).Width + num * 2.0;
		}
	}

	public override void OnApplyTemplate()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		if (mMinimizeButton != null)
		{
			mMinimizeButton.Click -= new RoutedEventHandler(mMinimizeButton_Click);
		}
		if (mMaximizeButton != null)
		{
			mMaximizeButton.Click -= new RoutedEventHandler(mMaximizeButton_Click);
		}
		if (mCloseButton != null)
		{
			mCloseButton.Click -= new RoutedEventHandler(mCloseButton_Click);
		}
		if (((Control)this).Template != null)
		{
			object obj = ((FrameworkTemplate)((Control)this).Template).FindName("PART_MinimizeButton", (FrameworkElement)(object)this);
			mMinimizeButton = (ButtonBase)((obj is ButtonBase) ? obj : null);
			object obj2 = ((FrameworkTemplate)((Control)this).Template).FindName("PART_MaximizeButton", (FrameworkElement)(object)this);
			mMaximizeButton = (ButtonBase)((obj2 is ButtonBase) ? obj2 : null);
			object obj3 = ((FrameworkTemplate)((Control)this).Template).FindName("PART_CloseButton", (FrameworkElement)(object)this);
			mCloseButton = (ButtonBase)((obj3 is ButtonBase) ? obj3 : null);
			object obj4 = ((FrameworkTemplate)((Control)this).Template).FindName("PART_ClientAreaGrid", (FrameworkElement)(object)this);
			mClientAreaGrid = (FrameworkElement)((obj4 is FrameworkElement) ? obj4 : null);
		}
		if (mMinimizeButton != null)
		{
			mMinimizeButton.Click += new RoutedEventHandler(mMinimizeButton_Click);
		}
		if (mMaximizeButton != null)
		{
			mMaximizeButton.Click += new RoutedEventHandler(mMaximizeButton_Click);
		}
		if (mCloseButton != null)
		{
			mCloseButton.Click += new RoutedEventHandler(mCloseButton_Click);
		}
		((FrameworkElement)this).OnApplyTemplate();
	}

	private void updateActualBorderWidth()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected I4, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		PresentationSource.FromVisual((Visual)(object)this);
		double num;
		if (!BorderWidth.HasValue)
		{
			ResizeMode resizeMode = ((Window)this).ResizeMode;
			switch ((int)resizeMode)
			{
			case 0:
			case 1:
				num = SystemParameters.FixedFrameVerticalBorderWidth;
				break;
			default:
				num = SystemParameters.ResizeFrameVerticalBorderWidth;
				break;
			}
		}
		else
		{
			num = BorderWidth.Value;
		}
		if ((int)((Window)this).WindowState == 2)
		{
			num -= SystemParameters.FixedFrameVerticalBorderWidth;
		}
		num = Math.Max(0.0, num);
		ActualBorderWidth = num;
	}

	private void updateActualBorderHeight()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected I4, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		PresentationSource.FromVisual((Visual)(object)this);
		double num;
		if (!BorderHeight.HasValue)
		{
			ResizeMode resizeMode = ((Window)this).ResizeMode;
			switch ((int)resizeMode)
			{
			case 0:
			case 1:
				num = SystemParameters.FixedFrameHorizontalBorderHeight;
				break;
			default:
				num = SystemParameters.ResizeFrameHorizontalBorderHeight;
				break;
			}
		}
		else
		{
			num = BorderHeight.Value;
		}
		if ((int)((Window)this).WindowState == 2)
		{
			num -= SystemParameters.ResizeFrameHorizontalBorderHeight;
		}
		num = Math.Max(0.0, num);
		ActualBorderHeight = num;
	}

	private void updateActualTitleBarHeight()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected I4, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		PresentationSource.FromVisual((Visual)(object)this);
		double num;
		if (!TitleBarHeight.HasValue)
		{
			ResizeMode resizeMode = ((Window)this).ResizeMode;
			switch ((int)resizeMode)
			{
			case 0:
			case 1:
				num = SystemParameters.CaptionHeight + SystemParameters.FixedFrameHorizontalBorderHeight;
				break;
			default:
				num = SystemParameters.CaptionHeight + SystemParameters.ResizeFrameHorizontalBorderHeight;
				break;
			}
		}
		else
		{
			num = TitleBarHeight.Value;
		}
		if ((int)((Window)this).WindowState == 2)
		{
			num -= SystemParameters.ResizeFrameHorizontalBorderHeight;
		}
		num = Math.Max(0.0, num);
		ActualTitleBarHeight = num;
	}

	private void mCloseButton_Click(object sender, RoutedEventArgs e)
	{
		((Window)this).Close();
	}

	private void mMaximizeButton_Click(object sender, RoutedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Window)this).WindowState == 2)
		{
			Restore();
		}
		else
		{
			Maximize();
		}
	}

	private void mMinimizeButton_Click(object sender, RoutedEventArgs e)
	{
		Minimize();
	}

	private static bool pointInVisual(Point point, FrameworkElement visual)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Point val = ((Visual)visual).PointToScreen(new Point(0.0, 0.0));
		Point val2 = ((Visual)visual).PointToScreen(new Point(visual.ActualWidth, visual.ActualHeight));
		if (((Point)(point)).X >= ((Point)(val)).X && ((Point)(point)).Y >= ((Point)(val)).Y && ((Point)(point)).X < ((Point)(val2)).X)
		{
			return ((Point)(point)).Y < ((Point)(val2)).Y;
		}
		return false;
	}

	protected override void OnStateChanged(EventArgs e)
	{
		((Window)this).OnStateChanged(e);
		updateActualTitleBarHeight();
		updateActualBorderWidth();
		updateActualBorderHeight();
	}

	protected override IntPtr WndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Invalid comparison between Unknown and I4
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Invalid comparison between Unknown and I4
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Invalid comparison between Unknown and I4
		switch (message)
		{
		case 132:
		{
			int val3 = lParam.ToInt32();
			short xLParam = Native.GetXLParam(val3);
			short yLParam = Native.GetYLParam(val3);
			PresentationSource obj = PresentationSource.FromVisual((Visual)(object)this);
			HwndSource val4 = (HwndSource)(object)((obj is HwndSource) ? obj : null);
			Matrix transformToDevice = ((CompositionTarget)val4.CompositionTarget).TransformToDevice;
			double m = ((Matrix)(transformToDevice)).M11;
			double m2 = ((Matrix)(transformToDevice)).M22;
			Point val5 = default(Point);
			((Point)(val5))._002Ector((double)xLParam / m, (double)yLParam / m2);
			Native.GetWindowRect(hWnd, out var rect);
			Rect val6 = default(Rect);
			((Rect)(val6))._002Ector((double)rect.Left / m, (double)rect.Top / m2, (double)(rect.Right - rect.Left) / m, (double)(rect.Bottom - rect.Top) / m2);
			bool nonClientElementFound = false;
			PointHitTestParameters val7 = new PointHitTestParameters((Point)(val5 - ((Rect)(val6)).Location));
			VisualTreeHelper.HitTest((Visual)(object)this, (HitTestFilterCallback)null, (HitTestResultCallback)delegate(HitTestResult hr)
			{
				for (DependencyObject val8 = hr.VisualHit; val8 != null; val8 = VisualTreeHelper.GetParent(val8))
				{
					if (GetIsNonClientElement(val8) || val8 == NonClientContent)
					{
						nonClientElementFound = true;
						return (HitTestResultBehavior)0;
					}
				}
				return (HitTestResultBehavior)1;
			}, (HitTestParameters)(object)val7);
			if (!nonClientElementFound)
			{
				if ((int)((Window)this).WindowState == 2)
				{
					((Rect)(val6)).X = ((Rect)(val6)).X + SystemParameters.ResizeFrameVerticalBorderWidth;
					((Rect)(val6)).Width = ((Rect)(val6)).Width - SystemParameters.ResizeFrameVerticalBorderWidth * 2.0;
					((Rect)(val6)).Y = ((Rect)(val6)).Y + SystemParameters.ResizeFrameHorizontalBorderHeight;
					((Rect)(val6)).Height = ((Rect)(val6)).Height - SystemParameters.ResizeFrameHorizontalBorderHeight * 2.0;
				}
				if (!(((Point)(val5)).X >= ((Rect)(val6)).Left) || !(((Point)(val5)).X < ((Rect)(val6)).Right) || !(((Point)(val5)).Y >= ((Rect)(val6)).Top) || !(((Point)(val5)).Y < ((Rect)(val6)).Bottom))
				{
					break;
				}
				if ((int)((Window)this).ResizeMode == 2 || (int)((Window)this).ResizeMode == 3)
				{
					if (((Point)(val5)).Y < ((Rect)(val6)).Top + ActualBorderHeight)
					{
						if (((Point)(val5)).X >= ((Rect)(val6)).Right - ActualBorderWidth * 2.0)
						{
							return new IntPtr(14);
						}
						if (((Point)(val5)).X < ((Rect)(val6)).Left + ActualBorderWidth * 2.0)
						{
							return new IntPtr(13);
						}
						return new IntPtr(12);
					}
					if (((Point)(val5)).Y >= ((Rect)(val6)).Bottom - ActualBorderHeight)
					{
						if (((Point)(val5)).X >= ((Rect)(val6)).Right - ActualBorderWidth * 2.0)
						{
							return new IntPtr(17);
						}
						if (((Point)(val5)).X < ((Rect)(val6)).Left + ActualBorderWidth * 2.0)
						{
							return new IntPtr(16);
						}
						return new IntPtr(15);
					}
					if (((Point)(val5)).X >= ((Rect)(val6)).Right - ActualBorderWidth)
					{
						return new IntPtr(11);
					}
					if (((Point)(val5)).X < ((Rect)(val6)).Left + ActualBorderWidth)
					{
						return new IntPtr(10);
					}
					if (((Point)(val5)).Y < ((Rect)(val6)).Top + ActualTitleBarHeight)
					{
						return new IntPtr(2);
					}
				}
				else if (((Point)(val5)).Y < ((Rect)(val6)).Top + ActualTitleBarHeight)
				{
					return new IntPtr(2);
				}
				break;
			}
			return new IntPtr(1);
		}
		case 131:
			if (wParam != IntPtr.Zero)
			{
				return new IntPtr(0);
			}
			break;
		case 12:
		case 134:
		{
			IntPtr windowLong = Native.GetWindowLong(hWnd, -16);
			Native.SetWindowLong(hWnd, -16, new IntPtr(windowLong.ToInt64() & -268435457));
			base.WndProc(hWnd, message, wParam, lParam);
			Native.SetWindowLong(hWnd, -16, windowLong);
			return new IntPtr(1);
		}
		case 133:
		case 174:
		case 175:
			return new IntPtr(1);
		case 5:
		{
			Rectangle rectangle = new Rectangle(0, 0, Native.LoWord(lParam.ToInt32()), Native.HiWord(lParam.ToInt32()));
			updateShadowBorder();
			Region val = new Region(rectangle);
			try
			{
				Graphics val2 = Graphics.FromHwnd(hWnd);
				try
				{
					Native.GetWindowPlacement(hWnd, out var lpwndpl);
					if ((long)lpwndpl.showCmd != 3)
					{
						IntPtr hrgn = val.GetHrgn(val2);
						Native.SetWindowRgn(hWnd, hrgn, bRedraw: false);
						if (hrgn != IntPtr.Zero)
						{
							val.ReleaseHrgn(hrgn);
						}
						if (mClientAreaGrid != null)
						{
							mClientAreaGrid.Margin = new Thickness(0.0);
						}
						if (mBorderWin != null)
						{
							((Window)mBorderWin).Show();
						}
					}
					else
					{
						Native.SetWindowRgn(hWnd, IntPtr.Zero, bRedraw: false);
						Size frameBorderSize = SystemInformation.FrameBorderSize;
						if (mClientAreaGrid != null)
						{
							mClientAreaGrid.Margin = new Thickness((double)frameBorderSize.Width, (double)frameBorderSize.Height, (double)frameBorderSize.Width, (double)frameBorderSize.Height);
						}
						if (mBorderWin != null)
						{
							((Window)mBorderWin).Hide();
						}
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			break;
		}
		}
		return base.WndProc(hWnd, message, wParam, lParam);
	}

	public static bool GetIsNonClientElement(DependencyObject obj)
	{
		return (bool)obj.GetValue(IsNonClientElementProperty);
	}

	public static void SetIsNonClientElement(DependencyObject obj, bool value)
	{
		obj.SetValue(IsNonClientElementProperty, (object)value);
	}
}
