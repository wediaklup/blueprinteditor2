using System;

namespace S9BEditor;

internal class DirectoryTreeViewFileSelectedArgs : EventArgs
{
	public string FileName { get; private set; }

	public DirectoryTreeViewFileSelectedArgs(string fileName)
	{
		FileName = fileName;
	}
}
