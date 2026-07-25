using System.Collections.Generic;
using TypeEdit.Base;
using TypeEdit.Interfaces.Editing;
using TypeEdit.Interfaces.UI;

namespace S9BEditor.ViewModels;

public class VMDocumentMenuItem : VMMenuItem
{
	private IDocument mDocument;

	public new IDocumentActionItem ActionItem => (IDocumentActionItem)base.ActionItem;

	public IDocument Document
	{
		get
		{
			return mDocument;
		}
		private set
		{
			if (mDocument != value)
			{
				mDocument = value;
				RaisePropertyChanged("Document");
			}
		}
	}

	public VMDocumentMenuItem(IDocumentActionItem actionItem)
		: base(actionItem)
	{
		VMDocumentMenuItem vMDocumentMenuItem = this;
		IEnumerable<IDocumentActionItem> items = actionItem.Items;
		if (items != null)
		{
			foreach (IDocumentActionItem item2 in items)
			{
				VMDocumentMenuItem item = new VMDocumentMenuItem(item2);
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
				actionItem.PerformAction(vMDocumentMenuItem.Document);
			}, () => actionItem.CanPerformAction(vMDocumentMenuItem.Document));
		}
	}

	public void Update(IDocument document)
	{
		Document = document;
		foreach (VMMenuItem item in base.Items)
		{
			if (item is VMDocumentMenuItem vMDocumentMenuItem)
			{
				vMDocumentMenuItem.Update(document);
			}
		}
		UpdateText();
	}

	public override void UpdateText()
	{
		base.Text = ActionItem.GetDisplayText(Document);
	}
}
