using System.Windows;
using System.Windows.Controls;
using S9BEditor.ViewModels;

namespace S9BEditor
{
	internal class DataEditorItemIndexedStyleSelector : StyleSelector
	{
		public override Style SelectStyle(object item, DependencyObject container)
		{
			if (item is VMClassTypeDatum)
			{
				object obj = Application.Current.Resources[(object)"headeredIndexedContainerStyle"];
				return (Style)((obj is Style) ? obj : null);
			}
			object obj2 = Application.Current.Resources[(object)"indexedItemStyle"];
			return (Style)((obj2 is Style) ? obj2 : null);
		}
	}
}
