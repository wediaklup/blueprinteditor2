using System;
using System.Collections;

namespace S9BEditor;

internal class OverflowStatesChangedEventArgs : EventArgs
{
	public ICollection ChangedItems { get; private set; }

	public OverflowStatesChangedEventArgs(ICollection changedItems)
	{
		ChangedItems = changedItems;
	}
}
