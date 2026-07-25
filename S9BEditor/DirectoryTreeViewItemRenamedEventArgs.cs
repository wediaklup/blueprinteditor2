using System;

namespace S9BEditor;

internal class DirectoryTreeViewItemRenamedEventArgs : EventArgs
{
	public string NewFileName { get; set; }

	public DirectoryTreeViewItemRenamedEventArgs(string newFileName)
	{
		NewFileName = newFileName;
	}
}
