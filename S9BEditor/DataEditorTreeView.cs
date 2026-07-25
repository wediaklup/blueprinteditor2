using System.Windows;
using System.Windows.Controls;

namespace S9BEditor;

internal class DataEditorTreeView : TreeView
{
	public DataEditorTreeView()
	{
		((FrameworkElement)this).DefaultStyleKey = typeof(DataEditorTreeView);
	}

	protected override DependencyObject GetContainerForItemOverride()
	{
		return (DependencyObject)(object)new DataEditorTreeViewItem();
	}
}
