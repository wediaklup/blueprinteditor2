using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using TypeEdit.Interfaces.Editing;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	internal class DocumentTabControl : OverflowTabControl, IDocumentHost
	{
		private ObservableCollection<DocumentTabItem> mDocumentTabItems;

		private Dictionary<FileSystemWatcher, List<DocumentTabItem>> mDocumentFolderWatchers;

		public List<IDocumentTypeProvider> DocumentFileTypeMap { get; set; }

		public IDocument CurrentFocusedDocument
		{
			get
			{
				if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
				{
					return documentTabItem.Document;
				}
				return null;
			}
			set
			{
				foreach (object item in (IEnumerable)((ItemsControl)this).Items)
				{
					if (item is DocumentTabItem documentTabItem && documentTabItem.Document == value)
					{
						((Selector)this).SelectedItem = documentTabItem;
						break;
					}
				}
			}
		}

		static DocumentTabControl()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DocumentTabControl), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(DocumentTabControl)));
		}

		public DocumentTabControl()
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Expected O, but got Unknown
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Expected O, but got Unknown
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Expected O, but got Unknown
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Expected O, but got Unknown
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Expected O, but got Unknown
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Expected O, but got Unknown
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Expected O, but got Unknown
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Expected O, but got Unknown
			//IL_0218: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Expected O, but got Unknown
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Expected O, but got Unknown
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Expected O, but got Unknown
			base._002Ector();
			mDocumentFolderWatchers = new Dictionary<FileSystemWatcher, List<DocumentTabItem>>();
			mDocumentTabItems = new ObservableCollection<DocumentTabItem>();
			((ItemsControl)this).ItemsSource = mDocumentTabItems;
			ICollectionView defaultView = CollectionViewSource.GetDefaultView((object)((ItemsControl)this).ItemsSource);
			((Collection<SortDescription>)(object)defaultView.SortDescriptions).Add(new SortDescription("Header", ListSortDirection.Ascending));
			CommandBinding val = new CommandBinding();
			val.Command = (ICommand)ApplicationCommands.Close;
			val.Executed += new ExecutedRoutedEventHandler(closeCommand_Executed);
			val.CanExecute += new CanExecuteRoutedEventHandler(closeCommand_CanExecute);
			((UIElement)this).CommandBindings.Add(val);
			val = new CommandBinding();
			val.Command = (ICommand)Commands.CloseAll;
			val.Executed += new ExecutedRoutedEventHandler(closeAllCommand_Executed);
			val.CanExecute += new CanExecuteRoutedEventHandler(closeAllCommand_CanExecute);
			((UIElement)this).CommandBindings.Add(val);
			val = new CommandBinding();
			val.Command = (ICommand)ApplicationCommands.Save;
			val.Executed += new ExecutedRoutedEventHandler(saveCommand_Executed);
			val.CanExecute += new CanExecuteRoutedEventHandler(saveCommand_CanExecute);
			((UIElement)this).CommandBindings.Add(val);
			val = new CommandBinding();
			val.Command = (ICommand)Commands.SaveAll;
			val.Executed += new ExecutedRoutedEventHandler(saveAllCommand_Executed);
			val.CanExecute += new CanExecuteRoutedEventHandler(saveAllCommand_CanExecute);
			((UIElement)this).CommandBindings.Add(val);
			val = new CommandBinding();
			val.Command = (ICommand)ApplicationCommands.Copy;
			val.Executed += new ExecutedRoutedEventHandler(copyCommand_Executed);
			val.CanExecute += new CanExecuteRoutedEventHandler(copyCommand_CanExecute);
			((UIElement)this).CommandBindings.Add(val);
			val = new CommandBinding();
			val.Command = (ICommand)ApplicationCommands.Cut;
			val.Executed += new ExecutedRoutedEventHandler(cutCommand_Executed);
			val.CanExecute += new CanExecuteRoutedEventHandler(cutCommand_CanExecute);
			((UIElement)this).CommandBindings.Add(val);
			val = new CommandBinding();
			val.Command = (ICommand)ApplicationCommands.Paste;
			val.Executed += new ExecutedRoutedEventHandler(pasteCommand_Executed);
			val.CanExecute += new CanExecuteRoutedEventHandler(pasteCommand_CanExecute);
			((UIElement)this).CommandBindings.Add(val);
			val = new CommandBinding();
			val.Command = (ICommand)ApplicationCommands.Delete;
			val.Executed += new ExecutedRoutedEventHandler(deleteCommand_Executed);
			val.CanExecute += new CanExecuteRoutedEventHandler(deleteCommand_CanExecute);
			((UIElement)this).CommandBindings.Add(val);
		}

		private void closeAllCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = ((CollectionView)((ItemsControl)this).Items).Count > 0;
			((RoutedEventArgs)e).Handled = true;
		}

		private void closeAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CloseAll();
			((RoutedEventArgs)e).Handled = true;
		}

		private void copyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				e.CanExecute = documentTabItem.Document.CanCopy;
			}
			else
			{
				e.CanExecute = false;
			}
			((RoutedEventArgs)e).Handled = true;
		}

		private void copyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				documentTabItem.Document.Copy();
			}
			((RoutedEventArgs)e).Handled = true;
		}

		private void cutCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				e.CanExecute = documentTabItem.Document.CanCut;
			}
			else
			{
				e.CanExecute = false;
			}
			((RoutedEventArgs)e).Handled = true;
		}

		private void cutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				documentTabItem.Document.Cut();
			}
			((RoutedEventArgs)e).Handled = true;
		}

		private void pasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				e.CanExecute = documentTabItem.Document.CanPaste;
			}
			else
			{
				e.CanExecute = false;
			}
			((RoutedEventArgs)e).Handled = true;
		}

		private void pasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				documentTabItem.Document.Paste();
			}
			((RoutedEventArgs)e).Handled = true;
		}

		private void deleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				e.CanExecute = documentTabItem.Document.CanDelete;
			}
			else
			{
				e.CanExecute = false;
			}
			((RoutedEventArgs)e).Handled = true;
		}

		private void deleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				documentTabItem.Document.Delete();
			}
			((RoutedEventArgs)e).Handled = true;
		}

		private void closeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (e.Parameter is DocumentTabItem documentTabItem)
			{
				e.CanExecute = documentTabItem.CanClose;
			}
			else
			{
				e.CanExecute = false;
			}
		}

		public void FocusContent()
		{
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				documentTabItem.FocusContent();
			}
		}

		private void saveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CurrentFocusedDocument != null && CurrentFocusedDocument.CanSave;
			((RoutedEventArgs)e).Handled = true;
		}

		private void saveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			TrySave(null, confirmBeforeSave: false);
			((RoutedEventArgs)e).Handled = true;
		}

		private void saveAllCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = ((CollectionView)((ItemsControl)this).Items).Count > 0;
			((RoutedEventArgs)e).Handled = true;
		}

		private void saveAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SaveAll();
			((RoutedEventArgs)e).Handled = true;
		}

		private void closeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (!(e.Parameter is DocumentTabItem documentTabItem))
			{
				return;
			}
			bool flag = true;
			if (documentTabItem.Document != null)
			{
				if (documentTabItem.Modified)
				{
					flag = TrySave(documentTabItem);
				}
				if (flag)
				{
					documentTabItem.Dispose();
				}
			}
			if (flag)
			{
				closeDocument(documentTabItem);
			}
		}

		public IDocument LoadDocument(string fileName)
		{
			if (DocumentFileTypeMap != null)
			{
				IDocumentType documentType = null;
				foreach (IDocumentTypeProvider item in DocumentFileTypeMap)
				{
					documentType = item.GetDocumentTypeForFile(fileName);
					if (documentType != null)
					{
						break;
					}
				}
				if (documentType != null)
				{
					IDocument document = null;
					foreach (DocumentTabItem mDocumentTabItem in mDocumentTabItems)
					{
						if (mDocumentTabItem.DocumentType == documentType && mDocumentTabItem.FileName == fileName)
						{
							document = mDocumentTabItem.Document;
							((TabItem)mDocumentTabItem).IsSelected = true;
						}
					}
					if (document == null)
					{
						try
						{
							document = documentType.CreateDocument();
						}
						catch (Exception e)
						{
							AppServices.ErrorManager.ErrorMessage("Could not create document for " + fileName + ". Document type was " + documentType.GetType().FullName, e);
						}
						if (document != null)
						{
							if (document.Initialise(this))
							{
								string directoryName = Path.GetDirectoryName(Path.GetFullPath(fileName));
								DocumentTabItem documentTabItem = new DocumentTabItem(document, documentType, fileName);
								mDocumentTabItems.Add(documentTabItem);
								((TabItem)documentTabItem).IsSelected = true;
								List<DocumentTabItem> list = null;
								foreach (KeyValuePair<FileSystemWatcher, List<DocumentTabItem>> mDocumentFolderWatcher in mDocumentFolderWatchers)
								{
									if (mDocumentFolderWatcher.Key.Path == directoryName)
									{
										list = mDocumentFolderWatcher.Value;
										break;
									}
								}
								if (list == null)
								{
									FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(directoryName);
									fileSystemWatcher.IncludeSubdirectories = false;
									FileSystemWatcher fileSystemWatcher2 = fileSystemWatcher;
									fileSystemWatcher2.Deleted += fsw_Deleted;
									fileSystemWatcher2.EnableRaisingEvents = true;
									list = new List<DocumentTabItem>();
									mDocumentFolderWatchers.Add(fileSystemWatcher2, list);
								}
								list?.Add(documentTabItem);
							}
							else if (document is IDisposable disposable)
							{
								disposable.Dispose();
							}
						}
					}
					return document;
				}
			}
			return null;
		}

		private void fsw_Deleted(object sender, FileSystemEventArgs e)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)delegate
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Expected O, but got Unknown
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Expected O, but got Unknown
				FrameworkElement val = (FrameworkElement)(object)this;
				while (val.Parent is FrameworkElement)
				{
					val = (FrameworkElement)val.Parent;
				}
				if (((UIElement)val).IsKeyboardFocusWithin)
				{
					checkAllDocumentsExist();
				}
				else
				{
					((UIElement)val).IsKeyboardFocusWithinChanged += new DependencyPropertyChangedEventHandler(curParent_IsKeyboardFocusWithinChanged);
				}
			}, new object[0]);
		}

		private void curParent_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			FrameworkElement val = (FrameworkElement)((sender is FrameworkElement) ? sender : null);
			if (val != null)
			{
				((UIElement)val).IsKeyboardFocusWithinChanged -= new DependencyPropertyChangedEventHandler(curParent_IsKeyboardFocusWithinChanged);
			}
			checkAllDocumentsExist();
		}

		public bool SaveAll()
		{
			List<DocumentTabItem> list = new List<DocumentTabItem>();
			foreach (DocumentTabItem mDocumentTabItem in mDocumentTabItems)
			{
				list.Add(mDocumentTabItem);
			}
			return saveDocuments(list, confirmBeforeSave: false);
		}

		public bool CloseAll()
		{
			List<DocumentTabItem> list = new List<DocumentTabItem>();
			foreach (DocumentTabItem mDocumentTabItem in mDocumentTabItems)
			{
				if (mDocumentTabItem.CanClose)
				{
					list.Add(mDocumentTabItem);
				}
			}
			bool flag = saveDocuments(list);
			if (flag)
			{
				List<DocumentTabItem> list2 = new List<DocumentTabItem>(mDocumentTabItems);
				foreach (DocumentTabItem item in list2)
				{
					closeDocument(item);
				}
			}
			return flag;
		}

		public bool TrySave(DocumentTabItem document = null, bool confirmBeforeSave = true)
		{
			if (document == null && ((Selector)this).SelectedItem is DocumentTabItem documentTabItem)
			{
				document = documentTabItem;
			}
			if (document != null)
			{
				return saveDocuments(new DocumentTabItem[1] { document }, confirmBeforeSave);
			}
			return false;
		}

		private void checkCurrentDocumentExists()
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Invalid comparison between Unknown and I4
			if (((Selector)this).SelectedItem is DocumentTabItem documentTabItem && !documentTabItem.EditorOnlyModified && !File.Exists(documentTabItem.FileName))
			{
				documentTabItem.EditorOnlyModified = true;
				if ((int)MessageBox.Show((Window)(object)AppServices.MainWindow, "File " + Path.GetFileName(documentTabItem.FileName) + " no longer exists on disk. Keep this file in editor?", "Blueprint Editor 2", (MessageBoxButton)4) == 7)
				{
					closeDocument(documentTabItem);
				}
			}
		}

		private bool saveDocuments(IEnumerable<DocumentTabItem> documents, bool confirmBeforeSave = true)
		{
			checkAllDocumentsExist();
			List<string> list = new List<string>();
			foreach (DocumentTabItem document in documents)
			{
				if (document.Modified)
				{
					list.Add(Path.GetFullPath(document.FileName));
				}
			}
			bool result = true;
			if (list.Count > 0)
			{
				bool flag;
				bool flag2;
				if (confirmBeforeSave)
				{
					SaveModifiedFilesWindow saveModifiedFilesWindow = new SaveModifiedFilesWindow();
					((Window)saveModifiedFilesWindow).Owner = Window.GetWindow((DependencyObject)(object)this);
					saveModifiedFilesWindow.DocumentsFileNames = list;
					flag = ((Window)saveModifiedFilesWindow).ShowDialog() ?? false;
					flag2 = saveModifiedFilesWindow.ShouldSaveFiles;
				}
				else
				{
					flag = true;
					flag2 = true;
				}
				if (flag)
				{
					if (flag2)
					{
						foreach (DocumentTabItem document2 in documents)
						{
							if (document2.Document.Save())
							{
								document2.EditorOnlyModified = false;
								continue;
							}
							result = false;
							break;
						}
					}
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		private void checkAllDocumentsExist()
		{
			List<DocumentTabItem> list = new List<DocumentTabItem>(mDocumentTabItems);
			foreach (DocumentTabItem item in list)
			{
				if (!item.EditorOnlyModified && !File.Exists(item.FileName))
				{
					CurrentFocusedDocument = item.Document;
					checkCurrentDocumentExists();
				}
			}
		}

		private void closeDocument(DocumentTabItem documentTabItem)
		{
			string directoryName = Path.GetDirectoryName(Path.GetFullPath(documentTabItem.FileName));
			FileSystemWatcher fileSystemWatcher = null;
			foreach (KeyValuePair<FileSystemWatcher, List<DocumentTabItem>> mDocumentFolderWatcher in mDocumentFolderWatchers)
			{
				if (mDocumentFolderWatcher.Key.Path == directoryName)
				{
					mDocumentFolderWatcher.Value.Remove(documentTabItem);
					if (mDocumentFolderWatcher.Value.Count == 0)
					{
						fileSystemWatcher = mDocumentFolderWatcher.Key;
					}
				}
			}
			if (fileSystemWatcher != null)
			{
				fileSystemWatcher.EnableRaisingEvents = false;
				fileSystemWatcher.Dispose();
				fileSystemWatcher.Deleted -= fsw_Deleted;
				mDocumentFolderWatchers.Remove(fileSystemWatcher);
			}
			if (documentTabItem == ((Selector)this).SelectedItem && (((UIElement)documentTabItem).IsKeyboardFocusWithin || ((UIElement)documentTabItem).IsFocused))
			{
				int num = ((CollectionView)((ItemsControl)this).Items).IndexOf((object)documentTabItem);
				num = ((num > 0) ? (num - 1) : ((mDocumentTabItems.Count > 1) ? 1 : (-1)));
				if (num != -1 && ((ItemsControl)this).Items[num] is DocumentTabItem documentTabItem2)
				{
					((Selector)this).SelectedItem = documentTabItem2;
					((UIElement)this).UpdateLayout();
					documentTabItem2.FocusContent();
				}
			}
			mDocumentTabItems.Remove(documentTabItem);
		}
	}
}
