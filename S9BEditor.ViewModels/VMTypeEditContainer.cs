using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Instancing;

namespace S9BEditor.ViewModels
{
	public class VMTypeEditContainer : ViewModelBase
	{
		private bool mModified;

		private List<VMTypeEditComponent> mAllViewModels;

		private List<VMProperty> mReferenceTargets;

		private TypeDatumViewModelFactory mFactory;

		private string mProvider;

		private string mProduct;

		private string mSourceDirectory;

		private string mRelativePath;

		private ObservableCollection<VMTypeEditComponent> mSelectedItems;

		public VMTypeDatumBase Datum
		{
			get
			{
				ReadOnlyObservableCollection<VMTypeDatumBase> items = Items;
				if (items != null && items.Count > 0)
				{
					return items[0];
				}
				return null;
			}
			private set
			{
				ReadOnlyObservableCollection<VMTypeDatumBase> items = Items;
				VMTypeDatumBase vMTypeDatumBase = null;
				if (items != null && items.Count > 0)
				{
					vMTypeDatumBase = items[0];
				}
				if (vMTypeDatumBase != value)
				{
					if (vMTypeDatumBase != null)
					{
						vMTypeDatumBase.PropertyChanged -= Datum_PropertyChanged;
					}
					vMTypeDatumBase = value;
					vMTypeDatumBase.PropertyChanged += Datum_PropertyChanged;
					ObservableCollection<VMTypeDatumBase> observableCollection = new ObservableCollection<VMTypeDatumBase>();
					observableCollection.Add(vMTypeDatumBase);
					Items = new ReadOnlyObservableCollection<VMTypeDatumBase>(observableCollection);
					RaisePropertyChanged("Datum");
					RaisePropertyChanged("Name");
					RaisePropertyChanged("Items");
				}
			}
		}

		public ReadOnlyObservableCollection<VMTypeDatumBase> Items { get; private set; }

		public string Name
		{
			get
			{
				if (Datum == null)
				{
					return null;
				}
				return Datum.Name;
			}
		}

		public bool Modified
		{
			get
			{
				return mModified;
			}
			private set
			{
				if (mModified != value)
				{
					mModified = value;
					RaisePropertyChanged("Modified");
				}
			}
		}

		public bool ValidationRequired => Datum.ValidationRequired;

		public bool IsValid => Datum.IsValid;

		internal TypeDatumViewModelFactory Factory => mFactory;

		public string Provider
		{
			get
			{
				return mProvider;
			}
			private set
			{
				if (mProvider != value)
				{
					mProvider = value;
					RaisePropertyChanged("Provider");
				}
			}
		}

		public string Product
		{
			get
			{
				return mProduct;
			}
			private set
			{
				if (mProduct != value)
				{
					mProduct = value;
					RaisePropertyChanged("Product");
				}
			}
		}

		public string SourceDirectory
		{
			get
			{
				return mSourceDirectory;
			}
			private set
			{
				if (mSourceDirectory != value)
				{
					mSourceDirectory = value;
					RaisePropertyChanged("SourceDirectory");
				}
			}
		}

		public string RelativePath
		{
			get
			{
				return mRelativePath;
			}
			private set
			{
				if (mRelativePath != value)
				{
					mRelativePath = value;
					RaisePropertyChanged("RelativePath");
				}
			}
		}

		public ReadOnlyObservableCollection<VMTypeEditComponent> SelectedItems { get; private set; }

		public event EventHandler ValidationRequested;

		public event EventHandler<ReferenceTargetEventArgs> ReferenceTargetRegistered;

		public event EventHandler<ReferenceTargetEventArgs> ReferenceTargetUnregistering;

		public VMTypeEditContainer(ITypeDatum datum, string provider, string product, string relativePath, string sourceDirectory)
			: base(null)
		{
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
			GC.WaitForFullGCComplete();
			mFactory = new TypeDatumViewModelFactory();
			mReferenceTargets = new List<VMProperty>();
			mAllViewModels = new List<VMTypeEditComponent>();
			mSelectedItems = new ObservableCollection<VMTypeEditComponent>();
			SelectedItems = new ReadOnlyObservableCollection<VMTypeEditComponent>(mSelectedItems);
			Provider = provider;
			Product = product;
			SourceDirectory = sourceDirectory;
			RelativePath = relativePath;
			Datum = Factory.CreateViewModel(this, datum);
			Datum.Initialise();
		}

		private void Datum_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name")
			{
				RaisePropertyChanged("Name");
			}
			else if (e.PropertyName == "Modified")
			{
				Modified |= Datum.Modified;
			}
			else if (e.PropertyName == "IsValid")
			{
				RaisePropertyChanged("IsValid");
			}
			else if (e.PropertyName == "ValidationRequired")
			{
				RaisePropertyChanged("ValidationRequired");
			}
		}

		public void RequestRevalidation(VMTypeEditComponent component)
		{
			ValidationRequested?.Invoke(this, EventArgs.Empty);
		}

		public bool CommitChanges(bool resetModified)
		{
			if (Datum.CommitChanges(resetModified))
			{
				if (resetModified)
				{
					Modified = false;
				}
				return true;
			}
			return false;
		}

		public override void Dispose()
		{
			Datum.PropertyChanged -= Datum_PropertyChanged;
			Datum.Dispose();
			mFactory.Dispose();
			base.Dispose();
		}

		internal void RegisterViewModel(VMTypeEditComponent viewModel)
		{
			mAllViewModels.Add(viewModel);
			viewModel.PropertyChanged += viewModel_PropertyChanged;
		}

		private void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!(e.PropertyName == "IsSelected") || !(sender is VMTypeEditComponent vMTypeEditComponent))
			{
				return;
			}
			if (vMTypeEditComponent.IsSelected)
			{
				if (!mSelectedItems.Contains(vMTypeEditComponent))
				{
					mSelectedItems.Add(vMTypeEditComponent);
				}
			}
			else
			{
				mSelectedItems.Remove(vMTypeEditComponent);
			}
		}

		internal void UnregisterViewModel(VMTypeEditComponent viewModel)
		{
			mSelectedItems.Remove(viewModel);
			viewModel.PropertyChanged -= viewModel_PropertyChanged;
			mAllViewModels.Remove(viewModel);
		}

		internal void RegisterReferenceTarget(VMProperty refTargetProp)
		{
			mReferenceTargets.Add(refTargetProp);
			ReferenceTargetRegistered?.Invoke(this, new ReferenceTargetEventArgs(refTargetProp));
		}

		internal void UnregisterReferenceTarget(VMProperty refTargetProp)
		{
			if (mReferenceTargets.Contains(refTargetProp))
			{
				ReferenceTargetUnregistering?.Invoke(this, new ReferenceTargetEventArgs(refTargetProp));
				mReferenceTargets.Remove(refTargetProp);
			}
		}

		internal VMProperty GetReferenceTargetViewModel(IIndexedTypeDatum targetToFind)
		{
			foreach (VMProperty mReferenceTarget in mReferenceTargets)
			{
				if (mReferenceTarget.PropertyContent.Datum == targetToFind)
				{
					return mReferenceTarget;
				}
			}
			return null;
		}
	}
}
