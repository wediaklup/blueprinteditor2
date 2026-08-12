using System;
using S9BEditor.Scripts;
using TypeEdit.Base;
using TypeEdit.Interfaces;

namespace S9BEditor.ViewModels
{
	internal class VMScriptProperty : ViewModelBase
	{
		public ScriptPropertyDefinition Property { get; private set; }

		public string Value
		{
			get
			{
				return Property.Value;
			}
			set
			{
				if (Property.Value != value)
				{
					Property.Value = value;
					RaisePropertyChanged("Value");
				}
			}
		}

		public string Name => Property.PropertyInfo.Name;

		public ScriptPropertyAttribute ScriptPropertyAttribute { get; private set; }

		public VMScriptProperty(ScriptPropertyDefinition property, VMScript owner)
			: base(owner)
		{
			Property = property;
			object[] customAttributes = property.PropertyInfo.GetCustomAttributes(typeof(ScriptPropertyAttribute), inherit: true);
			if (customAttributes.Length > 0)
			{
				ScriptPropertyAttribute = (ScriptPropertyAttribute)customAttributes[0];
				return;
			}
			throw new InvalidOperationException("VMScriptProperty expects a ScriptPropertyAttribute on the property");
		}
	}
}
