using System.Collections.Generic;
using TypeEdit.Base;
using TypeEdit.Interfaces.UI;

namespace S9BEditor.ViewModels
{
	public class VMFileMenuItem : VMMenuItem
	{
		public new IFileActionItem ActionItem => (IFileActionItem)base.ActionItem;

		public string SourceDirectory { get; private set; }

		public string RelativePath { get; private set; }

		public bool IsDirectory { get; private set; }

		public VMFileMenuItem(IFileActionItem actionItem)
			: base(actionItem)
		{
			VMFileMenuItem vMFileMenuItem = this;
			IEnumerable<IFileActionItem> items = actionItem.Items;
			if (items != null)
			{
				foreach (IFileActionItem item2 in items)
				{
					VMFileMenuItem item = new VMFileMenuItem(item2);
					base.Items.Add(item);
				}
			}
			if (base.Items.Count > 0)
			{
				base.Command = new DelegateCommand(delegate
				{
				}, delegate
				{
					bool flag = false;
					foreach (VMMenuItem item3 in base.Items)
					{
						if (!item3.IsSeparator)
						{
							flag = item3.Command.CanExecute(null);
							if (flag)
							{
								break;
							}
						}
					}
					return flag;
				});
			}
			else
			{
				base.Command = new DelegateCommand(delegate
				{
					actionItem.PerformAction(vMFileMenuItem.SourceDirectory, vMFileMenuItem.RelativePath, vMFileMenuItem.IsDirectory);
				}, () => actionItem.CanPerformAction(vMFileMenuItem.SourceDirectory, vMFileMenuItem.RelativePath, vMFileMenuItem.IsDirectory));
			}
		}

		public void Update(string sourceDirectory, string relativePath, bool isDirectory)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (sourceDirectory != SourceDirectory)
			{
				SourceDirectory = sourceDirectory;
				flag = true;
			}
			if (relativePath != RelativePath)
			{
				RelativePath = relativePath;
				flag2 = true;
			}
			if (isDirectory != IsDirectory)
			{
				IsDirectory = isDirectory;
				flag3 = true;
			}
			if (flag)
			{
				RaisePropertyChanged("SourceDirectory");
			}
			if (flag2)
			{
				RaisePropertyChanged("RelativePath");
			}
			if (flag3)
			{
				RaisePropertyChanged("IsDirectory");
			}
			foreach (VMMenuItem item in base.Items)
			{
				if (item is VMFileMenuItem vMFileMenuItem)
				{
					vMFileMenuItem.Update(sourceDirectory, relativePath, isDirectory);
				}
			}
			UpdateText();
		}

		public override void UpdateText()
		{
			base.Text = ActionItem.GetDisplayText(SourceDirectory, RelativePath, IsDirectory);
		}
	}
}
