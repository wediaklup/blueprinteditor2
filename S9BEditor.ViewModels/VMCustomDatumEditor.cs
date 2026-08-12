using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using TypeEdit.Base;
using TypeEdit.Interfaces.Data.Instancing;
using TypeEdit.Interfaces.Editing;

namespace S9BEditor.ViewModels
{
	internal class VMCustomDatumEditor : VMTypeDatumBase
	{
		internal IDataEditor Editor { get; private set; }

		public object Value
		{
			get
			{
				if (Editor != null)
				{
					try
					{
						return Editor.Value;
					}
					catch (Exception ex)
					{
						reportEditorException(ex, "getting Value Property");
					}
				}
				return "Failed to load Editor";
			}
			set
			{
				if (Editor != null)
				{
					try
					{
						Editor.Value = value;
					}
					catch (Exception ex)
					{
						reportEditorException(ex, "setting Value Property");
					}
				}
			}
		}

		public object Content
		{
			get
			{
				if (Editor != null)
				{
					try
					{
						return Editor.Content;
					}
					catch (Exception ex)
					{
						reportEditorException(ex, "getting Content Property");
					}
				}
				return null;
			}
		}

		public bool IsValueEditable
		{
			get
			{
				if (Editor != null)
				{
					try
					{
						return Editor.IsValueEditable;
					}
					catch (Exception ex)
					{
						reportEditorException(ex, "getting IsValueEditable Property");
					}
				}
				return false;
			}
		}

		public override bool IsValid
		{
			get
			{
				if (Editor == null)
				{
					return true;
				}
				return Editor.IsValid;
			}
		}

		public override bool CanResetToDefault
		{
			get
			{
				if (Editor is IDataEditor2 dataEditor)
				{
					return dataEditor.CanResetToDefault;
				}
				return false;
			}
		}

		public override string Name
		{
			get
			{
				if (Editor is IDatumNameProvider datumNameProvider)
				{
					if (datumNameProvider.Name.Length > 0)
					{
						return base.Name + ": \"" + datumNameProvider.Name + "\"";
					}
					return base.Name + ": <unnamed>";
				}
				return base.Name;
			}
		}

		public VMCustomDatumEditor(ViewModelBase owner, ITypeDatum datum, IDataEditor customEditor, int elementIndex = 0)
			: base(owner, datum, elementIndex)
		{
			Editor = customEditor;
			if (Editor != null)
			{
				Editor.ValueChanged += Editor_ValueChanged;
				Editor.ModifiedChanged += Editor_ModifiedChanged;
				Editor.ErrorsChanged += Editor_ErrorsChanged;
				Editor.WarningsChanged += Editor_WarningsChanged;
				Editor.IsValidChanged += Editor_IsValidChanged;
				Editor.ValidationRequiredChanged += Editor_ValidationRequiredChanged;
			}
			if (Editor is IDatumNameProvider datumNameProvider)
			{
				datumNameProvider.NameChanged += asNameProvider_NameChanged;
			}
		}

		private void asNameProvider_NameChanged(object sender, EventArgs e)
		{
			RaisePropertyChanged("Name");
		}

		private void Editor_ValidationRequiredChanged(object sender, EventArgs e)
		{
			base.ValidationRequired = Editor.ValidationRequired;
		}

		private void Editor_IsValidChanged(object sender, EventArgs e)
		{
			RaisePropertyChanged("IsValid");
		}

		private void Editor_WarningsChanged(object sender, EventArgs e)
		{
			base.Warnings = Editor.Warnings;
		}

		private void Editor_ErrorsChanged(object sender, EventArgs e)
		{
			base.Errors = Editor.Errors;
		}

		private void Editor_ModifiedChanged(object sender, EventArgs e)
		{
			try
			{
				base.Modified = Editor.Modified;
			}
			catch (Exception ex)
			{
				reportEditorException(ex, "getting Modified Property");
			}
		}

		private void Editor_ValueChanged(object sender, EventArgs e)
		{
			RaisePropertyChanged("Value");
		}

		public override string GetValue(int elementIndex, CultureInfo cultureInfo)
		{
			if (elementIndex != 0)
			{
				throw new NotSupportedException("elementIndex must be 0");
			}
			return Convert.ToString(Value, CultureInfo.CurrentCulture);
		}

		public override void SetValue(string value, int elementIndex, CultureInfo cultureInfo)
		{
			if (elementIndex != 0)
			{
				throw new NotSupportedException("elementIndex must be 0");
			}
			if (!IsValueEditable)
			{
				throw new NotSupportedException("IsValueEditable is set to false but trying to set a value");
			}
			Value = value;
		}

		public override bool TrySetValue(string value, int elementIndex, CultureInfo cultureInfo)
		{
			try
			{
				SetValue(value, elementIndex, cultureInfo);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		private void reportEditorException(Exception ex, string reason)
		{
			if (Editor != null)
			{
				Assembly assembly = Assembly.GetAssembly(Editor.GetType());
				if (assembly != null)
				{
					AppServices.ErrorManager.ErrorMessage("An add-in threw an exception when " + reason + "." + Environment.NewLine + "Add-in: " + Path.GetFileName(assembly.Location) + ", Editor Type: " + Editor.GetType().FullName, ex);
				}
				removeEditor();
			}
		}

		private void removeEditor()
		{
			if (Editor != null)
			{
				Editor.ValueChanged -= Editor_ValueChanged;
				Editor.ModifiedChanged -= Editor_ModifiedChanged;
				Editor.ErrorsChanged -= Editor_ErrorsChanged;
				Editor.WarningsChanged -= Editor_WarningsChanged;
				Editor.IsValidChanged -= Editor_IsValidChanged;
				Editor.ValidationRequiredChanged -= Editor_ValidationRequiredChanged;
				if (Editor is IDatumNameProvider datumNameProvider)
				{
					datumNameProvider.NameChanged -= asNameProvider_NameChanged;
				}
				if (Editor is IDisposable disposable)
				{
					disposable.Dispose();
				}
				RaisePropertyChanged("Content");
				RaisePropertyChanged("Value");
				RaisePropertyChanged("IsValueEditable");
			}
		}

		public override void Dispose()
		{
			removeEditor();
			base.Dispose();
		}

		public override bool CommitChanges(bool resetModifiedState)
		{
			bool flag = true;
			if (Editor != null)
			{
				flag = Editor.CommitChanges(resetModifiedState);
			}
			if (flag)
			{
				return base.CommitChanges(resetModifiedState);
			}
			return false;
		}

		public override void Reload()
		{
			if (Editor != null)
			{
				try
				{
					Editor.Reload();
				}
				catch (Exception ex)
				{
					reportEditorException(ex, "reloading");
				}
			}
			base.Reload();
		}

		internal override void Initialise()
		{
			if (Editor != null)
			{
				try
				{
					Editor.Initialise(base.Datum);
				}
				catch (Exception ex)
				{
					reportEditorException(ex, "initialising");
				}
			}
			base.Initialise();
		}

		public override void ResetToDefault()
		{
			if (Editor is IDataEditor2 dataEditor)
			{
				dataEditor.ResetToDefault();
			}
			CommitChanges(resetModifiedState: false);
		}
	}
}
