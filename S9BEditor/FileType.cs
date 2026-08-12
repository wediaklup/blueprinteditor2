using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	public class FileType
	{
		public string Name => FileTypeInfo.Name;

		public string DefaultFileName => FileTypeInfo.DefaultFileName;

		public IEditableFileTypeInfo FileTypeInfo { get; private set; }

		public FileType(IEditableFileTypeInfo fileType)
		{
			FileTypeInfo = fileType;
		}
	}
}
