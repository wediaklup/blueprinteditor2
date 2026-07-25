using System;

namespace S9BEditor.ViewModels;

internal class ReferenceTargetEventArgs : EventArgs
{
	public VMProperty ReferenceTarget { get; private set; }

	public ReferenceTargetEventArgs(VMProperty referenceTarget)
	{
		ReferenceTarget = referenceTarget;
	}
}
