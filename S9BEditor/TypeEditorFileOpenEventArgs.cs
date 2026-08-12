using System;

namespace S9BEditor
{
	internal class TypeEditorFileOpenEventArgs : EventArgs
	{
		public string FileName { get; private set; }

		public string Provider { get; private set; }

		public string Product { get; private set; }

		public bool UseProviderProduct { get; private set; }

		public bool Handled { get; set; }

		public TypeEditorFileOpenEventArgs(string fileName, bool useProviderProduct = false, string provider = null, string product = null)
		{
			FileName = fileName;
			Provider = provider;
			Product = product;
			UseProviderProduct = useProviderProduct;
		}
	}
}
