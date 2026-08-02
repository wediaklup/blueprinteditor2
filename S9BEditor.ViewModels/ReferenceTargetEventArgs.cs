using System;

namespace S9BEditor.ViewModels
{
	public class ReferenceTargetEventArgs : EventArgs
	{
		public VMProperty ReferenceTarget { get; private set; }

		public ReferenceTargetEventArgs(VMProperty referenceTarget)
		{
			ReferenceTarget = referenceTarget;
		}
	}
}
