using System;
using System.Collections.Generic;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.UI;

namespace S9BEditor;

internal class TypeEditFileTypeInfoProvider : IFileTypeInfoProvider
{
	private List<TypeEditFileTypeInfo> mFileTypeInfos;

	public IEnumerable<IFileTypeInfo> FileTypes => mFileTypeInfos;

	public TypeEditFileTypeInfoProvider()
	{
		AppServices.TypeEditSchema.Initialised += TypeEditSchema_Initialised;
		if (AppServices.TypeEditSchema.IsInitialised)
		{
			onTypeEditSchemaInitialised();
		}
	}

	private void TypeEditSchema_Initialised(object sender, EventArgs e)
	{
		onTypeEditSchemaInitialised();
	}

	private void onTypeEditSchemaInitialised()
	{
		mFileTypeInfos = new List<TypeEditFileTypeInfo>();
		foreach (ITypeDescriptor type in AppServices.TypeEditSchema.GetTypes())
		{
			if (!(type is IClassTypeDescriptor { IsAbstract: false } classTypeDescriptor))
			{
				continue;
			}
			foreach (string category in classTypeDescriptor.GetCategories())
			{
				TypeEditFileTypeInfo typeEditFileTypeInfo = null;
				if (category == "Blueprint")
				{
					typeEditFileTypeInfo = new TypeEditFileTypeInfo(classTypeDescriptor, "Blueprint");
				}
				else if (category == "AudioControlBlueprint")
				{
					typeEditFileTypeInfo = new TypeEditFileTypeInfo(classTypeDescriptor, "Audio Control");
				}
				if (typeEditFileTypeInfo != null)
				{
					mFileTypeInfos.Add(typeEditFileTypeInfo);
				}
			}
		}
	}
}
