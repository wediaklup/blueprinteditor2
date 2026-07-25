using System;
using TypeEdit.Interfaces;

namespace S9BEditor;

internal class FileManager : IFileManager
{
	public event EventHandler<FileManagerEventArgs> RequestShowInTree;

	public event EventHandler<FileManagerEventArgs> RequestOpenFileInternally;

	public event EventHandler<FileManagerEventArgs> RequestOpenFileExternally;

	public bool ShowInTree(string fileName)
	{
		FileManagerEventArgs e = new FileManagerEventArgs(fileName);
		OnRequestShowInTree(e);
		return e.Handled;
	}

	public OpenFileResult OpenFile(string fileName, bool allowExternalApp = true)
	{
		FileManagerEventArgs e = new FileManagerEventArgs(fileName);
		OnRequestOpenFileInternally(e);
		if (allowExternalApp && !e.Handled)
		{
			OnRequestOpenFileExternally(e);
			if (e.Handled)
			{
				return OpenFileResult.OpenedExternally;
			}
		}
		if (!e.Handled)
		{
			return OpenFileResult.Failed;
		}
		return OpenFileResult.OpenedInternally;
	}

	protected virtual void OnRequestShowInTree(FileManagerEventArgs e)
	{
		EventHandler<FileManagerEventArgs> requestShowInTree = RequestShowInTree;
		if (requestShowInTree == null)
		{
			return;
		}
		Delegate[] invocationList = requestShowInTree.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			EventHandler<FileManagerEventArgs> eventHandler = (EventHandler<FileManagerEventArgs>)invocationList[i];
			eventHandler(this, e);
			if (e.Handled)
			{
				break;
			}
		}
	}

	protected virtual void OnRequestOpenFileInternally(FileManagerEventArgs e)
	{
		EventHandler<FileManagerEventArgs> requestOpenFileInternally = RequestOpenFileInternally;
		if (requestOpenFileInternally == null)
		{
			return;
		}
		Delegate[] invocationList = requestOpenFileInternally.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			EventHandler<FileManagerEventArgs> eventHandler = (EventHandler<FileManagerEventArgs>)invocationList[i];
			eventHandler(this, e);
			if (e.Handled)
			{
				break;
			}
		}
	}

	protected virtual void OnRequestOpenFileExternally(FileManagerEventArgs e)
	{
		EventHandler<FileManagerEventArgs> requestOpenFileExternally = RequestOpenFileExternally;
		if (requestOpenFileExternally == null)
		{
			return;
		}
		Delegate[] invocationList = requestOpenFileExternally.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			EventHandler<FileManagerEventArgs> eventHandler = (EventHandler<FileManagerEventArgs>)invocationList[i];
			eventHandler(this, e);
			if (e.Handled)
			{
				break;
			}
		}
	}
}
