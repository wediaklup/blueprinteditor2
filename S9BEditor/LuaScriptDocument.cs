using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using S9BEditor.ViewModels;
using TypeEdit.Interfaces.Editing;

namespace S9BEditor
{
	internal class LuaScriptDocument : IDocument, IDisposable
	{
		private VMLuaDocument mLuaDocument;

		public bool Modified
		{
			get
			{
				return mLuaDocument.Modified;
			}
			private set
			{
				mLuaDocument.Modified = value;
			}
		}

		public object Content => mLuaDocument;

		public bool CanSave => true;

		public string FileName { get; private set; }

		public bool CanCut
		{
			get
			{
				if (mLuaDocument.CanCopy)
				{
					return mLuaDocument.CanDelete;
				}
				return false;
			}
		}

		public bool CanCopy => mLuaDocument.CanCopy;

		public bool CanDelete => mLuaDocument.CanDelete;

		public bool CanPaste => mLuaDocument.CanPaste;

		public event EventHandler ModifiedChanged;

		public LuaScriptDocument()
		{
			mLuaDocument = new VMLuaDocument();
			mLuaDocument.PropertyChanged += mLuaDocument_PropertyChanged;
		}

		private void mLuaDocument_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Modified")
			{
				ModifiedChanged?.Invoke(this, e);
			}
		}

		public bool Initialise(IDocumentHost host)
		{
			return true;
		}

		public bool Save()
		{
			try
			{
				using (FileStream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write))
				{
					using StreamWriter streamWriter = new StreamWriter(stream, Encoding.ASCII);
					streamWriter.Write(mLuaDocument.Text);
				}
				mLuaDocument.Modified = false;
			}
			catch (Exception e)
			{
				AppServices.ErrorManager.ErrorMessage("Failed to save " + Path.GetFileName(FileName) + ".", e);
			}
			return true;
		}

		public bool LoadFrom(string fileName)
		{
			FileName = fileName;
			using (StreamReader streamReader = new StreamReader(fileName))
			{
				mLuaDocument.Text = streamReader.ReadToEnd();
				mLuaDocument.Modified = false;
			}
			return true;
		}

		public void Dispose()
		{
			if (mLuaDocument != null)
			{
				mLuaDocument.Dispose();
			}
		}

		public void Copy()
		{
			mLuaDocument.Copy();
		}

		public void Delete()
		{
			mLuaDocument.Delete();
		}

		public void Cut()
		{
			mLuaDocument.Copy();
			mLuaDocument.Delete();
		}

		public void Paste()
		{
			mLuaDocument.Paste();
		}

		public void SelectComponent(object component)
		{
		}
	}
}
