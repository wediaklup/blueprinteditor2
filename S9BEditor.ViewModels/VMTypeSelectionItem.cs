using System.ComponentModel;
using System.Configuration;
using S9BEditor.Properties;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Descriptors;

namespace S9BEditor.ViewModels
{
	internal class VMTypeSelectionItem : ViewModelBase
	{
		public string Name
		{
			get
			{
				if (Settings.Default.AdvancedDataEditorView)
				{
					return TypeDescriptor.Name;
				}
				return S9BEUtil.CamelCaseToNormal(S9BEUtil.RemoveNamespaces(TypeDescriptor.Name));
			}
		}

		public ITypeDescriptor TypeDescriptor { get; private set; }

		internal VMTypeSelectionItem(ViewModelBase owner, ITypeDescriptor desc)
			: base(owner)
		{
			TypeDescriptor = desc;
			((ApplicationSettingsBase)Settings.Default).PropertyChanged += globalSettingsPropertyChanged;
		}

		private void globalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "AdvancedDataEditorView")
			{
				RaisePropertyChanged("Name");
			}
		}

		public override void Dispose()
		{
			((ApplicationSettingsBase)Settings.Default).PropertyChanged -= globalSettingsPropertyChanged;
			base.Dispose();
		}
	}
}
