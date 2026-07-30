using System;
using System.Collections.ObjectModel;
using System.Globalization;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Instancing;

namespace S9BEditor.ViewModels
{
	internal class VMEnumTypeDatum : VMTypeDatumBase
	{
		private IEnumTypeDatum mDatumAsEnum;

		private int mElementIndex;

		private VMEnumItem mSelectedItem;

		internal new IEnumTypeDatum Datum => base.Datum as IEnumTypeDatum;

		public VMEnumItem SelectedItem
		{
			get
			{
				return mSelectedItem;
			}
			set
			{
				if (mSelectedItem != value)
				{
					mSelectedItem = value;
					base.Modified = true;
					base.ValidationRequired = true;
					FindTopMost<VMTypeEditContainer>()?.RequestRevalidation(this);
					RaisePropertyChanged("SelectedItem");
					RaisePropertyChanged("Value");
				}
			}
		}

		public string Value
		{
			get
			{
				if (SelectedItem != null)
				{
					return SelectedItem.Value;
				}
				return "";
			}
			set
			{
				foreach (VMEnumItem possibleValue in PossibleValues)
				{
					if (possibleValue.Value == value)
					{
						SelectedItem = possibleValue;
						break;
					}
				}
			}
		}

		public ReadOnlyCollection<VMEnumItem> PossibleValues { get; private set; }

		public override bool IsValid => base.Errors == null;

		public override bool CanResetToDefault => true;

		internal VMEnumTypeDatum(ViewModelBase owner, IEnumTypeDatum datum, int elementIndex = 0)
			: base(owner, datum, elementIndex)
		{
			mDatumAsEnum = datum;
			mElementIndex = elementIndex;
			VMTypeEditContainer vMTypeEditContainer = FindTopMost<VMTypeEditContainer>();
			if (vMTypeEditContainer == null)
			{
				return;
			}
			PossibleValues = vMTypeEditContainer.Factory.GetEnumItems(datum.TypeDescriptor);
			string value = mDatumAsEnum.GetValue(mElementIndex);
			foreach (VMEnumItem possibleValue in PossibleValues)
			{
				if (possibleValue.Value == value)
				{
					mSelectedItem = possibleValue;
					break;
				}
			}
		}

		public override void SetValue(string value, int elementIndex, CultureInfo cultureInfo)
		{
			if (elementIndex != 0)
			{
				throw new NotSupportedException("elementIndex must be 0");
			}
			Value = value;
		}

		public override string GetValue(int elementIndex, CultureInfo cultureInfo)
		{
			if (elementIndex != 0)
			{
				throw new NotSupportedException("elementIndex must be 0");
			}
			return Value;
		}

		public override bool TrySetValue(string value, int elementIndex, CultureInfo cultureInfo)
		{
			SetValue(value, elementIndex, cultureInfo);
			return true;
		}

		public override bool CommitChanges(bool resetModifiedState)
		{
			base.ValidationRequired = false;
			if (Value != mDatumAsEnum.GetValue(mElementIndex) && !mDatumAsEnum.TrySetValue(Value, mElementIndex))
			{
				SetCurrentError(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid value.", new object[1] { Value }));
				RaisePropertyChanged("IsValid");
				return false;
			}
			SetCurrentError(null);
			RaisePropertyChanged("IsValid");
			return base.CommitChanges(resetModifiedState);
		}

		public override void ResetToDefault()
		{
			base.ResetToDefault();
			Value = mDatumAsEnum.TypeDescriptor.GetDefaultValue();
			CommitChanges(resetModifiedState: false);
		}

		public override void Reload()
		{
			Value = mDatumAsEnum.GetValue(mElementIndex);
			base.Reload();
		}
	}
}
