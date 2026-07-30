using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.Data.Instancing;

namespace S9BEditor.ViewModels
{
	internal class VMIndexedTypeDatum : VMTypeDatumBase, INotifyCollectionChanged
	{
		private ObservableCollection<VMTypeDatumBase> mItems;

		private ReadOnlyObservableCollection<VMTypeDatumBase> mItemsReadOnly;

		private bool mInitialised;

		private bool mIsValid;

		internal new IIndexedTypeDatum Datum => base.Datum as IIndexedTypeDatum;

		public override IList<VMTypeDatumBase> Items => mItemsReadOnly;

		public override bool IsIndexed => true;

		public override bool IsValid => mIsValid;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		internal VMIndexedTypeDatum(ViewModelBase owner, IIndexedTypeDatum datum, int elementIndex = 0)
			: base(owner, datum, elementIndex)
		{
			mIsValid = true;
			mItems = new ObservableCollection<VMTypeDatumBase>();
			mItemsReadOnly = new ReadOnlyObservableCollection<VMTypeDatumBase>(mItems);
			mIsValid = true;
			Reload();
		}

		private void mItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.Modified = true;
			CollectionChanged?.Invoke(this, e);
		}

		public override bool CommitChanges(bool resetModifiedState)
		{
			bool flag = true;
			foreach (VMTypeDatumBase mItem in mItems)
			{
				if (!mItem.CommitChanges(resetModifiedState))
				{
					flag = false;
				}
			}
			if (flag)
			{
				return base.CommitChanges(resetModifiedState);
			}
			return false;
		}

		public void AddItem(ITypeDatum datum)
		{
			insertItemCore(datum, null);
		}

		public void AddItem(ITypeDescriptor selType)
		{
			InsertItem(selType, null);
		}

		private VMTypeDatumBase insertItemCore(ITypeDatum datum, VMTypeDatumBase beforeItem)
		{
			int num = -1;
			if (beforeItem != null)
			{
				for (int i = 0; i < mItems.Count; i++)
				{
					if (mItems[i].Datum == beforeItem.Datum)
					{
						num = i;
						break;
					}
				}
			}
			if (num == -1)
			{
				num = mItems.Count;
			}
			VMTypeEditContainer vMTypeEditContainer = FindTopMost<VMTypeEditContainer>();
			if (vMTypeEditContainer != null)
			{
				VMTypeDatumBase vMTypeDatumBase = vMTypeEditContainer.Factory.CreateViewModel(this, datum);
				vMTypeDatumBase.PropertyChanged += item_PropertyChanged;
				mItems.Insert(num, vMTypeDatumBase);
				if (mInitialised)
				{
					vMTypeDatumBase.Initialise();
				}
				if (vMTypeDatumBase.ValidationRequired)
				{
					base.ValidationRequired = true;
					vMTypeEditContainer.RequestRevalidation(this);
				}
				return vMTypeDatumBase;
			}
			return null;
		}

		public VMTypeDatumBase InsertItem(ITypeDatum datum, VMTypeDatumBase beforeItem)
		{
			return insertItemCore(datum, beforeItem);
		}

		private void setIsValid(bool value)
		{
			if (value != mIsValid)
			{
				mIsValid = value;
				RaisePropertyChanged("IsValid");
			}
		}

		private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
				bool isValid = true;
				foreach (VMTypeDatumBase item in Items)
				{
					if (!item.IsValid)
					{
						isValid = false;
						break;
					}
				}
				setIsValid(isValid);
			}
			else
			{
				if (!(e.PropertyName == "ValidationRequired"))
				{
					return;
				}
				bool validationRequired = false;
				foreach (VMTypeDatumBase item2 in Items)
				{
					if (item2.ValidationRequired)
					{
						validationRequired = true;
						break;
					}
				}
				base.ValidationRequired = validationRequired;
			}
		}

		public VMTypeDatumBase InsertItem(ITypeDescriptor selType, VMTypeDatumBase beforeItem)
		{
			int num = -1;
			if (beforeItem != null)
			{
				num = Datum.IndexOf(beforeItem.Datum);
			}
			if (num == -1)
			{
				num = Datum.Count;
			}
			ITypeDatum datum = Datum.Insert(num, selType);
			return insertItemCore(datum, beforeItem);
		}

		public bool RemoveItem(VMTypeDatumBase typeDatum)
		{
			for (int i = 0; i < mItems.Count; i++)
			{
				VMTypeDatumBase vMTypeDatumBase = mItems[i];
				if (vMTypeDatumBase.Datum != typeDatum.Datum || !Datum.Remove(typeDatum.Datum))
				{
					continue;
				}
				if (vMTypeDatumBase.IsSelected)
				{
					if (i < mItems.Count - 1)
					{
						mItems[i + 1].IsSelected = true;
					}
					else if (i > 0)
					{
						mItems[i - 1].IsSelected = true;
					}
					else
					{
						VMProperty vMProperty = FindAncestor<VMProperty>();
						if (vMProperty.PropertyContent == this)
						{
							vMProperty.IsSelected = true;
						}
					}
				}
				vMTypeDatumBase.PropertyChanged -= item_PropertyChanged;
				mItems.Remove(vMTypeDatumBase);
				return true;
			}
			return false;
		}

		public override void Dispose()
		{
			mItems.CollectionChanged -= mItems_CollectionChanged;
			clearItems();
			base.Dispose();
		}

		public override void Reload()
		{
			mItems.CollectionChanged -= mItems_CollectionChanged;
			clearItems();
			addItems();
			mItems.CollectionChanged += mItems_CollectionChanged;
			base.Reload();
		}

		private void addItems()
		{
			IIndexedTypeDatum datum = Datum;
			foreach (ITypeDatum item in datum.Items)
			{
				AddItem(item);
			}
		}

		private void clearItems()
		{
			foreach (VMTypeDatumBase mItem in mItems)
			{
				mItem.PropertyChanged -= item_PropertyChanged;
				mItem.Dispose();
			}
			mItems.Clear();
		}

		internal override void Initialise()
		{
			if (!mInitialised)
			{
				mInitialised = true;
				foreach (VMTypeDatumBase item in Items)
				{
					item.Initialise();
				}
			}
			base.Initialise();
		}
	}
}
