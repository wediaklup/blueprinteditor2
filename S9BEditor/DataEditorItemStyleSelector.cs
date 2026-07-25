using System.Windows;
using System.Windows.Controls;
using S9BEditor.ViewModels;

namespace S9BEditor;

public class DataEditorItemStyleSelector : StyleSelector
{
	public override Style SelectStyle(object item, DependencyObject container)
	{
		if (item is VMClassGroup vMClassGroup)
		{
			if (vMClassGroup.IsDefaultGroup)
			{
				object obj = Application.Current.Resources[(object)"nonHeaderedContainerStyle"];
				return (Style)((obj is Style) ? obj : null);
			}
			object obj2 = Application.Current.Resources[(object)"headeredGroupStyle"];
			return (Style)((obj2 is Style) ? obj2 : null);
		}
		if (item is VMProperty { PropertyContent: var propertyContent })
		{
			if (propertyContent is VMClassTypeDatum { IsEmpty: false })
			{
				object obj3 = Application.Current.Resources[(object)"headeredPropertyContainerStyle"];
				return (Style)((obj3 is Style) ? obj3 : null);
			}
			if (propertyContent is VMIndexedTypeDatum)
			{
				object obj4 = Application.Current.Resources[(object)"headeredIndexedPropertyContainerStyle"];
				return (Style)((obj4 is Style) ? obj4 : null);
			}
			if (propertyContent is VMPrimitiveTypeDatum || propertyContent is VMEnumTypeDatum || propertyContent is VMReferenceTypeDatum || propertyContent is VMCustomDatumEditor)
			{
				object obj5 = Application.Current.Resources[(object)"propertyItemStyle"];
				return (Style)((obj5 is Style) ? obj5 : null);
			}
			object obj6 = Application.Current.Resources[(object)"hiddenContainerStyle"];
			return (Style)((obj6 is Style) ? obj6 : null);
		}
		object obj7 = Application.Current.Resources[(object)"nonHeaderedContainerStyle"];
		return (Style)((obj7 is Style) ? obj7 : null);
	}
}
