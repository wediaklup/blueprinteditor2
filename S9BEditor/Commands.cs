using System.Collections;
using System.Windows.Input;

namespace S9BEditor
{
	internal static class Commands
	{
		public static readonly RoutedUICommand SaveAll;

		public static readonly RoutedUICommand CloseAll;

		public static readonly RoutedUICommand SaveOutput;

		public static readonly RoutedUICommand AddItem;

		public static readonly RoutedUICommand AddFolder;

		public static readonly RoutedUICommand OpenInExplorer;

		public static readonly RoutedUICommand Rename;

		public static readonly RoutedUICommand Properties;

		public static readonly RoutedUICommand OpenFile;

		public static readonly RoutedUICommand ReloadBlueprintSchema;

		public static readonly RoutedUICommand RunScript;

		static Commands()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Expected O, but got Unknown
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Expected O, but got Unknown
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Expected O, but got Unknown
			SaveAll = new RoutedUICommand("Save All", "SaveAll", typeof(Commands), new InputGestureCollection((IList)new KeyGesture[1]
			{
				new KeyGesture((Key)62, (ModifierKeys)6)
			}));
			CloseAll = new RoutedUICommand("Close All", "CloseAll", typeof(Commands));
			SaveOutput = new RoutedUICommand("Save Output", "SaveOutput", typeof(Commands));
			AddItem = new RoutedUICommand("Add Item", "AddItem", typeof(Commands));
			AddFolder = new RoutedUICommand("Add Folder", "AddFolder", typeof(Commands));
			OpenInExplorer = new RoutedUICommand("Open in Explorer", "OpenInExplorer", typeof(Commands));
			Rename = new RoutedUICommand("Rename", "Rename", typeof(Commands), new InputGestureCollection((IList)new KeyGesture[1]
			{
				new KeyGesture((Key)91, (ModifierKeys)0)
			}));
			Properties = new RoutedUICommand("Properties", "Properties", typeof(Commands));
			OpenFile = new RoutedUICommand("Open File", "OpenFile", typeof(Commands));
			ReloadBlueprintSchema = new RoutedUICommand("Reload BlueprintSchema.xml", "ReloadBlueprintSchema", typeof(Commands));
			RunScript = new RoutedUICommand("Run Script", "RunScript", typeof(Commands));
		}
	}
}
