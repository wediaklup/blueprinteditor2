using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace S9BEditor
{
	internal class S9BEUtil
	{
		public enum FileIconFlags : uint
		{
			None = 0u,
			SmallSize = 1u,
			LinkOverlay = 32768u,
			AddOverlays = 32u
		}

		public static Icon GetFileIcon(string name, FileIconFlags iconFlags)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			Native.SHFILEINFO psfi = default(Native.SHFILEINFO);
			uint uFlags = (uint)(iconFlags | (FileIconFlags)0x100u | (FileIconFlags)0x10u);
			Native.SHGetFileInfo(name, 128u, ref psfi, (uint)Marshal.SizeOf((object)psfi), uFlags);
			if (psfi.hIcon != IntPtr.Zero)
			{
				Icon result = (Icon)Icon.FromHandle(psfi.hIcon).Clone();
				Native.DestroyIcon(psfi.hIcon);
				return result;
			}
			return null;
		}

		public static bool IsPopupDescendant(DependencyObject parent, DependencyObject child)
		{
			while (child != null)
			{
				ContextMenu val = (ContextMenu)(object)((child is ContextMenu) ? child : null);
				DependencyObject val2 = (DependencyObject)((val == null) ? ((object)VisualTreeHelper.GetParent(child)) : ((object)val.PlacementTarget));
				DependencyObject parent2 = LogicalTreeHelper.GetParent(child);
				Popup val3 = (Popup)(object)((parent2 is Popup) ? parent2 : null);
				child = (DependencyObject)((val2 == null) ? ((object)val3) : ((object)val2));
				if (child == parent)
				{
					return true;
				}
			}
			return false;
		}

		public static string TypeNameToFriendly(string text)
		{
			return CamelCaseToNormal(RemoveNamespaces(text));
		}

		public static string RemoveNamespaces(string text)
		{
			if (text.Length > 0)
			{
				return text.Substring(text.LastIndexOf('-') + 1);
			}
			return text;
		}

		public static string CamelCaseToNormal(string text)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '_')
				{
					if (num == 0)
					{
						num = 1;
					}
					else
					{
						stringBuilder.Append(" - ");
					}
					num = 1;
					continue;
				}
				if (text[i] == '-')
				{
					stringBuilder.Append(" - ");
					num = 0;
					continue;
				}
				switch (num)
				{
					case 0:
						if (!char.IsLower(text[i]))
						{
							num = 2;
							stringBuilder.Append(text[i]);
						}
						break;
					case 1:
						if (text[i - 1] == '_')
						{
							stringBuilder.Append(char.ToUpper(text[i], CultureInfo.CurrentCulture));
						}
						else
						{
							stringBuilder.Append(text[i]);
						}
						num = 2;
						break;
					case 2:
					{
						bool flag = i + 1 == text.Length;
						int num2 = (char.IsLower(text[i - 1]) ? 1 : 0);
						int num3 = (char.IsUpper(text[i - 1]) ? 1 : 0);
						int num4 = (char.IsNumber(text[i - 1]) ? 1 : 0);
						int num5 = (char.IsLower(text[i]) ? 1 : 0);
						int num6 = (char.IsUpper(text[i]) ? 1 : 0);
						int num7 = (char.IsNumber(text[i]) ? 1 : 0);
						int num8 = ((!flag && char.IsLower(text[i + 1])) ? 1 : 0);
						int num9 = ((!flag && char.IsNumber(text[i + 1])) ? 1 : 0);
						if (num2 * (num6 + num7) + num3 * (num6 * num8 + num7) + num4 * (num5 + num6 * (num8 + num9)) != 0)
						{
							stringBuilder.Append(' ');
							if (num6 * num8 != 0)
							{
								stringBuilder.Append(char.ToLower(text[i], CultureInfo.CurrentCulture));
							}
							else
							{
								stringBuilder.Append(text[i]);
							}
						}
						else
						{
							stringBuilder.Append(text[i]);
						}
						break;
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
