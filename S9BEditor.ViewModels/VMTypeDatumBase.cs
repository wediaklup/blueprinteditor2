using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using S9BEditor.Properties;
using TypeEdit.Base;
using TypeEdit.DataHandling;
using TypeEdit.Interfaces.Data.Instancing;

namespace S9BEditor.ViewModels
{
	public abstract class VMTypeDatumBase : VMTypeEditComponent, IDatumBase<VMTypeDatumBase>, IDatumBase
	{
		private bool mModified;

		[Browsable(false)]
		internal ITypeDatum Datum { get; private set; }

		public virtual string Name
		{
			get
			{
				if (Settings.Default.AdvancedDataEditorView)
				{
					return Datum.TypeDescriptor.Name;
				}
				return S9BEUtil.TypeNameToFriendly(Datum.TypeDescriptor.Name);
			}
		}

		public string TypeName => Datum.TypeDescriptor.Name;

		public int ElementIndex { get; private set; }

		public bool Modified
		{
			get
			{
				return mModified;
			}
			set
			{
				if (mModified != value)
				{
					mModified = value;
					RaisePropertyChanged("Modified");
					ModifiedChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		public override bool CanDelete => base.Owner is VMIndexedTypeDatum;

		public override bool CanCopy => true;

		public override bool CanPaste => TypeEditSchemaUtil.CanPasteTo(Datum);

		public override bool CanSelect => base.Owner is VMIndexedTypeDatum;

		VMTypeDatumBase IDatumBase<VMTypeDatumBase>.Owner => base.Owner as VMTypeDatumBase;

		public virtual bool IsIndexed => false;

		public virtual IList<VMTypeDatumBase> Items
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public virtual bool IsClass => false;

		public virtual int ElementCount => Datum.ElementCount;

		public virtual bool CanResetToDefault => false;

		public event EventHandler ModifiedChanged;

		internal VMTypeDatumBase(ViewModelBase owner, ITypeDatum datum, int elementIndex = 0)
			: base(owner)
		{
			Datum = datum;
			ElementIndex = elementIndex;
			((ApplicationSettingsBase)Settings.Default).PropertyChanged += globalSettingsPropertyChanged;
		}

		public virtual bool CommitChanges(bool resetModifiedState)
		{
			if (resetModifiedState)
			{
				Modified = false;
			}
			return true;
		}

		public override void Copy()
		{
			if (CommitChanges(resetModifiedState: false))
			{
				TypeEditSchemaUtil.CopyDatumToClipboard(Datum);
			}
		}

		public override void Paste()
		{
			if (TypeEditSchemaUtil.TryPasteTo(Datum))
			{
				Reload();
				Modified = true;
			}
		}

		public override void Delete()
		{
			if (base.Owner is VMIndexedTypeDatum vMIndexedTypeDatum)
			{
				vMIndexedTypeDatum.RemoveItem(this);
				Dispose();
			}
		}

		public virtual void ResetToDefault()
		{
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

		public virtual void SetValue(string value, int elementIndex, CultureInfo cultureInfo)
		{
			Datum.SetValue(value, elementIndex, cultureInfo);
		}

		public virtual string GetValue(int elementIndex, CultureInfo cultureInfo)
		{
			return Datum.GetValue(elementIndex, cultureInfo);
		}

		public virtual bool TrySetValue(string value, int elementIndex, CultureInfo cultureInfo)
		{
			return Datum.TrySetValue(value, elementIndex, cultureInfo);
		}

		public virtual bool TryGetPropertyDatum(string propertyName, int elementIndex, out VMTypeDatumBase foundProp)
		{
			throw new NotSupportedException();
		}

		protected void SetCurrentError(string errorMsg)
		{
			if (errorMsg == null)
			{
				if (base.Errors != null)
				{
					base.Errors = null;
				}
				return;
			}
			if (base.Errors == null || base.Errors.Length != 1)
			{
				base.Errors = new string[1];
			}
			base.Errors[0] = errorMsg + Environment.NewLine + "@ " + TypeEditSchemaUtil.GetDatumPath(Datum);
		}

		protected void SetCurrentWarning(string warningMsg)
		{
			if (warningMsg == null)
			{
				if (base.Warnings != null)
				{
					base.Warnings = null;
				}
				return;
			}
			if (base.Warnings == null || base.Errors.Length != 1)
			{
				base.Warnings = new string[1];
			}
			base.Warnings[0] = warningMsg + Environment.NewLine + "@ " + TypeEditSchemaUtil.GetDatumPath(Datum);
		}

		public override void BringIntoView()
		{
			if (base.Owner is VMIndexedTypeDatum)
			{
				base.BringIntoView();
			}
			else if (base.Owner is VMProperty vMProperty)
			{
				vMProperty.BringIntoView();
			}
		}
	}
}
