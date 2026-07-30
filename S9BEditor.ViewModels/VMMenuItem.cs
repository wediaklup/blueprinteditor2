using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using TypeEdit.Base;
using TypeEdit.Interfaces.UI;

namespace S9BEditor.ViewModels
{
	public abstract class VMMenuItem : ViewModelBase
	{
		private bool mIsSeparator;

		private ICommand mCommand;

		private Uri mImageUri;

		private string mInputGestureText;

		private string mText;

		public bool IsSeparator
		{
			get
			{
				return mIsSeparator;
			}
			private set
			{
				if (mIsSeparator != value)
				{
					mIsSeparator = value;
				}
			}
		}

		public ICommand Command
		{
			get
			{
				return mCommand;
			}
			set
			{
				if (mCommand != value)
				{
					mCommand = value;
					RaisePropertyChanged("Command");
				}
			}
		}

		public Uri ImageUri
		{
			get
			{
				return mImageUri;
			}
			set
			{
				if (mImageUri != value)
				{
					mImageUri = value;
					RaisePropertyChanged("ImageUri");
				}
			}
		}

		public string InputGestureText
		{
			get
			{
				return mInputGestureText;
			}
			set
			{
				if (mInputGestureText != value)
				{
					mInputGestureText = value;
					RaisePropertyChanged("InputGestureText");
				}
			}
		}

		public string Text
		{
			get
			{
				return mText;
			}
			protected set
			{
				if (mText != value)
				{
					mText = value;
					RaisePropertyChanged("Text");
				}
			}
		}

		public IActionItem ActionItem { get; private set; }

		public ObservableCollection<VMMenuItem> Items { get; private set; }

		public VMMenuItem(IActionItem actionItem)
			: base(null)
		{
			Items = new ObservableCollection<VMMenuItem>();
			ActionItem = actionItem;
			if (ActionItem is IWPFActionItem iWPFActionItem)
			{
				foreach (InputGesture wPFInputGesture in iWPFActionItem.GetWPFInputGestures())
				{
					KeyGesture val = (KeyGesture)(object)((wPFInputGesture is KeyGesture) ? wPFInputGesture : null);
					if (val != null)
					{
						InputGestureText = val.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
						break;
					}
				}
			}
			if (actionItem is IUriProviderActionItem uriProviderActionItem)
			{
				ImageUri = uriProviderActionItem.GetResourceURI(ActionItemResourceType.MenuItemIcon);
			}
			IsSeparator = actionItem is ISeparatorActionItem;
		}

		public abstract void UpdateText();
	}
}
