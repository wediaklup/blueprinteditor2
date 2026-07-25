using System.Windows;
using System.Windows.Controls;
using S9BEditor.ViewModels;
using TypeEdit.Interfaces;

namespace S9BEditor;

internal class ScriptPropertyDataTemplateSelector : DataTemplateSelector
{
	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		if (item is VMScriptProperty vMScriptProperty)
		{
			switch (vMScriptProperty.ScriptPropertyAttribute.SpecialType)
			{
			case ScriptPropertySpecialType.Provider:
			{
				object obj3 = Application.Current.Resources[(object)"providerScriptProperty"];
				return (DataTemplate)((obj3 is DataTemplate) ? obj3 : null);
			}
			case ScriptPropertySpecialType.Product:
			{
				object obj2 = Application.Current.Resources[(object)"productScriptProperty"];
				return (DataTemplate)((obj2 is DataTemplate) ? obj2 : null);
			}
			default:
			{
				object obj = Application.Current.Resources[(object)"stringScriptProperty"];
				return (DataTemplate)((obj is DataTemplate) ? obj : null);
			}
			}
		}
		return ((DataTemplateSelector)this).SelectTemplate(item, container);
	}
}
