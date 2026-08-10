using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using TypeEdit.DataHandling;
using TypeEdit.Interfaces.Data.Descriptors;
using TypeEdit.Interfaces.Data.Exporting;
using TypeEdit.Interfaces.Editing;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	internal class ExportContextActionsProvider : IActionProvider, IAsyncTaskProvider, IToolBarProvider, IToolBar
	{
		private enum ExportActionItemType
		{
			ExportThis,
			Export,
			ForcedExportThis,
			ForcedExport,
			CancelExport,
			Preview
		}

		private abstract class ExportActionItem : IWPFActionItem, IUriProviderActionItem, IActionItem
		{
			protected readonly ExportContextActionsProvider Owner;

			protected readonly List<InputGesture> Gestures;

			public ExportActionItemType Type { get; private set; }

			public ExportActionItem(ExportActionItemType type, ExportContextActionsProvider owner)
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Expected O, but got Unknown
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Expected O, but got Unknown
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Expected O, but got Unknown
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Expected O, but got Unknown
				Gestures = new List<InputGesture>();
				switch (type)
				{
					case ExportActionItemType.Export:
						Gestures.Add((InputGesture)new KeyGesture((Key)96));
						break;
					case ExportActionItemType.ForcedExport:
						Gestures.Add((InputGesture)new KeyGesture((Key)96, (ModifierKeys)3));
						break;
					case ExportActionItemType.Preview:
						Gestures.Add((InputGesture)new KeyGesture((Key)97));
						break;
					case ExportActionItemType.CancelExport:
						Gestures.Add((InputGesture)new KeyGesture((Key)1, (ModifierKeys)2));
						break;
				}
				Owner = owner;
				Type = type;
			}

			protected string GetDisplayText()
			{
				if (Type == ExportActionItemType.ExportThis)
					return "Export This Only";
				else if (Type is ExportActionItemType.ForcedExportThis)
					return "Force Export This Only";
				else if (Type is ExportActionItemType.Export)
					return "Export With References";
				else if (Type is ExportActionItemType.ForcedExport)
					return "Force Export With References";
				else if (Type is ExportActionItemType.CancelExport)
					return "Cancel Export";
				else if (Type is ExportActionItemType.Preview)
					return "Preview";
				else
					return string.Empty;
			}

			protected bool CanPreview(string relativePath, string sourceDirectory)
			{
				DataUtil.GetProviderProductPath(relativePath, out var provider, out var product, out var outRelativePath);
				if (provider != null && product != null && outRelativePath != null)
				{
					string extension = Path.GetExtension(relativePath);
					if (extension.Equals(".xml", StringComparison.CurrentCultureIgnoreCase))
					{
						try
						{
							if (AppServices.TypeEditSchema.GetRootTypeFromFile(Path.Combine(sourceDirectory, relativePath)) is IClassTypeDescriptor classTypeDescriptor)
							{
								foreach (string category in classTypeDescriptor.GetCategories())
								{
									if (category == "ShapeBlueprint" || category == "PreviewBlueprint")
									{
										return true;
									}
								}
							}
						}
						catch (Exception)
						{
						}
					}
				}
				return false;
			}

			protected bool CanExportFile(string relativePath, string sourceDirectory)
			{
				DataUtil.GetProviderProductPath(relativePath, out var provider, out var product, out var outRelativePath);
				if (provider != null && product != null && outRelativePath != null)
				{
					string extension = Path.GetExtension(relativePath);
					if (extension.Equals(".xml", StringComparison.CurrentCultureIgnoreCase))
					{
						try
						{
							if (AppServices.TypeEditSchema.GetRootTypeFromFile(Path.Combine(sourceDirectory, relativePath)) != null)
							{
								return true;
							}
						}
						catch (Exception)
						{
						}
					}
					else if (extension.Equals(".lua", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".csv", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".igs", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".ia", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".ace", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".kif", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".dds", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".wav", StringComparison.CurrentCultureIgnoreCase))
					{
						return true;
					}
				}
				return false;
			}

			protected void AddDirectoryRecursivelyToExportQueue(string sourceDirectory, string relativePath)
			{
				string path = Path.Combine(sourceDirectory, relativePath);
				string[] files = Directory.GetFiles(path);
				string[] array = files;
				foreach (string path2 in array)
				{
					string fileName = Path.GetFileName(path2);
					if (!Path.GetExtension(fileName).Equals(".xml", StringComparison.CurrentCultureIgnoreCase))
					{
						continue;
					}
					try
					{
						string outRelativePath = Path.Combine(relativePath, fileName);
						if (AppServices.TypeEditSchema.GetRootTypeFromFile(Path.Combine(sourceDirectory, outRelativePath)) != null)
						{
							DataUtil.GetProviderProductPath(outRelativePath, out var provider, out var product, out outRelativePath);
							ExportContext exportContext = new ExportContext();
							exportContext.AssetsDirectory = AppServices.ResourceManager.AssetsPath;
							exportContext.SourceDirectory = sourceDirectory;
							exportContext.SourceFileExporterFlags |= SourceFileExporterFlags.RecursiveAdd;
							if (Type == ExportActionItemType.ForcedExport)
							{
								exportContext.SourceFileExporterFlags |= SourceFileExporterFlags.ForceExport;
							}
							exportContext.SourceFileType = SourceFileType.XML;
							exportContext.ToolsDirectory = AppServices.ResourceManager.DeploymentPath;
							AppServices.Exporter.AddFile(provider, product, outRelativePath, exportContext);
						}
					}
					catch (Exception)
					{
						PLogger.Write("ExportContextActionsProvider : EXCEPTION");
					}
				}
				files = Directory.GetDirectories(path);
				string[] array2 = files;
				foreach (string path3 in array2)
				{
					string fileName2 = Path.GetFileName(path3);
					string relativePath2 = Path.Combine(relativePath, fileName2);
					AddDirectoryRecursivelyToExportQueue(sourceDirectory, relativePath2);
				}
			}

			protected void AddSingleFileToExportQueue(string sourceDirectory, string relativePath)
			{
				DataUtil.GetProviderProductPath(relativePath, out var provider, out var product, out relativePath);
				ExportContext exportContext = new ExportContext();
				exportContext.AssetsDirectory = AppServices.ResourceManager.AssetsPath;
				exportContext.SourceDirectory = sourceDirectory;
				if (Type == ExportActionItemType.Export || Type == ExportActionItemType.ForcedExport || Type == ExportActionItemType.Preview)
				{
					exportContext.SourceFileExporterFlags |= SourceFileExporterFlags.RecursiveAdd;
				}
				if (Type == ExportActionItemType.ForcedExportThis || Type == ExportActionItemType.ForcedExport)
				{
					exportContext.SourceFileExporterFlags |= SourceFileExporterFlags.ForceExport;
				}
				string overrideTargetFileName = null;
				if (Type == ExportActionItemType.Preview)
				{
					exportContext.SourceFileExporterFlags |= SourceFileExporterFlags.OverrideProviderProduct;
					exportContext.TargetProvider = "Kuju";
					exportContext.TargetProduct = "Shapeviewer";
					overrideTargetFileName = "PreviewShape.xml";
				}
				exportContext.SourceFileType = SourceFileType.Unknown;
				exportContext.ToolsDirectory = AppServices.ResourceManager.DeploymentPath;
				PLogger.Write("ExportContextActionsProvider.AddSingleFileToExportQueue: AddFile(" + provider + ", " + product + ", " + exportContext + ", " + overrideTargetFileName + ")");
				AppServices.Exporter.AddFile(provider, product, relativePath, exportContext, overrideTargetFileName);
			}

			public IList<InputGesture> GetWPFInputGestures()
			{
				return Gestures;
			}

			public Uri GetResourceURI(ActionItemResourceType ResourceType)
			{
				if (Type == ExportActionItemType.ExportThis)
					return new Uri("pack://application:,,,/Resources/ExportOneHS.png");
				else if (Type is ExportActionItemType.Export)
					return new Uri("pack://application:,,,/Resources/ExportHS.png");
				else if (Type is ExportActionItemType.Preview)
					return new Uri("pack://application:,,,/Resources/FormRunHS.png");
				else if (Type is ExportActionItemType.CancelExport)
					return new Uri("pack://application:,,,/Resources/StopHS.png");
				else
					return null;
			}
		}

		private class DocumentExportActionItem : ExportActionItem, IDocumentActionItem, IActionItem
		{
			public IEnumerable<IDocumentActionItem> Items => null;

			public DocumentExportActionItem(ExportActionItemType type, ExportContextActionsProvider owner)
				: base(type, owner)
			{
			}

			public string GetDisplayText(IDocument document)
			{
				return GetDisplayText();
			}

			public bool CanPerformAction(IDocument document)
			{
				if (document != null && !AppServices.Exporter.IsExporting)
				{
					string text = AppServices.ResourceManager.SourcePath;
					string relativeDirectory = DataUtil.GetRelativeDirectory(document.FileName, text);
					if (relativeDirectory == null)
					{
						text = AppServices.ResourceManager.DevSourcePath;
						relativeDirectory = DataUtil.GetRelativeDirectory(document.FileName, text);
					}
					if (base.Type == ExportActionItemType.ExportThis || base.Type == ExportActionItemType.Export || base.Type == ExportActionItemType.ForcedExportThis || base.Type == ExportActionItemType.ForcedExport)
					{
						return CanExportFile(relativeDirectory, text);
					}
					if (base.Type == ExportActionItemType.Preview)
					{
						return CanPreview(relativeDirectory, text);
					}
				}
				return false;
			}

			public void PerformAction(IDocument document)
			{
				if (document.Modified && !document.Save())
				{
					return;
				}
				string text = AppServices.ResourceManager.SourcePath;
				string relativeDirectory = DataUtil.GetRelativeDirectory(document.FileName, text);
				if (relativeDirectory == null)
				{
					text = AppServices.ResourceManager.DevSourcePath;
					relativeDirectory = DataUtil.GetRelativeDirectory(document.FileName, text);
				}
				if (base.Type == ExportActionItemType.Preview)
				{
					if (document is TypeEditDocument document2)
					{
						AddSingleFileToExportQueue(text, relativeDirectory);
						Owner.schedulePreview(document2);
					}
				}
				else
				{
					AddSingleFileToExportQueue(text, relativeDirectory);
					Owner.scheduleExport();
				}
			}
		}

		private class ApplicationExportActionItem : ExportActionItem, IApplicationActionItem, IWPFActionItem, IUriProviderActionItem, IActionItem
		{
			public IEnumerable<IApplicationActionItem> Items => null;

			public ApplicationExportActionItem(ExportActionItemType type, ExportContextActionsProvider owner)
				: base(type, owner)
			{
			}

			string IApplicationActionItem.GetDisplayText()
			{
				return GetDisplayText();
			}

			public bool CanPerformAction()
			{
				if (base.Type == ExportActionItemType.CancelExport)
				{
					return AppServices.Exporter.IsExporting;
				}
				return false;
			}

			public void PerformAction()
			{
				if (base.Type == ExportActionItemType.CancelExport)
				{
					Owner.cancelExport();
				}
			}
		}

		private class FileExportActionItem : ExportActionItem, IFileActionItem, IWPFActionItem, IUriProviderActionItem, IActionItem
		{
			public IEnumerable<IFileActionItem> Items => null;

			public FileExportActionItem(ExportActionItemType type, ExportContextActionsProvider owner)
				: base(type, owner)
			{
			}

			public string GetDisplayText(string sourceDirectory, string relativePath, bool isDirectory)
			{
				return GetDisplayText();
			}

			public bool CanPerformAction(string sourceDirectory, string relativePath, bool isDirectory)
			{
				if (sourceDirectory != null && relativePath != null && !AppServices.Exporter.IsExporting)
				{
					if (isDirectory)
					{
						if (base.Type == ExportActionItemType.Export || base.Type == ExportActionItemType.ForcedExport)
						{
							return true;
						}
					}
					else if (base.Type == ExportActionItemType.ExportThis || base.Type == ExportActionItemType.Export || base.Type == ExportActionItemType.ForcedExportThis || base.Type == ExportActionItemType.ForcedExport)
					{
						return CanExportFile(relativePath, sourceDirectory);
					}
				}
				return false;
			}

			public void PerformAction(string sourceDirectory, string relativePath, bool isDirectory)
			{
				if (isDirectory)
				{
					if (base.Type == ExportActionItemType.Export || base.Type == ExportActionItemType.ForcedExport)
					{
						AddDirectoryRecursivelyToExportQueue(sourceDirectory, relativePath);
						if (!AppServices.Exporter.FileQueueEmpty)
						{
							Owner.scheduleExport();
						}
					}
				}
				else if (base.Type == ExportActionItemType.ExportThis || base.Type == ExportActionItemType.Export || base.Type == ExportActionItemType.ForcedExportThis || base.Type == ExportActionItemType.ForcedExport)
				{
					AddSingleFileToExportQueue(sourceDirectory, relativePath);
					Owner.scheduleExport();
				}
			}
		}

		private ExportAsyncTask mCurrentTask;

		public List<IActionItem> ActionItems { get; private set; }

		IEnumerable<IActionItem> IActionProvider.Items => ActionItems;

		public IEnumerable<IToolBar> ToolBars
		{
			get
			{
				yield return this;
			}
		}

		IEnumerable<IActionItem> IToolBar.Actions => ToolBarActionItems;

		public List<IActionItem> ToolBarActionItems { get; private set; }

		public event EventHandler<AsyncTaskEventArgs> TaskAdded;

		internal void cancelExport()
		{
			if (mCurrentTask != null)
			{
				mCurrentTask.Cancel();
			}
		}

		public ExportContextActionsProvider()
		{
			ActionItems = new List<IActionItem>
			{
				new FileActionParentItem
				{
					DisplayText = "Export",
					Items = 
					{
						(IFileActionItem)new FileExportActionItem(ExportActionItemType.Export, this),
						(IFileActionItem)new FileExportActionItem(ExportActionItemType.ExportThis, this),
						(IFileActionItem)new FileSeparatorActionItem(),
						(IFileActionItem)new FileExportActionItem(ExportActionItemType.ForcedExport, this),
						(IFileActionItem)new FileExportActionItem(ExportActionItemType.ForcedExportThis, this)
					}
				},
				new ApplicationExportActionItem(ExportActionItemType.CancelExport, this),
				new DocumentActionParentItem
				{
					DisplayText = "Export {0}",
					Items = 
					{
						(IDocumentActionItem)new DocumentExportActionItem(ExportActionItemType.Export, this),
						(IDocumentActionItem)new DocumentExportActionItem(ExportActionItemType.ExportThis, this),
						(IDocumentActionItem)new DocumentSeparatorActionItem(),
						(IDocumentActionItem)new DocumentExportActionItem(ExportActionItemType.ForcedExport, this),
						(IDocumentActionItem)new DocumentExportActionItem(ExportActionItemType.ForcedExportThis, this)
					}
				},
				new DocumentExportActionItem(ExportActionItemType.Preview, this)
			};
			ToolBarActionItems = new List<IActionItem>
			{
				new DocumentExportActionItem(ExportActionItemType.Export, this),
				new DocumentExportActionItem(ExportActionItemType.Preview, this),
				new DocumentSeparatorActionItem(),
				new ApplicationExportActionItem(ExportActionItemType.CancelExport, this)
			};
		}

		private void scheduleExport()
		{
			if (mCurrentTask == null)
			{
				mCurrentTask = new ExportAsyncTask();
				mCurrentTask.TaskComplete += mCurrentTask_TaskComplete;
				OnTaskAdded(new AsyncTaskEventArgs(mCurrentTask));
			}
		}

		private void schedulePreview(TypeEditDocument document)
		{
			if (mCurrentTask == null)
			{
				mCurrentTask = new PreviewAsyncTask(document);
				mCurrentTask.TaskComplete += mCurrentTask_TaskComplete;
				OnTaskAdded(new AsyncTaskEventArgs(mCurrentTask));
			}
		}

		private void mCurrentTask_TaskComplete(object sender, EventArgs e)
		{
			mCurrentTask.TaskComplete -= mCurrentTask_TaskComplete;
			mCurrentTask = null;
		}

		protected virtual void OnTaskAdded(AsyncTaskEventArgs args)
		{
			TaskAdded?.Invoke(this, args);
		}
	}
}
