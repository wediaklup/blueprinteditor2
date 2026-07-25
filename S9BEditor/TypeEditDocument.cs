using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Threading;
using S9BEditor.ViewModels;
using TypeEdit.DataHandling;
using TypeEdit.Interfaces;
using TypeEdit.Interfaces.Data.Instancing;
using TypeEdit.Interfaces.Editing;

namespace S9BEditor;

internal class TypeEditDocument : IDocument, IDisposable
{
	private IClassTypeDatum mCurrentDatum;

	private DispatcherTimer mValidationTimer;

	private bool mModified;

	public IClassTypeDatum CurrentDatum
	{
		get
		{
			return mCurrentDatum;
		}
		set
		{
			mCurrentDatum = value;
			updateDataEditor();
		}
	}

	public bool Modified
	{
		get
		{
			return mModified;
		}
		private set
		{
			if (mModified != value)
			{
				mModified = value;
				ModifiedChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public bool CanSave => true;

	public string FileName { get; private set; }

	public object Content => TypeEditContainer;

	public VMTypeEditContainer TypeEditContainer { get; private set; }

	public bool CanCopy
	{
		get
		{
			if (TypeEditContainer.SelectedItems.Count > 0)
			{
				return TypeEditContainer.SelectedItems[0]?.CanCopy ?? false;
			}
			return false;
		}
	}

	public bool CanDelete
	{
		get
		{
			if (TypeEditContainer.SelectedItems.Count > 0)
			{
				return TypeEditContainer.SelectedItems[0]?.CanDelete ?? false;
			}
			return false;
		}
	}

	public bool CanCut
	{
		get
		{
			if (TypeEditContainer.SelectedItems.Count > 0)
			{
				VMTypeEditComponent vMTypeEditComponent = TypeEditContainer.SelectedItems[0];
				if (vMTypeEditComponent != null && vMTypeEditComponent.CanDelete)
				{
					return vMTypeEditComponent.CanCopy;
				}
				return false;
			}
			return false;
		}
	}

	public bool CanPaste
	{
		get
		{
			if (TypeEditContainer.SelectedItems.Count > 0)
			{
				return TypeEditContainer.SelectedItems[0]?.CanPaste ?? false;
			}
			return false;
		}
	}

	public event EventHandler ModifiedChanged;

	public bool Initialise(IDocumentHost host)
	{
		return true;
	}

	public bool Save()
	{
		if (CurrentDatum != null)
		{
			try
			{
				if (TypeEditContainer != null)
				{
					if (TypeEditContainer.CommitChanges(resetModified: true))
					{
						AppServices.TypeEditSchema.SaveDataToFile(CurrentDatum, FileName);
						Modified = false;
						return true;
					}
					AppServices.ErrorManager.ErrorMessage("Failed to save " + Path.GetFileName(FileName) + ". A modified property could not be saved. See errors");
				}
			}
			catch (Exception e)
			{
				AppServices.ErrorManager.ErrorMessage("Failed to save " + Path.GetFileName(FileName), e);
			}
		}
		return false;
	}

	public bool LoadFrom(string fileName)
	{
		FileName = fileName;
		if (AppServices.TypeEditSchema.LoadDataFromFile(fileName) is IClassTypeDatum currentDatum)
		{
			CurrentDatum = currentDatum;
			return true;
		}
		return false;
	}

	private void updateDataEditor()
	{
		disposeRootItem();
		if (mCurrentDatum != null)
		{
			string text = AppServices.ResourceManager.SourcePath;
			string outRelativePath = DataUtil.GetRelativeDirectory(FileName, text);
			if (outRelativePath == null)
			{
				text = AppServices.ResourceManager.DevSourcePath;
				outRelativePath = DataUtil.GetRelativeDirectory(FileName, AppServices.ResourceManager.DevSourcePath);
			}
			if (outRelativePath != null)
			{
				DataUtil.GetProviderProductPath(outRelativePath, out var provider, out var product, out outRelativePath);
				TypeEditContainer = new VMTypeEditContainer(mCurrentDatum, provider, product, outRelativePath, text);
				TypeEditContainer.PropertyChanged += TypeEditContainer_PropertyChanged;
				TypeEditContainer.ValidationRequested += TypeEditContainer_ValidationRequested;
			}
		}
		scheduleValidation();
	}

	private void TypeEditContainer_ValidationRequested(object sender, EventArgs e)
	{
		scheduleValidation();
	}

	private void TypeEditContainer_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "Modified")
		{
			Modified |= TypeEditContainer.Modified;
		}
		else if (e.PropertyName == "ValidationRequired")
		{
			scheduleValidation();
		}
	}

	private void deleteValidationTimer()
	{
		if (mValidationTimer != null)
		{
			mValidationTimer.Stop();
			mValidationTimer.Tick -= mValidationTimer_Tick;
			mValidationTimer = null;
		}
	}

	private void scheduleValidation()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		deleteValidationTimer();
		mValidationTimer = new DispatcherTimer();
		mValidationTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
		mValidationTimer.Tick += mValidationTimer_Tick;
		mValidationTimer.Start();
	}

