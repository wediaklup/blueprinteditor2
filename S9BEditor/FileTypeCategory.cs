using System.Collections.ObjectModel;

namespace S9BEditor;

public class FileTypeCategory
{
	public string Name { get; private set; }

	public ObservableCollection<FileType> Items { get; private set; }

	public FileTypeCategory(string name)
	{
		Name = name;
		Items = new ObservableCollection<FileType>();
	}
}
