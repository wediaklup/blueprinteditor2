using System.Threading;
using System.Windows;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	internal class UIManager : IUIManager
	{
		public SynchronizationContext Context { get; private set; }

		public UIManager()
		{
			Context = SynchronizationContext.Current;
		}

		public Window CreateStyledWindow(CreateWindowFlags flags = CreateWindowFlags.Default)
		{
			CustomWindow customWindow = new CustomWindow();
			object obj = Application.Current.Resources[(object)"BPEWindowStyle"];
			((FrameworkElement)customWindow).Style = (Style)((obj is Style) ? obj : null);
			if (flags.HasFlag(CreateWindowFlags.DialogBorder))
			{
				((Window)customWindow).ResizeMode = (ResizeMode)0;
			}
			else
			{
				((Window)customWindow).ResizeMode = (ResizeMode)2;
			}
			customWindow.CanMaximize = flags.HasFlag(CreateWindowFlags.CanMaximize);
			customWindow.CanMinimize = flags.HasFlag(CreateWindowFlags.CanMinimize);
			customWindow.HasDialogBackground = flags.HasFlag(CreateWindowFlags.DialogBackground);
			((Window)customWindow).Owner = (Window)(object)AppServices.MainWindow;
			return (Window)(object)customWindow;
		}
	}
}
