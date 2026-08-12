using System;
using System.IO;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	internal class S9BEDocumentTypeProvider : IDocumentTypeProvider
	{
		private DocumentType<LuaScriptDocument> mLuaScriptDocumentType;

		private DocumentType<TypeEditDocument> mTypeEditDocumentType;

		public S9BEDocumentTypeProvider()
		{
			mLuaScriptDocumentType = new DocumentType<LuaScriptDocument>();
			mTypeEditDocumentType = new DocumentType<TypeEditDocument>();
		}

		public IDocumentType GetDocumentTypeForFile(string fileName)
		{
			string extension = Path.GetExtension(fileName);
			if (!extension.Equals(".lua", StringComparison.OrdinalIgnoreCase) && extension.Equals(".xml", StringComparison.OrdinalIgnoreCase) && AppServices.TypeEditSchema.GetRootTypeFromFile(fileName) != null)
			{
				return mTypeEditDocumentType;
			}
			return null;
		}
	}
}
