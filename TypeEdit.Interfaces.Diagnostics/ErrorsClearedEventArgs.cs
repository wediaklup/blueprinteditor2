using System;
using TypeEdit.Interfaces.Editing;

namespace TypeEdit.Interfaces.Diagnostics;

public class ErrorsClearedEventArgs : EventArgs
{
	public string Filter { get; private set; }

	public OutputMessageTarget Target { get; private set; }

	public IDocument Document { get; set; }

	public ErrorsClearedEventArgs(string filter, OutputMessageTarget target, IDocument document)
	{
		Filter = filter;
		Target = target;
		Document = document;
	}
}
