using System;

namespace S9BEditor;

internal class FileManagerEventArgs : EventArgs
{
	public string FileName { get; private set; }

	public bool Handled { get; set; }

	public FileManagerEventArgs(string fileName)
	{
		FileName = fileName;
	}
}
