using System.ComponentModel;
using System.Configuration;
using S9BEditor.Properties;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Descriptors;

namespace S9BEditor.ViewModels
{
	internal class VMEnumItem : ViewModelBase
	{
		private IEnumItemDescriptor mEnumItemDescriptor;

		private IClassTypeDescriptor mAssociatedMeta;

		public string Value => mEnumItemDescriptor.Name;

		public string Name
		{
			get
			{
				if (Settings.Default.AdvancedDataEditorView)
				{
					return mEnumItemDescriptor.Name;
				}
				if (TryGetFriendlyEnumItemName(out var friendlyName))
				{
					return friendlyName;
				}
				return S9BEUtil.CamelCaseToNormal(mEnumItemDescriptor.Name);
			}
		}

		internal VMEnumItem(ViewModelBase owner, IEnumItemDescriptor enumItemDescriptor)
			: base(owner)
		{
			mEnumItemDescriptor = enumItemDescriptor;
			string name = "cMeta_" + enumItemDescriptor.Owner.Name;
			mAssociatedMeta = enumItemDescriptor.Owner.GetTypeEditSchema().GetTypeFromName(name) as IClassTypeDescriptor;
			((ApplicationSettingsBase)Settings.Default).PropertyChanged += globalSettingsPropertyChanged;
		}

		private void globalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "AdvancedDataEditorView")
			{
				RaisePropertyChanged("Name");
			}
		}

		public bool TryGetFriendlyEnumItemName(out string friendlyName)
		{
			if (mAssociatedMeta != null)
			{
				return mAssociatedMeta.TryGetEditHintValue("EnumMember:" + mEnumItemDescriptor.Name, "name", out friendlyName);
			}
			friendlyName = null;
			return false;
		}

		public override void Dispose()
		{
			((ApplicationSettingsBase)Settings.Default).PropertyChanged -= globalSettingsPropertyChanged;
			base.Dispose();
		}
	}
}
