using TypeEdit.Base;

namespace S9BEditor.ViewModels;

internal class VMTypeEditComponent : ViewModelBase
{
	private string[] mErrors;

	private string[] mWarnings;

	private bool mValidationRequired;

	private bool mBringIntoViewRequested;

	private bool mIsExpanded;

	public virtual bool IsValid => true;

	public string[] Errors
	{
		get
		{
			return mErrors;
		}
		set
		{
			if (mErrors != value)
			{
				mErrors = value;
				RaisePropertyChanged("Errors");
			}
		}
	}

	public string[] Warnings
	{
		get
		{
			return mWarnings;
		}
		set
		{
			if (mWarnings != value)
			{
				mWarnings = value;
				RaisePropertyChanged("Warnings");
			}
		}
	}

	public bool ValidationRequired
	{
		get
		{
			return mValidationRequired;
		}
		protected set
		{
			if (mValidationRequired != value)
			{
				mValidationRequired = value;
				RaisePropertyChanged("ValidationRequired");
			}
		}
	}

	public bool BringIntoViewRequested
	{
		get
		{
			return mBringIntoViewRequested;
		}
		set
		{
			mBringIntoViewRequested = value;
			RaisePropertyChanged("BringIntoViewRequested");
		}
	}

	public bool IsExpanded
	{
		get
		{
			return mIsExpanded;
		}
		set
		{
			if (mIsExpanded != value)
			{
				mIsExpanded = value;
				RaisePropertyChanged("IsExpanded");
			}
		}
	}

	public VMTypeEditComponent(ViewModelBase owner)
		: base(owner)
	{
	}

	internal virtual void Initialise()
	{
		FindTopMost<VMTypeEditContainer>()?.RegisterViewModel(this);
	}

	public override void Dispose()
	{
		FindTopMost<VMTypeEditContainer>()?.UnregisterViewModel(this);
		base.Dispose();
	}

	public virtual void BringIntoView()
	{
		ExpandToThis();
		BringIntoViewRequested = true;
	}

	private void ExpandToThis()
	{
		if (base.Owner != null && base.Owner is VMTypeEditComponent vMTypeEditComponent)
		{
			vMTypeEditComponent.ExpandToThis();
		}
		IsExpanded = true;
	}
}
