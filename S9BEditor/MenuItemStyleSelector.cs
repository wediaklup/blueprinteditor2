using System.Windows;
using System.Windows.Controls;
using S9BEditor.ViewModels;

namespace S9BEditor;

internal class MenuItemStyleSelector : StyleSelector
{
	public override Style SelectStyle(object item, DependencyObject container)
	{
		if (item is VMMenuItem vMMenuItem)
		{
			if (vMMenuItem.IsSeparator)
			{
				object obj = Application.Current.Resources[(object)"separatorActionItemStyle"];
				return (Style)((obj is Style) ? obj : null);
			}
			object obj2 = Application.Current.Resources[(object)"menuItemActionItemStyle"];
			return (Style)((obj2 is Style) ? obj2 : null);
		}
		return ((StyleSelector)this).SelectStyle(item, container);
	}
}
