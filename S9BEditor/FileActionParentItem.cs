using System.Collections.Generic;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	internal class FileActionParentItem : IFileActionItem, IActionItem
	{
		IEnumerable<IFileActionItem> IFileActionItem.Items => Items.ToArray();

		public List<IFileActionItem> Items { get; private set; }

		public string DisplayText { get; set; }

		public FileActionParentItem()
		{
			Items = new List<IFileActionItem>();
		}

		string IFileActionItem.GetDisplayText(string sourceDirectory, string relativePath, bool isDirectory)
		{
			return DisplayText;
		}

		bool IFileActionItem.CanPerformAction(string sourceDirectory, string relativePath, bool isDirectory)
		{
			foreach (IFileActionItem item in Items)
			{
				if (item.CanPerformAction(sourceDirectory, relativePath, isDirectory))
				{
					return true;
				}
			}
			return false;
		}

		void IFileActionItem.PerformAction(string sourceDirectory, string relativePath, bool isDirectory)
		{
		}
	}
}
