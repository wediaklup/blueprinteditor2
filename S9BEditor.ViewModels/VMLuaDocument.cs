using TypeEdit.Base;

namespace S9BEditor.ViewModels;

internal class VMLuaDocument : ViewModelBase
{
	private bool mModified;

	private string mText;

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
			}
		}
	}

	public string Text
	{
		get
		{
			return mText;
		}
		set
		{
			if (mText != value)
			{
				mText = value;
				RaisePropertyChanged("Text");
				Modified = true;
			}
		}
	}

	public VMLuaDocument()
		: base(null)
	{
	}
}
