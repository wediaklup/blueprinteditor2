using System;
using System.Collections.Generic;
using System.IO;
using TypeEdit.Interfaces.UI;

namespace S9BEditor;

internal class LuaScriptFileTypeInfo : IEditableFileTypeInfo, IFileTypeInfo, IFileTypeInfoProvider
{
	private static string[] mActions = new string[1] { "Export" };

	public string Name => "Empty LUA Script";

	public string FileCategory => "LUA Script";

	public string FileExtension => ".lua";

	public string DefaultFileName => "LuaScript1.lua";

	public IEnumerable<IFileTypeInfo> FileTypes
	{
		get
		{
			yield return this;
		}
	}

	public string[] CustomActions => mActions;

	public bool TryCreateFile(string fileName)
	{
		try
		{
			File.Create(fileName)?.Close();
			return true;
		}
		catch (Exception e)
		{
			AppServices.ErrorManager.ErrorMessage("Failed to create file \"" + Path.GetFileName(fileName) + "\"", e);
		}
		return false;
	}

	public bool ValidFileLocation(string rootDirectory, string relativePath)
	{
		string[] array = relativePath.Split(new char[1] { '\\' });
		if (array.Length >= 2)
		{
			return true;
		}
		return false;
	}

	public bool MatchesFileType(string sourceDirectory, string relativePath)
	{
		try
		{
			return Path.GetExtension(Path.Combine(sourceDirectory, relativePath)).Equals(FileExtension, StringComparison.CurrentCultureIgnoreCase);
		}
		catch (Exception)
		{
			return false;
		}
	}
}
