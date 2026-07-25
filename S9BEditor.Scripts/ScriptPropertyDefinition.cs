using System.Reflection;

namespace S9BEditor.Scripts;

internal class ScriptPropertyDefinition
{
	public PropertyInfo PropertyInfo { get; private set; }

	public string Value { get; set; }

	public ScriptPropertyDefinition(PropertyInfo pi, string initialValue)
	{
		PropertyInfo = pi;
		Value = initialValue;
	}
}
