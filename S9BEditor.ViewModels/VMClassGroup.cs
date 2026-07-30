using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.Data.Instancing;

namespace S9BEditor.ViewModels
{
	internal class VMClassGroup : VMTypeEditComponent
	{
		private IClassGroupDescriptor mGroup;

		private ObservableCollection<VMProperty> mProperties;

		public ReadOnlyObservableCollection<VMProperty> Properties { get; private set; }

		public bool IsDefaultGroup { get; private set; }

		public string GroupName => mGroup.Name;

		internal VMClassGroup(ViewModelBase owner, IClassGroupDescriptor group, IEnumerable<IProperty> properties)
			: base(owner)
		{
			mGroup = group;
			mProperties = new ObservableCollection<VMProperty>();
			Properties = new ReadOnlyObservableCollection<VMProperty>(mProperties);
			foreach (IProperty property in properties)
			{
				if (property.Datum != null)
				{
					mProperties.Add(new VMProperty(this, property));
				}
			}
			IsDefaultGroup = mGroup.Name.Equals("Default", StringComparison.InvariantCultureIgnoreCase);
		}

		public override void Dispose()
		{
			foreach (VMProperty property in Properties)
			{
				property.Dispose();
			}
			mProperties.Clear();
			base.Dispose();
		}

		internal override void Initialise()
		{
			foreach (VMProperty property in Properties)
			{
				property.Initialise();
			}
			base.Initialise();
		}
	}
}
