using System;
using System.IO;
using TypeEdit.Interfaces.Data;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.Data.Instancing;
using TypeEdit.Interfaces.UI;

namespace S9BEditor;

internal class TypeEditFileTypeInfo : IEditableFileTypeInfo, IFileTypeInfo
{
	private IClassTypeDescriptor mClassType;

	public string Name => S9BEUtil.CamelCaseToNormal(S9BEUtil.RemoveNamespaces(mClassType.Name));

	public string FileCategory { get; private set; }

	public string FileExtension => ".xml";

	public string DefaultFileName => S9BEUtil.TypeNameToFriendly(mClassType.Name) + "1.xml";

	public TypeEditFileTypeInfo(IClassTypeDescriptor classType, string category)
	{
		mClassType = classType;
		FileCategory = category;
	}

	public bool TryCreateFile(string fileName)
	{
		try
		{
			ITypeEditSchema typeEditSchema = mClassType.GetTypeEditSchema();
			ITypeDatum instanceOfType = mClassType.CreateDatum(null);
			typeEditSchema.SaveDataToFile(instanceOfType, fileName);
			return true;
		}
		catch (Exception e)
		{
			AppServices.ErrorManager.ErrorMessage("Failed to create file \"" + Path.GetFileName(fileName), e);
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
			string text = Path.Combine(sourceDirectory, relativePath);
			if (Path.GetExtension(text).Equals(FileExtension, StringComparison.CurrentCultureIgnoreCase))
			{
				ITypeEditSchema typeEditSchema = mClassType.GetTypeEditSchema();
				ITypeDescriptor rootTypeFromFile = typeEditSchema.GetRootTypeFromFile(text);
				return rootTypeFromFile == mClassType;
			}
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
