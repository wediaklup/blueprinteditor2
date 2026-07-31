using System;
using System.Drawing;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace S9BEditor
{
	public class NativeWindow : Window, IWin32Window
	{
		private IntPtr mOldWndProc;

		private Native.WndProc mNewWndProc;

		public static readonly DependencyProperty CanMaximizeProperty;

		public static readonly DependencyProperty CanMinimizeProperty;

		public static readonly DependencyProperty HasIconProperty;

		public IntPtr Handle { get; private set; }

		public bool CanMaximize
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(CanMaximizeProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(CanMaximizeProperty, (object)value);
				resizeModeChanged();
			}
		}

		public bool CanMinimize
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(CanMinimizeProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(CanMinimizeProperty, (object)value);
				resizeModeChanged();
			}
		}

		public bool HasIcon
		{
			get
			{
				return (bool)((DependencyObject)this).GetValue(HasIconProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(HasIconProperty, (object)value);
			}
		}

		private Rectangle rectVisualToNative(Rect rect, HwndSource hwndSource)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Matrix transformToDevice = ((CompositionTarget)hwndSource.CompositionTarget).TransformToDevice;
			return new Rectangle((int)(((Rect)(rect)).X / ((Matrix)(transformToDevice)).M11), (int)(((Rect)(rect)).Y / ((Matrix)(transformToDevice)).M22), (int)(((Rect)(rect)).Width / ((Matrix)(transformToDevice)).M11), (int)(((Rect)(rect)).Height / ((Matrix)(transformToDevice)).M22));
		}

		protected Rectangle RectVisualToNative(Rect rect)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_0020: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			PresentationSource obj = PresentationSource.FromVisual((Visual)(object)this);
			HwndSource val = (HwndSource)(object)((obj is HwndSource) ? obj : null);
			if (val == null)
			{
				HwndSource val2 = new HwndSource(default(HwndSourceParameters));
				val = val2;
				HwndSource val3 = val2;
				try
				{
					return rectVisualToNative(rect, val);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			return rectVisualToNative(rect, val);
		}

		private Rect rectNativeToVisual(Rectangle rect, HwndSource hwndSource)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			Matrix transformToDevice = ((CompositionTarget)hwndSource.CompositionTarget).TransformToDevice;
			return new Rect((double)rect.X * ((Matrix)(transformToDevice)).M11, (double)rect.Y * ((Matrix)(transformToDevice)).M22, (double)rect.Width * ((Matrix)(transformToDevice)).M11, (double)rect.Height * ((Matrix)(transformToDevice)).M22);
		}

		protected Rect RectNativeToVisual(Rectangle rect)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_0020: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			PresentationSource obj = PresentationSource.FromVisual((Visual)(object)this);
			HwndSource val = (HwndSource)(object)((obj is HwndSource) ? obj : null);
			if (val == null)
			{
				HwndSource val2 = new HwndSource(default(HwndSourceParameters));
				val = val2;
				HwndSource val3 = val2;
				try
				{
					return rectNativeToVisual(rect, val);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			return rectNativeToVisual(rect, val);
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			mNewWndProc = customWndProc;
			Handle = new WindowInteropHelper((Window)(object)this).Handle;
			mOldWndProc = Native.GetWindowLong(Handle, -4);
			Native.SetWindowLong(Handle, -4, mNewWndProc);
			((Window)this).OnSourceInitialized(e);
			resizeModeChanged();
		}

		private IntPtr customWndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam)
		{
			return WndProc(hWnd, message, wParam, lParam);
		}

		protected virtual IntPtr WndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam)
		{
			if (message == 2)
			{
				Native.SetWindowLong(hWnd, -4, mOldWndProc);
			}
			return Native.CallWindowProc(mOldWndProc, hWnd, message, wParam, lParam);
		}

		public void Maximize()
		{
			Native.PostMessage(Handle, 274u, new IntPtr(61488L), IntPtr.Zero);
		}

		public void Minimize()
		{
			Native.PostMessage(Handle, 274u, new IntPtr(61472L), IntPtr.Zero);
		}

		public void Restore()
		{
			Native.PostMessage(Handle, 274u, new IntPtr(61728L), IntPtr.Zero);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((FrameworkElement)this).OnPropertyChanged(e);
			if (((DependencyPropertyChangedEventArgs)(e)).Property == Window.ResizeModeProperty)
			{
				resizeModeChanged();
			}
		}

		private void resizeModeChanged()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Invalid comparison between Unknown and I4
			if (Handle != IntPtr.Zero)
			{
				bool flag = (int)((Window)this).ResizeMode != 0 && CanMinimize;
				bool flag2 = (int)((Window)this).ResizeMode != 0 && (int)((Window)this).ResizeMode != 1 && CanMaximize;
				int num = Native.GetWindowLong(Handle, -16).ToInt32();
				int num2 = num;
				int num3 = Native.GetWindowLong(Handle, -20).ToInt32();
				int num4 = num3;
				num3 = ((!HasIcon) ? (num3 | 1) : (num3 & -2));
				num = ((!flag) ? (num & -131073) : (num | 0x20000));
				num = ((!flag2) ? (num & -65537) : (num | 0x10000));
				bool flag3 = false;
				if (num != num2)
				{
					Native.SetWindowLong(Handle, -16, num);
					flag3 = true;
				}
				if (num3 != num4)
				{
					Native.SetWindowLong(Handle, -20, num3);
					flag3 = true;
				}
				if (flag3)
				{
					Native.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, 39u);
				}
				if (flag != CanMinimize)
				{
					((DependencyObject)this).SetValue(CanMinimizeProperty, (object)flag);
				}
				if (flag2 != CanMaximize)
				{
					((DependencyObject)this).SetValue(CanMaximizeProperty, (object)flag2);
				}
			}
		}

		static NativeWindow()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			CanMaximizeProperty = DependencyProperty.Register("CanMaximize", typeof(bool), typeof(NativeWindow), (PropertyMetadata)new UIPropertyMetadata((object)true));
			CanMinimizeProperty = DependencyProperty.Register("CanMinimize", typeof(bool), typeof(NativeWindow), (PropertyMetadata)new UIPropertyMetadata((object)true));
			HasIconProperty = DependencyProperty.Register("HasIcon", typeof(bool), typeof(NativeWindow), (PropertyMetadata)new UIPropertyMetadata((object)true));
		}
	}
}
