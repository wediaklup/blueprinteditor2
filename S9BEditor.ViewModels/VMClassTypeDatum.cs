using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using TypeEdit.Base;
using TypeEdit.DataHandling;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.Data.Instancing;

namespace S9BEditor.ViewModels
{
	internal class VMClassTypeDatum : VMTypeDatumBase
	{
		private object mModifyLock;

		private ObservableCollection<VMClassGroup> mGroups;

		private VMPrimitiveTypeDatum mDisplayNameDatum;

		private bool mInitialised;

		private ReadOnlyObservableCollection<VMClassGroup> mGroupsReadOnly;

		private bool? mCachedIsEmpty;

		private bool mIsValid;

		public override bool IsClass => true;

		public ReadOnlyObservableCollection<VMClassGroup> Groups
		{
			get
			{
				return mGroupsReadOnly;
			}
			private set
			{
				if (mGroupsReadOnly != value)
				{
					mGroupsReadOnly = value;
					RaisePropertyChanged("Groups");
				}
			}
		}

		internal new IClassTypeDatum Datum => base.Datum as IClassTypeDatum;

		public bool IsEmpty
		{
			get
			{
				if (!mCachedIsEmpty.HasValue)
				{
					int num = 0;
					foreach (VMClassGroup mGroup in mGroups)
					{
						num += mGroup.Properties.Count;
					}
					mCachedIsEmpty = num == 0;
				}
				return mCachedIsEmpty.Value;
			}
		}

		public override bool IsValid => mIsValid;

		public override string Name
		{
			get
			{
				if (mDisplayNameDatum != null)
				{
					if (mDisplayNameDatum.Value.Length > 0)
					{
						return base.Name + ": \"" + mDisplayNameDatum.Value + "\"";
					}
					return base.Name + ": <unnamed>";
				}
				return base.Name;
			}
		}

		internal VMClassTypeDatum(ViewModelBase owner, IClassTypeDatum datum, int elementIndex = 0)
			: base(owner, datum, elementIndex)
		{
			mIsValid = true;
			mGroups = new ObservableCollection<VMClassGroup>();
			Groups = new ReadOnlyObservableCollection<VMClassGroup>(mGroups);
			mModifyLock = new object();
			Reload();
		}

		public bool TryGetProperty(string propertyName, int elementIndex, out VMProperty foundProp)
		{
			lock (mModifyLock)
			{
				foreach (VMClassGroup group in Groups)
				{
					foreach (VMProperty property in group.Properties)
					{
						if (property.Property.Descriptor.Name == propertyName)
						{
							foundProp = property;
							return true;
						}
					}
				}
			}
			foundProp = null;
			return false;
		}

		public override bool TryGetPropertyDatum(string propertyName, int elementIndex, out VMTypeDatumBase foundDatum)
		{
			if (TryGetProperty(propertyName, elementIndex, out var foundProp))
			{
				foundDatum = foundProp.PropertyContent;
				return true;
			}
			foundDatum = null;
			return false;
		}

		private void setIsValid(bool value)
		{
			if (value != mIsValid)
			{
				mIsValid = value;
				RaisePropertyChanged("IsValid");
			}
		}

		public override bool CommitChanges(bool resetModifiedState)
		{
			bool flag = true;
			lock (mModifyLock)
			{
				foreach (VMClassGroup group in Groups)
				{
					foreach (VMProperty property in group.Properties)
					{
						if (!property.CommitChanges(resetModifiedState))
						{
							flag = false;
						}
					}
				}
			}
			if (flag)
			{
				return base.CommitChanges(resetModifiedState);
			}
			return false;
		}

		public override void Dispose()
		{
			clearCurrentData();
			base.Dispose();
		}

		private void clearCurrentData()
		{
			lock (mModifyLock)
			{
				if (mDisplayNameDatum != null)
				{
					mDisplayNameDatum.PropertyChanged -= displayNameDatum_PropertyChanged;
					mDisplayNameDatum = null;
				}
				foreach (VMClassGroup mGroup in mGroups)
				{
					foreach (VMProperty property in mGroup.Properties)
					{
						property.PropertyContent.PropertyChanged -= PropertyContent_PropertyChanged;
					}
					mGroup.Dispose();
				}
				mGroups.Clear();
			}
		}

