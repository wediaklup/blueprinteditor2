using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TypeEdit.Interfaces.AddIns;

namespace S9BEditor
{
	internal class AddInManager
	{
		private List<IAddIn> mAddIns;

		private bool mLoaded;

		private bool mLoading;

		public IEnumerable<IAddIn> AddIns => mAddIns;

		public event EventHandler AddInsLoaded;

		public AddInManager()
		{
			mAddIns = new List<IAddIn>();
			mLoading = false;
			mLoaded = false;
			loadAddIns();
		}

		private void loadAddIns()
		{
			if (mLoading || mLoaded)
			{
				return;
			}
			mLoading = true;
			try
			{
				if (Directory.Exists("AddIns"))
				{
					string[] files = Directory.GetFiles("AddIns", "*.dll");
					List<Type> list = new List<Type>();
					string[] array = files;
					foreach (string path in array)
					{
						try
						{
							Assembly assembly = Assembly.LoadFile(Path.GetFullPath(path));
							list.AddRange(assembly.GetExportedTypes());
						}
						catch (Exception e)
						{
							AppServices.ErrorManager.ErrorMessage("Failed to load Add-in \"" + Path.GetFileName(path) + "\"", e);
						}
					}
					foreach (Type item2 in list)
					{
						try
						{
							Type[] interfaces = item2.GetInterfaces();
							if (Array.IndexOf<Type>(interfaces, typeof(IAddIn)) != -1 && Activator.CreateInstance(item2) is IAddIn item)
							{
								mAddIns.Add(item);
							}
						}
						catch (Exception e2)
						{
							AppServices.ErrorManager.ErrorMessage("An exception occurred trying to create " + item2.Name, e2);
						}
					}
				}
			}
			catch (Exception e3)
			{
				AppServices.ErrorManager.ErrorMessage("Failed to load Add-ins", e3);
			}
			AddInsLoaded?.Invoke(this, EventArgs.Empty);
			mLoaded = true;
			mLoading = false;
		}
	}
}
