using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Instancing;

namespace S9BEditor.ViewModels
{
	internal class VMReferenceTypeDatum : VMTypeDatumBase
	{
		private IReferenceProxyTypeDatum mDatumAsReference;

		private int mElementIndex;

		private VMTypeEditContainer mRoot;

		private ITypeDatum mCurrentTargetDatum;

		private VMReferenceItem mSelectedItem;

		private ObservableCollection<VMReferenceItem> mRefItems;

		private ReadOnlyObservableCollection<VMReferenceItem> mRefItemsReadOnly;

		private VMIndexedTypeDatum mReferenceTargetViewModel;

		internal new IReferenceProxyTypeDatum Datum => base.Datum as IReferenceProxyTypeDatum;

		public VMReferenceItem SelectedItem
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
					ITypeDatum typeDatum = mCurrentTargetDatum;
					if (mSelectedItem == null)
					{
						mCurrentTargetDatum = null;
					}
					else
					{
						mCurrentTargetDatum = mSelectedItem.TargetDatum;
					}
					if (typeDatum != mCurrentTargetDatum)
					{
						base.Modified = true;
						base.ValidationRequired = true;
						FindTopMost<VMTypeEditContainer>()?.RequestRevalidation(this);
					}
					RaisePropertyChanged("SelectedItem");
				}
			}
		}

		public ReadOnlyObservableCollection<VMReferenceItem> PossibleReferences
		{
			get
			{
				return mRefItemsReadOnly;
			}
			private set
			{
				if (mRefItemsReadOnly != value)
				{
					mRefItemsReadOnly = value;
					RaisePropertyChanged("PossibleReferences");
				}
			}
		}

		public override bool IsValid => base.Errors == null;

		internal VMReferenceTypeDatum(ViewModelBase owner, IReferenceProxyTypeDatum datum, int elementIndex = 0)
			: base(owner, datum, elementIndex)
		{
			mDatumAsReference = datum;
			mElementIndex = elementIndex;
			mRoot = FindTopMost<VMTypeEditContainer>();
			if (mRoot != null)
			{
				mRoot.ReferenceTargetRegistered += root_ReferenceTargetRegistered;
			}
			mRefItems = new ObservableCollection<VMReferenceItem>();
			Reload();
		}

		public override void Reload()
		{
			if (mDatumAsReference != null)
			{
				mCurrentTargetDatum = mDatumAsReference.GetReferencedDatum(mElementIndex);
			}
			refreshPossibleReferences();
			base.Reload();
		}

		internal override void Initialise()
		{
			VMProperty referenceTargetViewModel = mRoot.GetReferenceTargetViewModel(Datum.GetReferenceTarget());
			setReferenceTargetViewModel(referenceTargetViewModel);
			base.Initialise();
		}

		private void setReferenceTargetViewModel(VMProperty target)
		{
			if (mReferenceTargetViewModel != null)
			{
				mReferenceTargetViewModel.CollectionChanged -= referenceTargetViewModel_CollectionChanged;
			}
			if (target != null)
			{
				mReferenceTargetViewModel = target.PropertyContent as VMIndexedTypeDatum;
				mReferenceTargetViewModel.CollectionChanged += referenceTargetViewModel_CollectionChanged;
			}
			refreshPossibleReferences();
		}

		private void root_ReferenceTargetRegistered(object sender, ReferenceTargetEventArgs e)
		{
			IIndexedTypeDatum referenceTarget = Datum.GetReferenceTarget();
			if (e.ReferenceTarget.PropertyContent.Datum == referenceTarget)
			{
				setReferenceTargetViewModel(e.ReferenceTarget);
			}
		}

		private void referenceTargetViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			refreshPossibleReferences();
		}

		private void refreshPossibleReferences()
		{
			ITypeDatum typeDatum = mCurrentTargetDatum;
			clearReferenceItems();
			if (mReferenceTargetViewModel == null)
			{
				return;
			}
			bool flag = false;
			VMReferenceItem vMReferenceItem = new VMReferenceItem(this, null);
			mRefItems.Add(vMReferenceItem);
			foreach (VMTypeDatumBase item in mReferenceTargetViewModel.Items)
			{
				VMReferenceItem vMReferenceItem2 = new VMReferenceItem(this, item);
				mRefItems.Add(vMReferenceItem2);
				if (item.Datum == typeDatum)
				{
					flag = true;
					SelectedItem = vMReferenceItem2;
				}
			}
			if (!flag)
			{
				SelectedItem = vMReferenceItem;
			}
			PossibleReferences = new ReadOnlyObservableCollection<VMReferenceItem>(mRefItems);
		}

		private void clearReferenceItems()
		{
			mRefItemsReadOnly = null;
			foreach (VMReferenceItem mRefItem in mRefItems)
			{
				mRefItem.Dispose();
			}
			mRefItems.Clear();
		}

		public override bool CommitChanges(bool resetModifiedState)
		{
			base.ValidationRequired = false;
			string text = ((mSelectedItem == null || !mSelectedItem.HasName) ? "" : mSelectedItem.Name);
			if (text != mDatumAsReference.GetValue(mElementIndex) && !mDatumAsReference.TrySetValue(text, mElementIndex))
			{
				SetCurrentError(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid value.", new object[1] { text }));
				RaisePropertyChanged("IsValid");
				return false;
			}
			SetCurrentError(null);
			RaisePropertyChanged("IsValid");
			return base.CommitChanges(resetModifiedState);
		}

		private VMIndexedTypeDatum findReferenceTargetVM(VMClassTypeDatum topMostClass, IIndexedTypeDatum refTargetDatum)
		{
			VMIndexedTypeDatum vMIndexedTypeDatum = null;
			foreach (VMClassGroup group in topMostClass.Groups)
			{
				foreach (VMProperty property in group.Properties)
				{
					if (property.PropertyContent is VMIndexedTypeDatum { Datum: var datum } vMIndexedTypeDatum2 && datum == refTargetDatum)
					{
						return vMIndexedTypeDatum2;
					}
					if (property.PropertyContent is VMClassTypeDatum topMostClass2)
					{
						vMIndexedTypeDatum = findReferenceTargetVM(topMostClass2, refTargetDatum);
						if (vMIndexedTypeDatum != null)
						{
							return vMIndexedTypeDatum;
						}
					}
				}
			}
			return null;
		}

		public override void Dispose()
		{
			if (mReferenceTargetViewModel != null)
			{
				mReferenceTargetViewModel.CollectionChanged -= referenceTargetViewModel_CollectionChanged;
			}
			if (mRoot != null)
			{
				mRoot.ReferenceTargetRegistered -= root_ReferenceTargetRegistered;
			}
			clearReferenceItems();
			base.Dispose();
		}
	}
}