		public override void Reload()
		{
			string text = "";
			if (mDisplayNameDatum != null && mDisplayNameDatum.Value.Length > 0)
			{
				text = mDisplayNameDatum.Value;
			}
			clearCurrentData();
			lock (mModifyLock)
			{
				IClassTypeDatum datum = Datum;
				if (datum != null)
				{
					IClassTypeDescriptor typeDescriptor = datum.TypeDescriptor;
					for (int i = 0; i < typeDescriptor.Groups.Count; i++)
					{
						_ = typeDescriptor.Groups[i];
						List<IProperty> list = new List<IProperty>();
						for (int j = 0; j < datum.Properties.GetLength(1); j++)
						{
							IProperty property = datum.Properties[base.ElementIndex, j];
							if (property.Descriptor.ParentGroup == typeDescriptor.Groups[i])
							{
								list.Add(property);
							}
						}
						VMClassGroup vMClassGroup = new VMClassGroup(this, typeDescriptor.Groups[i], list);
						bool flag = false;
						VMPrimitiveTypeDatum vMPrimitiveTypeDatum = null;
						foreach (VMProperty property3 in vMClassGroup.Properties)
						{
							property3.PropertyContent.PropertyChanged += PropertyContent_PropertyChanged;
							IProperty property2 = property3.Property;
							if (!flag)
							{
								flag = TypeEditSchemaUtil.IsDisplayMemberNameProperty(property2);
								if (flag || (property2.Datum is IPrimitiveTypeDatum && property2.Descriptor.Name == "Name"))
								{
									vMPrimitiveTypeDatum = property3.PropertyContent as VMPrimitiveTypeDatum;
								}
							}
						}
						if (vMPrimitiveTypeDatum != null)
						{
							mDisplayNameDatum = vMPrimitiveTypeDatum;
							mDisplayNameDatum.PropertyChanged += displayNameDatum_PropertyChanged;
							if (text != mDisplayNameDatum.Value)
							{
								RaisePropertyChanged("Name");
							}
						}
						mGroups.Add(vMClassGroup);
					}
				}
				if (mInitialised)
				{
					initialiseGroups();
				}
			}
			base.Reload();
		}

		private void updateChildPropertyEnabledStates()
		{
			foreach (VMClassGroup group in Groups)
			{
				foreach (VMProperty property in group.Properties)
				{
					IEditHint editHint = property.Property.Descriptor.GetEditHint("Condition");
					if (editHint == null)
					{
						continue;
					}
					bool isEnabled = false;
					string editHintValue = editHint.GetEditHintValue("property");
					VMTypeDatumBase singleDatum = TypeEditSchemaUtil.GetSingleDatum((VMTypeDatumBase)this, editHintValue, 0);
					if (singleDatum != null)
					{
						string editHintValue2 = editHint.GetEditHintValue("values");
						if (editHintValue2 != null)
						{
							string[] array = editHintValue2.Split(new string[1] { "||" }, StringSplitOptions.None);
							string value = singleDatum.GetValue(CultureInfo.InvariantCulture);
							if (Array.IndexOf(array, value) != -1)
							{
								isEnabled = true;
							}
						}
					}
					property.IsEnabled = isEnabled;
				}
			}
		}

		private void PropertyContent_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Modified")
			{
				if (sender is VMTypeDatumBase vMTypeDatumBase)
				{
					base.Modified |= vMTypeDatumBase.Modified;
				}
			}
			else if (e.PropertyName == "IsValid")
			{
				bool flag = true;
				foreach (VMClassGroup group in Groups)
				{
					foreach (VMProperty property in group.Properties)
					{
						if (!property.PropertyContent.IsValid)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
				setIsValid(flag);
			}
			else if (e.PropertyName == "ValidationRequired")
			{
				bool flag2 = false;
				foreach (VMClassGroup group2 in Groups)
				{
					foreach (VMProperty property2 in group2.Properties)
					{
						if (property2.PropertyContent.ValidationRequired)
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						break;
					}
				}
				base.ValidationRequired = flag2;
			}
			else if (e.PropertyName == "Value")
			{
				updateChildPropertyEnabledStates();
			}
		}

		private void displayNameDatum_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Value")
			{
				RaisePropertyChanged("Name");
			}
		}

		internal override void Initialise()
		{
			if (!mInitialised)
			{
				mInitialised = true;
				initialiseGroups();
			}
			base.Initialise();
		}

		private void initialiseGroups()
		{
			lock (mModifyLock)
			{
				if (Groups != null)
				{
					foreach (VMClassGroup group in Groups)
					{
						group.Initialise();
					}
				}
				updateChildPropertyEnabledStates();
			}
		}
	}
}
