using System;
using System.Globalization;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.Data.Instancing;

namespace S9BEditor.ViewModels
{
	internal class VMPrimitiveTypeDatum : VMTypeDatumBase
	{
		private IPrimitiveTypeDatum mDatumAsPrimitive;

		private int mElementIndex;

		private string mIntermediateValue;

		public string Value
		{
			get
			{
				return mIntermediateValue;
			}
			set
			{
				if (mIntermediateValue != value)
				{
					mIntermediateValue = value;
					base.Modified = true;
					base.ValidationRequired = true;
					FindTopMost<VMTypeEditContainer>()?.RequestRevalidation(this);
					RaisePropertyChanged("Value");
				}
			}
		}

		public string Units
		{
			get
			{
				string text = string.Empty;
				if (base.Owner is VMProperty vmProperty)
				{
					var property = vmProperty.Property;
					IEditHint editHint = property.Descriptor.GetEditHint("Units");
					if (editHint != null)
					{
						text = editHint.GetEditHintValue("value");
						switch (text)
						{
							case "METRES_PER_SEC_PER_SEC":
								text = "m/s²";
								break;
							case "DEGREES":
								text = "°";
								break;
							case "GALLONS(UK) PER HOUR":
								text = "gph (UK)";
								break;
							case "INCHES_OF_MERCURY_PER_SECOND":
								text = "inHg/s";
								break;
							case "INCHES":
								text = "in";
								break;
							case "NEWTONS/METRE":
								text = "N/m";
								break;
							case "NEWTON.SECONDS/METRE":
								text = "N·s/m";
								break;
						}
					}
				}
				return text;
			}
		}

		public override bool IsValid => base.Errors == null;

		internal new IPrimitiveTypeDatum Datum => base.Datum as IPrimitiveTypeDatum;

		public override bool CanResetToDefault => true;

		internal VMPrimitiveTypeDatum(ViewModelBase owner, IPrimitiveTypeDatum datum, int elementIndex = 0)
			: base(owner, datum, elementIndex)
		{
			mDatumAsPrimitive = datum;
			mElementIndex = elementIndex;
			mIntermediateValue = mDatumAsPrimitive.GetValue(mElementIndex);
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

		public override void Delete()
		{
			Value = string.Empty;
			base.Delete();
		}

		public override bool CommitChanges(bool resetModifiedState)
		{
			base.ValidationRequired = false;
			if (Value != mDatumAsPrimitive.GetValue(mElementIndex) && !mDatumAsPrimitive.TrySetValue(mIntermediateValue, mElementIndex))
			{
				SetCurrentError(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid value.", new object[1] { mIntermediateValue }));
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
			Value = mDatumAsPrimitive.TypeDescriptor.ValueToString(mDatumAsPrimitive.TypeDescriptor.GetDefaultValue(), CultureInfo.InvariantCulture);
			CommitChanges(resetModifiedState: false);
		}

		public override void Reload()
		{
			Value = mDatumAsPrimitive.GetValue(mElementIndex);
			base.Reload();
		}
	}
}
