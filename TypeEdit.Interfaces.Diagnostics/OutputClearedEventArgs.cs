using System;

namespace TypeEdit.Interfaces.Diagnostics;

public class OutputClearedEventArgs : EventArgs
{
	public string Filter { get; private set; }

	public OutputClearedEventArgs(string filter)
	{
		Filter = filter;
	}
}
