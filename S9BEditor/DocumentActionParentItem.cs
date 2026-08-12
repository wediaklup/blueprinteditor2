using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TypeEdit.Interfaces.Editing;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	internal class DocumentActionParentItem : IDocumentActionItem, IActionItem
	{
		IEnumerable<IDocumentActionItem> IDocumentActionItem.Items => Items;

		public List<IDocumentActionItem> Items { get; private set; }

		public string DisplayText { get; set; }

		public DocumentActionParentItem()
		{
			Items = new List<IDocumentActionItem>();
		}

		public string GetDisplayText(IDocument document)
		{
			if (document != null && DisplayText != null)
			{
				return string.Format(CultureInfo.CurrentCulture, DisplayText, new object[1] { Path.GetFileName(document.FileName) });
			}
			return string.Empty;
		}

		public bool CanPerformAction(IDocument document)
		{
			foreach (IDocumentActionItem item in Items)
			{
				if (item.CanPerformAction(document))
				{
					return true;
				}
			}
			return false;
		}

		public void PerformAction(IDocument document)
		{
		}
	}
}
