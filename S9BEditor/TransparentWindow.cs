using System;

namespace S9BEditor
{
	public class TransparentWindow : NativeWindow
	{
		protected override IntPtr WndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam)
		{
			if (message == 134 && wParam.ToInt32() == 1)
			{
				return IntPtr.Zero;
			}
			return base.WndProc(hWnd, message, wParam, lParam);
		}
	}
}
