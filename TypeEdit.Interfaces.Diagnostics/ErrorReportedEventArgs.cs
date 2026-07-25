using System;

namespace TypeEdit.Interfaces.Diagnostics;

public class ErrorReportedEventArgs : EventArgs
{
	public IErrorItem ErrorItem { get; private set; }

	public ErrorReportedEventArgs(IErrorItem errorItem)
	{
		ErrorItem = errorItem;
	}
}
