using System.Windows;
using System.Windows.Controls;

namespace S9BEditor
{
	public class DataEditorTreeView : TreeView
	{
		public DataEditorTreeView()
		{
			base.DefaultStyleKey = typeof(DataEditorTreeView);
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return (DependencyObject)(object)new DataEditorTreeViewItem();
		}
	}
}
