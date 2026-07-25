using System.Windows;
using System.Windows.Controls;
using S9BEditor.ViewModels;

namespace S9BEditor;

internal class DataEditorItemIndexedContentTemplateSelector : DataTemplateSelector
{
	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		if (item is VMTypeDatumBase)
		{
			if (item is VMClassTypeDatum)
			{
				object obj = Application.Current.Resources[(object)"indexedClassContainerTemplate"];
				return (DataTemplate)((obj is DataTemplate) ? obj : null);
			}
			object obj2 = Application.Current.Resources[(object)"indexedDatumItemTemplate"];
			return (DataTemplate)((obj2 is DataTemplate) ? obj2 : null);
		}
		return ((DataTemplateSelector)this).SelectTemplate(item, container);
	}
}
