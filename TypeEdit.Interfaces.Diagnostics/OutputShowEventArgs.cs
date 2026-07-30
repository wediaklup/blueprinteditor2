using System;

namespace TypeEdit.Interfaces.Diagnostics
{
	public class OutputShowEventArgs : EventArgs
	{
		public string Filter { get; private set; }

		public OutputShowEventArgs(string filter)
		{
			Filter = filter;
		}
	}
}
