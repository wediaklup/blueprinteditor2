using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace S9BEditor;

internal static class Native
{
	public struct SHFILEINFO
	{
		public const int NAMESIZE = 80;

		public IntPtr hIcon;

		public int iIcon;

		public uint dwAttributes;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szDisplayName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string szTypeName;
	}

	public struct RECT
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;

		public override string ToString()
		{
			return "{L: " + Left + ", T: " + Top + ", R:" + Right + ", B:" + Bottom + "}";
		}
	}

	public struct NCCALCSIZE_PARAMS
	{
		public RECT rcNewWindow;

		public RECT rcOldWindow;

		public RECT rcClient;

		public IntPtr lppos;
	}

	public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

	public struct WINDOWPLACEMENT
	{
		public int length;

		public int flags;

		public int showCmd;

		public Point ptMinPosition;

		public Point ptMaxPosition;

		public Rectangle rcNormalPosition;
	}

	public struct WINDOWPOS
	{
		public IntPtr hwnd;

		public IntPtr hwndInsertAfter;

		public Rectangle rect;

		public int flags;
	}

	public struct MINMAXINFO
	{
		public Point ptReserved;

		public Point ptMaxSize;

		public Point ptMaxPosition;

		public Point ptMinTrackSize;

		public Point ptMaxTrackSize;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class POINT
	{
		public int x;

		public int y;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct SHELLEXECUTEINFO
	{
		public int cbSize;

		public uint fMask;

		public IntPtr hwnd;

		[MarshalAs(UnmanagedType.LPTStr)]
		public string lpVerb;

		[MarshalAs(UnmanagedType.LPTStr)]
		public string lpFile;

		[MarshalAs(UnmanagedType.LPTStr)]
		public string lpParameters;

		[MarshalAs(UnmanagedType.LPTStr)]
		public string lpDirectory;

		public uint nShow;

		public IntPtr hInstApp;

		public IntPtr lpIDList;

		[MarshalAs(UnmanagedType.LPTStr)]
		public string lpClass;

		public IntPtr hkeyClass;

		public uint dwHotKey;

		public IntPtr hIcon;

		public IntPtr hProcess;
	}

	public const int MAX_PATH = 260;

	public const uint SHGFI_ICON = 256u;

	public const uint SHGFI_DISPLAYNAME = 512u;

	public const uint SHGFI_TYPENAME = 1024u;

	public const uint SHGFI_ATTRIBUTES = 2048u;

	public const uint SHGFI_ICONLOCATION = 4096u;

	public const uint SHGFI_EXETYPE = 8192u;

	public const uint SHGFI_SYSICONINDEX = 16384u;

	public const uint SHGFI_LINKOVERLAY = 32768u;

	public const uint SHGFI_SELECTED = 65536u;

	public const uint SHGFI_ATTR_SPECIFIED = 131072u;

	public const uint SHGFI_LARGEICON = 0u;

	public const uint SHGFI_SMALLICON = 1u;

	public const uint SHGFI_OPENICON = 2u;

	public const uint SHGFI_SHELLICONSIZE = 4u;

	public const uint SHGFI_PIDL = 8u;

	public const uint SHGFI_USEFILEATTRIBUTES = 16u;

	public const uint SHGFI_ADDOVERLAYS = 32u;

	public const uint SHGFI_OVERLAYINDEX = 64u;

	public const uint FILE_ATTRIBUTE_DIRECTORY = 16u;

	public const uint FILE_ATTRIBUTE_NORMAL = 128u;

	public const int WM_DESTROY = 2;

	public const int WM_NCCALCSIZE = 131;

	public const int WM_NCPAINT = 133;

	public const int WM_IME_SETCONTEXT = 641;

	public const int WM_IME_NOTIFY = 642;

	public const int WM_KILLFOCUS = 8;

	public const int WM_ACTIVATE = 6;

	public const int WM_ACTIVATEAPP = 28;

	public const int WM_SETTEXT = 12;

	public const int WM_GETTEXT = 13;

	public const int WM_NCACTIVATE = 134;

	public const int WM_SIZE = 5;

	public const int WM_NCHITTEST = 132;

	public const int WM_SYSCOMMAND = 274;

	public const int WM_NCUAHDRAWCAPTION = 174;

	public const int WM_NCUAHDRAWFRAME = 175;

	public const int WM_WINDOWPOSCHANGED = 71;

	public const int WM_GETMINMAXINFO = 36;

	public const int WM_SETICON = 128;

	public const int GWL_STYLE = -16;

	public const int GWL_EXSTYLE = -20;

	public const int GWL_WNDPROC = -4;

	public const int WS_VISIBLE = 268435456;

	public const int WS_EX_DLGMODALFRAME = 1;

	public const int WS_EX_TRANSPARENT = 32;

	public const int SWP_NOSIZE = 1;

	public const int SWP_NOMOVE = 2;

	public const int SWP_NOZORDER = 4;

	public const int SWP_FRAMECHANGED = 32;

	public const int HTBORDER = 18;

	public const int HTBOTTOM = 15;

	public const int HTBOTTOMLEFT = 16;

	public const int HTBOTTOMRIGHT = 17;

	public const int HTCAPTION = 2;

	public const int HTCLIENT = 1;

	public const int HTCLOSE = 20;

	public const int HTERROR = -2;

	public const int HTGROWBOX = 4;

	public const int HTHELP = 21;

	public const int HTHSCROLL = 6;

	public const int HTLEFT = 10;

	public const int HTMENU = 5;

	public const int HTMAXBUTTON = 9;

	public const int HTMINBUTTON = 8;

	public const int HTNOWHERE = 0;

	public const int HTREDUCE = 8;

	public const int HTRIGHT = 11;

	public const int HTSIZE = 4;

	public const int HTSYSMENU = 3;

	public const int HTTOP = 12;

	public const int HTTOPLEFT = 13;

	public const int HTTOPRIGHT = 14;

	public const int HTTRANSPARENT = -1;

	public const int HTVSCROLL = 7;

	public const int HTZOOM = 9;

	public const uint SW_HIDE = 0u;

	public const uint SW_NORMAL = 1u;

	public const uint SW_SHOWMINIMIZED = 2u;

	public const uint SW_MAXIMIZE = 3u;

	public const uint SW_SHOWNOACTIVATE = 4u;

	public const uint SW_SHOW = 5u;

	public const uint SW_MINIMIZE = 6u;

	public const uint SW_SHOWMINNOACTIVE = 7u;

	public const uint SW_SHOWNA = 8u;

	public const uint SW_RESTORE = 9u;

	private const uint SEE_MASK_INVOKEIDLIST = 12u;

	public const uint SC_MAXIMIZE = 61488u;

	public const uint SC_MINIMIZE = 61472u;

	public const uint SC_RESTORE = 61728u;

	public const int WS_MAXIMIZEBOX = 65536;

	public const int WS_MINIMIZEBOX = 131072;

	[DllImport("User32.dll")]
	public static extern int DestroyIcon(IntPtr hIcon);

	[DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
	public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

	[DllImport("gdi32.dll")]
	public static extern bool DeleteObject(IntPtr hObject);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("User32.dll")]
	public static extern IntPtr CallWindowProc(IntPtr wndProc, IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, WndProc dwNewLong);

	[DllImport("user32.dll")]
	public static extern int GetWindowRect(IntPtr hWnd, out RECT rect);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	[DllImport("user32.dll")]
	public static extern IntPtr GetCapture();

	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

	public static short HiWord(int val)
	{
		return (short)((val >> 16) & 0xFFFF);
	}

	public static short LoWord(int val)
	{
		return (short)(val & 0xFFFF);
	}

	public static short GetYLParam(int val)
	{
		return (short)((val >> 16) & 0xFFFF);
	}

	public static short GetXLParam(int val)
	{
		return (short)(val & 0xFFFF);
	}

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool GetClientRect(IntPtr hWnd, out RECT rect);

	[DllImport("User32", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	public static extern int ClientToScreen(IntPtr hWnd, [In][Out] POINT pt);

	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

	public static void RemoveIcon(IntPtr hWnd)
	{
		int num = GetWindowLong(hWnd, -20).ToInt32();
		SetWindowLong(hWnd, -20, num | 1);
		SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, 39u);
	}

	[DllImport("shell32.dll", CharSet = CharSet.Auto)]
	private static extern bool ShellExecuteEx(SHELLEXECUTEINFO lpExecInfo);

	public static bool ShowFileProperties(string Filename)
	{
		return ShowFileProperties(Filename, IntPtr.Zero);
	}

	public static bool ShowFileProperties(string Filename, IntPtr hwnd)
	{
		SHELLEXECUTEINFO lpExecInfo = default(SHELLEXECUTEINFO);
		lpExecInfo.cbSize = Marshal.SizeOf((object)lpExecInfo);
		lpExecInfo.lpVerb = "properties";
		lpExecInfo.lpFile = Filename;
		lpExecInfo.nShow = 1u;
		lpExecInfo.hwnd = hwnd;
		lpExecInfo.fMask = 12u;
		return ShellExecuteEx(lpExecInfo);
	}
}