	private void mValidationTimer_Tick(object sender, EventArgs e)
	{
		deleteValidationTimer();
		AppServices.OutputManager.ClearErrors("Application", OutputMessageTarget.Document, this);
		TypeEditContainer.CommitChanges(resetModified: false);
		reportViewModelErrors(TypeEditContainer.Datum);
	}

	private void reportViewModelErrors(VMTypeEditComponent component)
	{
		if (component.Errors != null)
		{
			string[] errors = component.Errors;
			foreach (string text in errors)
			{
				AppServices.OutputManager.ReportError(null, OutputMessageTarget.Document, ErrorMessageType.Error, text + Environment.NewLine + Path.GetFileName(FileName), new DocumentErrorData
				{
					Document = this,
					ErrorSource = component
				});
			}
		}
		if (component.Warnings != null)
		{
			string[] warnings = component.Warnings;
			foreach (string text2 in warnings)
			{
				AppServices.OutputManager.ReportError(null, OutputMessageTarget.Document, ErrorMessageType.Warning, text2 + Environment.NewLine + Path.GetFileName(FileName), new DocumentErrorData
				{
					Document = this,
					ErrorSource = component
				});
			}
		}
		if (component is VMProperty vMProperty)
		{
			reportViewModelErrors(vMProperty.PropertyContent);
		}
		if (component is VMClassTypeDatum vMClassTypeDatum)
		{
			foreach (VMClassGroup group in vMClassTypeDatum.Groups)
			{
				foreach (VMProperty property in group.Properties)
				{
					reportViewModelErrors(property);
				}
			}
		}
		if (!(component is VMIndexedTypeDatum vMIndexedTypeDatum))
		{
			return;
		}
		foreach (VMTypeDatumBase item in vMIndexedTypeDatum.Items)
		{
			reportViewModelErrors(item);
		}
	}

	private void disposeRootItem()
	{
		if (TypeEditContainer != null)
		{
			TypeEditContainer.PropertyChanged -= TypeEditContainer_PropertyChanged;
			TypeEditContainer.ValidationRequested -= TypeEditContainer_ValidationRequested;
			TypeEditContainer.Dispose();
		}
	}

	public void Preview()
	{
		AppServices.ShapeViewer.Document = this;
	}

	public void Dispose()
	{
		deleteValidationTimer();
		disposeRootItem();
		if (AppServices.ShapeViewer.Document == this)
		{
			AppServices.ShapeViewer.Document = null;
		}
		AppServices.OutputManager.ClearErrors(null, OutputMessageTarget.Document, this);
	}

	public void Copy()
	{
		if (TypeEditContainer.SelectedItems.Count > 0)
		{
			VMTypeEditComponent vMTypeEditComponent = TypeEditContainer.SelectedItems[0];
			vMTypeEditComponent.Copy();
		}
	}

	public void Delete()
	{
		if (TypeEditContainer.SelectedItems.Count > 0)
		{
			VMTypeEditComponent vMTypeEditComponent = TypeEditContainer.SelectedItems[0];
			vMTypeEditComponent.Delete();
		}
	}

	public void Cut()
	{
		if (TypeEditContainer.SelectedItems.Count > 0)
		{
			VMTypeEditComponent vMTypeEditComponent = TypeEditContainer.SelectedItems[0];
			vMTypeEditComponent.Copy();
			vMTypeEditComponent.Delete();
		}
	}

	public void Paste()
	{
		if (TypeEditContainer.SelectedItems.Count > 0)
		{
			VMTypeEditComponent vMTypeEditComponent = TypeEditContainer.SelectedItems[0];
			vMTypeEditComponent.Paste();
		}
	}

	public void SelectComponent(object component)
	{
		if (component is VMTypeEditComponent vMTypeEditComponent)
		{
			vMTypeEditComponent.BringIntoView();
			VMTypeEditComponent vMTypeEditComponent2 = vMTypeEditComponent;
			while (vMTypeEditComponent2 != null && !vMTypeEditComponent2.CanSelect)
			{
				vMTypeEditComponent2 = vMTypeEditComponent2.Owner as VMTypeEditComponent;
			}
			vMTypeEditComponent2.IsSelected = true;
		}
	}
}
