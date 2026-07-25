using TypeEdit.Base;
using TypeEdit.Interfaces.Editing;

namespace S9BEditor;

internal class DocumentErrorData
{
	public IDocument Document { get; set; }

	public ViewModelBase ErrorSource { get; set; }
}
