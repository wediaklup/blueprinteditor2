using System;

namespace TypeEdit.Interfaces.Diagnostics
{
	public class OutputReportedEventArgs : EventArgs
	{
		public string Filter { get; private set; }

		public string Line { get; private set; }

		public OutputReportedEventArgs(string filter, string line)
		{
			Filter = filter;
			Line = line;
		}
	}
}
