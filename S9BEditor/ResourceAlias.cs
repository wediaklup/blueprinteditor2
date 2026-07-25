using System;
using System.Collections;
using System.Windows.Markup;
using System.Xaml;

namespace S9BEditor;

[MarkupExtensionReturnType(typeof(object))]
internal class ResourceAlias : MarkupExtension
{
	public object ResourceKey { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		IRootObjectProvider val = (IRootObjectProvider)serviceProvider.GetService(typeof(IRootObjectProvider));
		if (val == null)
		{
			return null;
		}
		if (!(val.RootObject is IDictionary dictionary))
		{
			return null;
		}
		return dictionary[ResourceKey];
	}
}
