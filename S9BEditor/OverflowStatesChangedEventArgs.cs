using System;
using System.Collections;

namespace S9BEditor
{
	public class OverflowStatesChangedEventArgs : EventArgs
	{
		public ICollection ChangedItems { get; private set; }

		public OverflowStatesChangedEventArgs(ICollection changedItems)
		{
			ChangedItems = changedItems;
		}
	}
}
