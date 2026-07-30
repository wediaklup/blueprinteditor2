using System.Windows;
using System.Windows.Controls;
using S9BEditor.ViewModels;
using TypeEdit.Interfaces.Editing;

namespace S9BEditor
{
	public class DataEditorItemContentTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is VMProperty vmProperty)
			{
				var propertyContent = vmProperty.PropertyContent;
				if (propertyContent is VMClassTypeDatum || propertyContent is VMIndexedTypeDatum)
				{
					object obj = Application.Current.Resources[(object)"propertyHeaderedTemplate"];
					return (DataTemplate)((obj is DataTemplate) ? obj : null);
				}
				if (propertyContent is VMEnumTypeDatum || propertyContent is VMPrimitiveTypeDatum || propertyContent is VMReferenceTypeDatum || propertyContent is VMCustomDatumEditor)
				{
					object obj2 = Application.Current.Resources[(object)"propertySimpleTemplate"];
					return (DataTemplate)((obj2 is DataTemplate) ? obj2 : null);
				}
			}
			if (item is VMTypeDatumBase vMTypeDatumBase)
			{
				if (item is VMIndexedTypeDatum)
				{
					object obj3 = Application.Current.Resources[(object)"indexedContentTemplate"];
					return (DataTemplate)((obj3 is DataTemplate) ? obj3 : null);
				}
				if (item is VMClassTypeDatum)
				{
					object obj4 = Application.Current.Resources[(object)"classContentTemplate"];
					return (DataTemplate)((obj4 is DataTemplate) ? obj4 : null);
				}
				if (vMTypeDatumBase is VMPrimitiveTypeDatum vMPrimitiveTypeDatum)
				{
					if (vMPrimitiveTypeDatum.TypeName == "bool" || vMPrimitiveTypeDatum.TypeName == "sBool")
					{
						object obj5 = Application.Current.Resources[(object)"boolPrimitiveEditor"];
						return (DataTemplate)((obj5 is DataTemplate) ? obj5 : null);
					}
					object obj6 = Application.Current.Resources[(object)"stringPrimitiveEditor"];
					return (DataTemplate)((obj6 is DataTemplate) ? obj6 : null);
				}
				if (vMTypeDatumBase is VMEnumTypeDatum vMEnumTypeDatum)
				{
					if (vMEnumTypeDatum.TypeName == "eBoolean")
					{
						object obj7 = Application.Current.Resources[(object)"boolEnumEditor"];
						return (DataTemplate)((obj7 is DataTemplate) ? obj7 : null);
					}
					object obj8 = Application.Current.Resources[(object)"enumEditor"];
					return (DataTemplate)((obj8 is DataTemplate) ? obj8 : null);
				}
				if (item is VMCustomDatumEditor vMCustomDatumEditor)
				{
					if (vMCustomDatumEditor.Editor is IChildWindowDataEditor)
					{
						object obj9 = Application.Current.Resources[(object)"customChildWindowEditor"];
						return (DataTemplate)((obj9 is DataTemplate) ? obj9 : null);
					}
					object obj10 = Application.Current.Resources[(object)"customDropDownEditor"];
					return (DataTemplate)((obj10 is DataTemplate) ? obj10 : null);
				}
				if (vMTypeDatumBase is VMReferenceTypeDatum)
				{
					object obj11 = Application.Current.Resources[(object)"referenceEditor"];
					return (DataTemplate)((obj11 is DataTemplate) ? obj11 : null);
				}
			}
			return ((DataTemplateSelector)this).SelectTemplate(item, container);
		}
	}
}
