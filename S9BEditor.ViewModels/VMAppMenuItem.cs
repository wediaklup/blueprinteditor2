using System.Collections.Generic;
using TypeEdit.Base;
using TypeEdit.Interfaces.UI;

namespace S9BEditor.ViewModels
{
	public class VMAppMenuItem : VMMenuItem
	{
		public new IApplicationActionItem ActionItem => (IApplicationActionItem)base.ActionItem;

		public VMAppMenuItem(IApplicationActionItem actionItem)
			: base(actionItem)
		{
			VMAppMenuItem vMAppMenuItem = this;
			IEnumerable<IApplicationActionItem> items = actionItem.Items;
			if (items != null)
			{
				foreach (IApplicationActionItem item2 in items)
				{
					VMAppMenuItem item = new VMAppMenuItem(item2);
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
					actionItem.PerformAction();
				}, () => actionItem.CanPerformAction());
			}
		}

		public void Update()
		{
			foreach (VMMenuItem item in base.Items)
			{
				if (item is VMAppMenuItem vMAppMenuItem)
				{
					vMAppMenuItem.Update();
				}
			}
			UpdateText();
		}

		public override void UpdateText()
		{
			base.Text = ActionItem.GetDisplayText();
		}
	}
}
