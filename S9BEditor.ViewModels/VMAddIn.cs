using TypeEdit.Base;
using TypeEdit.Interfaces.AddIns;

namespace S9BEditor.ViewModels;

internal class VMAddIn : ViewModelBase
{
	private IAddIn mAddIn;

	public string Name => mAddIn.Name;

	public string Description => mAddIn.Description;

	public VMAddIn(IAddIn addIn)
		: base(null)
	{
		mAddIn = addIn;
	}
}
