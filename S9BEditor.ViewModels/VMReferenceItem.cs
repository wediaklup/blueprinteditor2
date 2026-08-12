using System;
using System.ComponentModel;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Instancing;
using TypeEdit.Interfaces.Editing;

namespace S9BEditor.ViewModels
{
	internal class VMReferenceItem : VMTypeEditComponent
	{
		private VMTypeDatumBase mReference;

		private IDatumNameProvider mNameProvider;

		private VMPrimitiveTypeDatum mNameDatum;

		internal ITypeDatum TargetDatum
		{
			get
			{
				if (mReference != null)
				{
					return mReference.Datum;
				}
				return null;
			}
		}

		public bool HasName
		{
			get
			{
				if (mNameDatum == null)
				{
					return mNameProvider != null;
				}
				return true;
			}
		}

		public string Name
		{
			get
			{
				if (mNameProvider != null)
				{
					return mNameProvider.Name;
				}
				if (mNameDatum != null)
				{
					return mNameDatum.Value;
				}
				return "(unset)";
			}
		}

		internal VMReferenceItem(ViewModelBase owner, VMTypeDatumBase reference)
			: base(owner)
		{
			if (reference != null)
			{
				mReference = reference;
				mReference.Reloaded += reference_Reloaded;
			}
			updateNameDatum();
		}

		private void reference_Reloaded(object sender, EventArgs e)
		{
			updateNameDatum();
		}

		private void updateNameDatum()
		{
			string text = "";
			if (mNameProvider != null)
			{
				mNameProvider.NameChanged -= asNameProvider_NameChanged;
				text = mNameProvider.Name;
				mNameProvider = null;
			}
			else if (mNameDatum != null)
			{
				mNameDatum.PropertyChanged -= NameDatum_PropertyChanged;
				text = mNameDatum.Value;
				mNameDatum = null;
			}
			if (mReference is VMClassTypeDatum vMClassTypeDatum)
			{
				VMPrimitiveTypeDatum vMPrimitiveTypeDatum = null;
				foreach (VMClassGroup group in vMClassTypeDatum.Groups)
				{
					foreach (VMProperty property in group.Properties)
					{
						if (property.PropertyContent.Datum is IPrimitiveTypeDatum && property.Property.Descriptor.Name == "Name")
						{
							vMPrimitiveTypeDatum = property.PropertyContent as VMPrimitiveTypeDatum;
							break;
						}
					}
					if (vMPrimitiveTypeDatum != null)
					{
						break;
					}
				}
				if (vMPrimitiveTypeDatum != null && mNameDatum != vMPrimitiveTypeDatum)
				{
					mNameDatum = vMPrimitiveTypeDatum;
				}
			}
			else if (mReference is VMCustomDatumEditor vmCustomDatumEditor && vmCustomDatumEditor.Editor is IDatumNameProvider editor)
			{
				mNameProvider = editor;
			}
			string text2 = "";
			if (mNameProvider != null)
			{
				text2 = mNameProvider.Name;
				mNameProvider.NameChanged += asNameProvider_NameChanged;
			}
			else if (mNameDatum != null)
			{
				text2 = mNameDatum.Value;
				mNameDatum.PropertyChanged += NameDatum_PropertyChanged;
			}
			if (text2 != text)
			{
				RaisePropertyChanged("Name");
			}
			RaisePropertyChanged("HasName");
		}

		private void asNameProvider_NameChanged(object sender, EventArgs e)
		{
			RaisePropertyChanged("Name");
		}

		private void NameDatum_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Value")
			{
				RaisePropertyChanged("Name");
			}
		}

		public override void Dispose()
		{
			if (mReference != null)
			{
				mReference.Reloaded -= reference_Reloaded;
			}
			if (mNameDatum != null)
			{
				mNameDatum.PropertyChanged -= NameDatum_PropertyChanged;
			}
			if (mNameProvider != null)
			{
				mNameProvider.NameChanged -= asNameProvider_NameChanged;
			}
			base.Dispose();
		}
	}
}
