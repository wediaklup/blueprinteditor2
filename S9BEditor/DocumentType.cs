using TypeEdit.Interfaces.Editing;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	internal class DocumentType<T> : IDocumentType where T : IDocument, new()
	{
		public IDocument CreateDocument()
		{
			return new T();
		}
	}
}
