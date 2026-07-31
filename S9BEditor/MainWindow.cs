using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using RailWorks;
using S9BEditor.Properties;
using S9BEditor.ViewModels;
using TypeEdit.DataHandling;
using TypeEdit.Interfaces;
using TypeEdit.Interfaces.AddIns;
using TypeEdit.Interfaces.Data.Instancing;
using TypeEdit.Interfaces.Diagnostics;
using TypeEdit.Interfaces.Editing;
using TypeEdit.Interfaces.UI;

namespace S9BEditor
{
	public class MainWindow : CustomWindow, IComponentConnector, IStyleConnector
	{
		private AddInManager mAddInManager;

		private List<IFileTypeInfoProvider> mFileTypeProviders;

		private List<IDocumentTypeProvider> mDocumentTypeProviders;

		private List<IActionProvider> mActionProviders;

		private List<IAsyncTaskProvider> mAsyncTaskProviders;

		private List<IToolBarProvider> mToolBarProviders;

		private List<VMMenuItem> mCustomMenuItems;

		private List<VMMenuItem> mCustomToolBarItems;

		private Dictionary<OutputMessageTarget, Dictionary<ErrorMessageType, List<IErrorItem>>> mErrorMessages;

		private Dictionary<string, string> mOutputMessages;

		private RunScriptWindow CurrentRunScriptWindow;

		private bool mUserCollapsed;

		private bool mSuppressOutputExpanderHeightStore;

		private List<IAsyncTask> mCurrentTasks;

		private WeakReference mSelectedTreeViewItem;

		internal Grid topGrid;

		internal Menu mainMenu;

		internal MenuItem fileMenuItem;

		internal MenuItem fileClose;

		internal MenuItem fileCloseAll;

		internal Separator fileSep1;

		internal MenuItem fileSaveItem;

		internal MenuItem fileSaveAllItem;

		internal MenuItem fileSaveOutputItem;

		internal Separator fileSep2;

		internal Separator fileSep3;

		internal MenuItem fileExitItem;

		internal MenuItem editMenuItem;

		internal MenuItem toolsMenuItem;

		internal MenuItem menuItemOptions;

		internal MenuItem menuItemAddInManager;

		internal MenuItem helpMenuItem;

		internal Grid fullGrid;

		internal ToolBarTray toolBarTray;

		internal Grid contentGrid;

		internal RowDefinition mainContentRow;

		internal RowDefinition outputExpanderRow;

		internal DocumentTabControl documentTabControl;

		internal Expander outputExpander;

		internal TabControl outputTabControl;

		internal ToggleButton errorsToggleButton;

		internal TextBlock errorsButtonText;

		internal ToggleButton warningsToggleButton;

		internal TextBlock warningsButtonText;

		internal ToggleButton messagesToggleButton;

		internal TextBlock messagesButtonText;

		internal ListView listView1;

		internal TabItem outputTab;

		internal ComboBox outputSources;

		internal TextBox outputTextBox;

		internal DirectoryTreeView directoryTreeView;

		internal TypeEditFileIconUriProvider typeEditFileIconUriProvider;

		internal ContextMenu dirViewContextMenu;

		internal Separator ctxAdditionalItemsSep;

		internal StatusBar statusBar;

		internal StatusBarItem statusBarText;

		internal StatusBarItem progressStatusBarItem;

		internal TextBlock progressTaskText;

		internal ProgressBar progressTaskBar;

		private bool _contentLoaded;

		public MainWindow()
		{
			mSelectedTreeViewItem = new WeakReference(null);
			mFileTypeProviders = new List<IFileTypeInfoProvider>();
			mDocumentTypeProviders = new List<IDocumentTypeProvider>();
			mActionProviders = new List<IActionProvider>();
			mAsyncTaskProviders = new List<IAsyncTaskProvider>();
			mToolBarProviders = new List<IToolBarProvider>();
			mCurrentTasks = new List<IAsyncTask>();
			mErrorMessages = new Dictionary<OutputMessageTarget, Dictionary<ErrorMessageType, List<IErrorItem>>>();
			mCustomMenuItems = new List<VMMenuItem>();
			mCustomToolBarItems = new List<VMMenuItem>();
			foreach (OutputMessageTarget value in Enum.GetValues(typeof(OutputMessageTarget)))
			{
				mErrorMessages.Add(value, new Dictionary<ErrorMessageType, List<IErrorItem>>());
				mErrorMessages[value].Add(ErrorMessageType.Error, new List<IErrorItem>());
				mErrorMessages[value].Add(ErrorMessageType.Warning, new List<IErrorItem>());
				mErrorMessages[value].Add(ErrorMessageType.Information, new List<IErrorItem>());
			}
			mOutputMessages = new Dictionary<string, string>();
			InitializeComponent();
			documentTabControl.DocumentFileTypeMap = mDocumentTypeProviders;
			NameScope.SetNameScope((DependencyObject)(object)dirViewContextMenu, NameScope.GetNameScope((DependencyObject)(object)this));
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
		}

		protected override void OnLocationChanged(EventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			Settings.Default.WindowRect2 = RectVisualToNative(((Window)this).RestoreBounds);
			base.OnLocationChanged(e);
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			Settings.Default.WindowRect2 = RectVisualToNative(((Window)this).RestoreBounds);
			base.OnRenderSizeChanged(sizeInfo);
		}

		protected override void OnStateChanged(EventArgs e)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			Settings.Default.Maximized = (int)((Window)this).WindowState == 2;
			base.OnStateChanged(e);
		}

		private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ThreadCount")
			{
				updateThreadCount();
			}
			else if (e.PropertyName == "DeploymentFolder")
			{
				updateTreeView();
			}
		}

		private void updateThreadCount()
		{
			AppServices.Exporter.MaxThreads = Settings.Default.ThreadCount;
		}

		private void updateTreeView()
		{
			bool flag = false;
			bool flag2 = false;
			List<object> list = new List<object>();
			foreach (object item in (IEnumerable)((ItemsControl)directoryTreeView).Items)
			{
				bool flag3 = false;
				if (item is DirectoryTreeViewItem directoryTreeViewItem)
				{
					if (directoryTreeViewItem.ActualPath == AppServices.ResourceManager.SourcePath)
					{
						flag3 = true;
						flag = true;
					}
					if (directoryTreeViewItem.ActualPath == AppServices.ResourceManager.DevSourcePath)
					{
						flag3 = true;
						flag2 = true;
					}
				}
				if (!flag3)
				{
					list.Add(item);
				}
			}
			foreach (object item2 in list)
			{
				((ItemsControl)directoryTreeView).Items.Remove(item2);
			}
			if (!flag && Directory.Exists(AppServices.ResourceManager.SourcePath))
			{
				DirectoryTreeViewItem directoryTreeViewItem2 = new DirectoryTreeViewItem();
				directoryTreeViewItem2.ActualPath = AppServices.ResourceManager.SourcePath;
				directoryTreeViewItem2.IsEditable = false;
				((ItemsControl)directoryTreeView).Items.Add((object)directoryTreeViewItem2);
				flag = true;
			}
			if (!flag2 && Directory.Exists(AppServices.ResourceManager.DevSourcePath))
			{
				DirectoryTreeViewItem directoryTreeViewItem2 = new DirectoryTreeViewItem();
				directoryTreeViewItem2.ActualPath = AppServices.ResourceManager.DevSourcePath;
				directoryTreeViewItem2.IsEditable = false;
				directoryTreeViewItem2.DisplayName = "Source (DEV)";
				((ItemsControl)directoryTreeView).Items.Add((object)directoryTreeViewItem2);
				flag2 = true;
			}
			if (!flag && !flag2)
			{
				DirectoryTreeViewItem directoryTreeViewItem2 = new DirectoryTreeViewItem();
				((HeaderedItemsControl)directoryTreeViewItem2).Header = "Invalid source path!";
				((ItemsControl)directoryTreeView).Items.Add((object)directoryTreeViewItem2);
			}
		}

		private void OutputManager_errorReported(object sender, ErrorReportedEventArgs e)
		{
			writeError(e.ErrorItem);
		}

		private void outputExpander_Collapsed(object sender, RoutedEventArgs e)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (!mSuppressOutputExpanderHeightStore)
			{
				Settings.Default.OutputExpandedHeight = ((FrameworkElement)outputExpander).ActualHeight;
			}
			outputExpanderRow.Height = new GridLength(24.0);
			mUserCollapsed = true;
		}

		private void outputExpander_Expanded(object sender, RoutedEventArgs e)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (mUserCollapsed)
			{
				if (!mSuppressOutputExpanderHeightStore)
				{
					mainContentRow.Height = new GridLength(((FrameworkElement)contentGrid).ActualHeight - 6.0 - Settings.Default.OutputExpandedHeight, (GridUnitType)2);
					outputExpanderRow.Height = new GridLength(Settings.Default.OutputExpandedHeight, (GridUnitType)2);
				}
				mUserCollapsed = false;
			}
		}

		private void GridSplitter_DragStarted(object sender, DragStartedEventArgs e)
		{
			if (outputExpander.IsExpanded)
			{
				Settings.Default.OutputExpandedHeight = ((FrameworkElement)outputExpander).ActualHeight;
			}
		}

		private void GridSplitter_DragDelta(object sender, DragDeltaEventArgs e)
		{
			if (((FrameworkElement)outputExpander).ActualHeight == outputExpanderRow.MinHeight)
			{
				mSuppressOutputExpanderHeightStore = true;
				outputExpander.IsExpanded = false;
				mSuppressOutputExpanderHeightStore = false;
			}
			else
			{
				mSuppressOutputExpanderHeightStore = true;
				outputExpander.IsExpanded = true;
				mSuppressOutputExpanderHeightStore = false;
			}
		}

		private void menuItemExit_Click(object sender, RoutedEventArgs e)
		{
			((Window)this).Close();
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			if (Settings.Default.Maximized)
			{
				((Window)this).WindowState = (WindowState)2;
			}
			base.OnSourceInitialized(e);
		}

		protected override void OnInitialized(EventArgs e)
		{
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Expected O, but got Unknown
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			Rectangle windowRect = Settings.Default.WindowRect2;
			bool flag = false;
			if (windowRect.X != -1 && windowRect.Y != -1 && windowRect.Width != 0 && windowRect.Height != 0)
			{
				flag = true;
			}
			if (flag)
			{
				System.Windows.Forms.Screen val = System.Windows.Forms.Screen.FromRectangle(windowRect);
				if (val == null)
				{
					val = Screen.PrimaryScreen;
				}
				if (val != null)
				{
					int val2 = Math.Max(val.WorkingArea.Left, windowRect.X);
					int val3 = Math.Max(val.WorkingArea.Top, windowRect.Y);
					int num = Math.Max(200, windowRect.Width);
					int num2 = Math.Max(200, windowRect.Height);
					val2 = Math.Min(val.WorkingArea.Right - num, val2);
					val3 = Math.Min(val.WorkingArea.Bottom - num2, val3);
					Rect val4 = RectNativeToVisual(new Rectangle(val2, val3, num, num2));
					((Window)this).Left = ((Rect)(val4)).Left;
					((Window)this).Top = ((Rect)(val4)).Top;
					((FrameworkElement)this).Width = ((Rect)(val4)).Width;
					((FrameworkElement)this).Height = ((Rect)(val4)).Height;
				}
			}
			mUserCollapsed = !Settings.Default.OutputIsExpanded;
			((Window)this).Title = (Application.Current.Resources[(object)"appTitle"] as string) + " - " + RailWorks.Properties.Version;
			mAddInManager = Application.Current.Resources[(object)"addInManager"] as AddInManager;
			foreach (IAddIn addIn in mAddInManager.AddIns)
			{
				if (!addIn.Initialise())
				{
					continue;
				}
				string name = addIn.GetType().Name;
				try
				{
					name = addIn.Name;
					if (addIn.FileTypeInfoProvider != null)
					{
						mFileTypeProviders.Add(addIn.FileTypeInfoProvider);
					}
					if (addIn.DocumentTypeProvider != null)
					{
						mDocumentTypeProviders.Add(addIn.DocumentTypeProvider);
					}
					if (addIn.ActionProvider != null)
					{
						mActionProviders.Add(addIn.ActionProvider);
					}
					if (addIn.AsyncTaskProvider != null)
					{
						mAsyncTaskProviders.Add(addIn.AsyncTaskProvider);
						addIn.AsyncTaskProvider.TaskAdded += asTaskProvider_TaskAdded;
					}
					if (addIn.ToolBarProvider != null)
					{
						mToolBarProviders.Add(addIn.ToolBarProvider);
					}
					if (addIn.ExportItemModifierProvider != null)
					{
						AppServices.Exporter.AddExportItemModifierProvider(addIn.ExportItemModifierProvider);
					}
				}
				catch (Exception e2)
				{
					AppServices.ErrorManager.ErrorMessage("An exception occurred trying to create AddIn: " + name, e2);
				}
			}
			mFileTypeProviders.Add(new TypeEditFileTypeInfoProvider());
			mDocumentTypeProviders.Add(new S9BEDocumentTypeProvider());
			mFileTypeProviders.Add(new LuaScriptFileTypeInfo());
			ExportContextActionsProvider exportContextActionsProvider = new ExportContextActionsProvider();
			mActionProviders.Add(exportContextActionsProvider);
			mAsyncTaskProviders.Add(exportContextActionsProvider);
			mToolBarProviders.Add(exportContextActionsProvider);
			exportContextActionsProvider.TaskAdded += asTaskProvider_TaskAdded;
			((ApplicationSettingsBase)Settings.Default).PropertyChanged += Default_PropertyChanged;
			AppServices.MainWindow = this;
			AppServices.OutputManager.ErrorReported += OutputManager_errorReported;
			AppServices.OutputManager.NotifyClearErrors += OutputManager_notifyClearErrors;
			AppServices.OutputManager.OutputWritten += OutputManager_outputWritten;
			AppServices.OutputManager.NotifyClearOutput += OutputManager_notifyClearOutput;
			AppServices.OutputManager.NotifyShowOutput += OutputManager_NotifyShowOutput;
			AppServices.FileManager.RequestOpenFileExternally += FileManager_RequestOpenFileExternally;
			AppServices.FileManager.RequestOpenFileInternally += FileManager_RequestOpenFileInternally;
			AppServices.FileManager.RequestShowInTree += FileManager_RequestShowInTree;
			((Selector)documentTabControl).SelectionChanged += new SelectionChangedEventHandler(documentTabControl_SelectionChanged);
			updateTreeView();
			updateOutputList();
			updateThreadCount();
			createCustomMenuItems();
			createToolBars();
			base.OnInitialized(e);
		}

		private void createToolBars()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Expected O, but got Unknown
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Expected O, but got Unknown
			foreach (IToolBarProvider mToolBarProvider in mToolBarProviders)
			{
				foreach (IToolBar toolBar in mToolBarProvider.ToolBars)
				{
					IEnumerable<IActionItem> actions = toolBar.Actions;
					if (actions == null)
					{
						continue;
					}
					ToolBar val = new ToolBar();
					foreach (IActionItem item in actions)
					{
						if (item is ISeparatorActionItem)
						{
							Separator val2 = new Separator();
							((ItemsControl)val).Items.Add((object)val2);
							continue;
						}
						if (item is IDocumentActionItem actionItem)
						{
							VMDocumentMenuItem vMDocumentMenuItem = new VMDocumentMenuItem(actionItem);
							Button val3 = new Button();
							((FrameworkElement)val3).DataContext = vMDocumentMenuItem;
							object obj = Application.Current.Resources[(object)"buttonActionItemStyle"];
							((FrameworkElement)val3).Style = (Style)((obj is Style) ? obj : null);
							((ItemsControl)val).Items.Add((object)val3);
							mCustomToolBarItems.Add(vMDocumentMenuItem);
						}
						if (item is IApplicationActionItem actionItem2)
						{
							VMAppMenuItem vMAppMenuItem = new VMAppMenuItem(actionItem2);
							Button val4 = new Button();
							((FrameworkElement)val4).DataContext = vMAppMenuItem;
							object obj2 = Application.Current.Resources[(object)"buttonActionItemStyle"];
							((FrameworkElement)val4).Style = (Style)((obj2 is Style) ? obj2 : null);
							((ItemsControl)val).Items.Add((object)val4);
							mCustomToolBarItems.Add(vMAppMenuItem);
						}
						if (item is IFileActionItem actionItem3)
						{
							VMFileMenuItem vMFileMenuItem = new VMFileMenuItem(actionItem3);
							Button val5 = new Button();
							((FrameworkElement)val5).DataContext = vMFileMenuItem;
							object obj3 = Application.Current.Resources[(object)"buttonActionItemStyle"];
							((FrameworkElement)val5).Style = (Style)((obj3 is Style) ? obj3 : null);
							((ItemsControl)val).Items.Add((object)val5);
							mCustomToolBarItems.Add(vMFileMenuItem);
						}
					}
					toolBarTray.ToolBars.Add(val);
				}
			}
		}

		private void documentTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			foreach (VMMenuItem mCustomMenuItem in mCustomMenuItems)
			{
				if (mCustomMenuItem is VMDocumentMenuItem vMDocumentMenuItem)
				{
					vMDocumentMenuItem.Update(documentTabControl.CurrentFocusedDocument);
				}
			}
			foreach (VMMenuItem mCustomToolBarItem in mCustomToolBarItems)
			{
				if (mCustomToolBarItem is VMDocumentMenuItem vMDocumentMenuItem2)
				{
					vMDocumentMenuItem2.Update(documentTabControl.CurrentFocusedDocument);
				}
			}
		}

		private void createCustomMenuItems()
		{
			foreach (IActionProvider mActionProvider in mActionProviders)
			{
				IEnumerable<IActionItem> items = mActionProvider.Items;
				if (items == null)
				{
					continue;
				}
				foreach (IActionItem item in items)
				{
					if (item is IDocumentActionItem actionItem)
					{
						mCustomMenuItems.Add(new VMDocumentMenuItem(actionItem));
					}
					if (item is IApplicationActionItem actionItem2)
					{
						mCustomMenuItems.Add(new VMAppMenuItem(actionItem2));
					}
					if (item is IFileActionItem actionItem3)
					{
						mCustomMenuItems.Add(new VMFileMenuItem(actionItem3));
					}
				}
			}
			foreach (VMMenuItem mCustomMenuItem in mCustomMenuItems)
			{
				createCustomInputBindings(mCustomMenuItem);
			}
		}

		private void createCustomInputBindings(VMMenuItem item)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			if (item.ActionItem is IWPFActionItem iWPFActionItem)
			{
				IList<InputGesture> wPFInputGestures = iWPFActionItem.GetWPFInputGestures();
				if (wPFInputGestures != null)
				{
					foreach (InputGesture item2 in wPFInputGestures)
					{
						if (item is VMFileMenuItem)
						{
							((UIElement)directoryTreeView).InputBindings.Add(new InputBinding(item.Command, item2));
						}
						else
						{
							((UIElement)this).InputBindings.Add(new InputBinding(item.Command, item2));
						}
					}
				}
			}
			if (item.Items == null)
			{
				return;
			}
			foreach (VMMenuItem item3 in item.Items)
			{
				createCustomInputBindings(item3);
			}
		}

		private void OutputManager_NotifyShowOutput(object sender, OutputShowEventArgs e)
		{
			((Selector)outputTabControl).SelectedItem = outputTab;
			if (!((CollectionView)((ItemsControl)outputSources).Items).Contains((object)e.Filter))
			{
				mOutputMessages.Add(e.Filter, string.Empty);
				((ItemsControl)outputSources).Items.Add((object)e.Filter);
			}
			((Selector)outputSources).SelectedItem = e.Filter;
		}

		private void FileManager_RequestShowInTree(object sender, FileManagerEventArgs e)
		{
			if (directoryTreeView.SelectItem(e.FileName))
			{
				e.Handled = true;
			}
		}

		private void FileManager_RequestOpenFileInternally(object sender, FileManagerEventArgs e)
		{
			if (openFileAsDocument(e.FileName))
			{
				e.Handled = true;
			}
		}

		private void FileManager_RequestOpenFileExternally(object sender, FileManagerEventArgs e)
		{
			if (openFileExternally(e.FileName))
			{
				e.Handled = true;
			}
		}

		private void OutputManager_notifyClearOutput(object sender, OutputClearedEventArgs e)
		{
			outputTextBox.Text = string.Empty;
			if (e.Filter == null)
			{
				return;
			}
			mOutputMessages.Remove(e.Filter);
			foreach (KeyValuePair<ErrorMessageType, List<IErrorItem>> item in mErrorMessages[OutputMessageTarget.Build])
			{
				item.Value.RemoveAll((IErrorItem err) => err.Filter == e.Filter);
			}
			((ItemsControl)outputSources).Items.Remove((object)e.Filter);
			updateOutputList();
		}

		private void asTaskProvider_TaskAdded(object sender, AsyncTaskEventArgs e)
		{
			if (!mCurrentTasks.Contains(e.Task))
			{
				mCurrentTasks.Add(e.Task);
				e.Task.ProgressChanged += Task_ProgressChanged;
				e.Task.TaskComplete += Task_TaskComplete;
				e.Task.Run();
				updateTaskProgress();
			}
		}

		private void updateTaskProgress()
		{
			float num = 0f;
			foreach (IAsyncTask mCurrentTask in mCurrentTasks)
			{
				num += Math.Max(0f, Math.Min(1f, mCurrentTask.Progress));
			}
			if (mCurrentTasks.Count > 1)
			{
				progressTaskText.Text = mCurrentTasks.Count + " Tasks...";
				((UIElement)progressStatusBarItem).Visibility = (Visibility)0;
			}
			else if (mCurrentTasks.Count == 1)
			{
				progressTaskText.Text = mCurrentTasks[0].ProgressText;
				((UIElement)progressStatusBarItem).Visibility = (Visibility)0;
			}
			else
			{
				((UIElement)progressStatusBarItem).Visibility = (Visibility)2;
			}
			((RangeBase)progressTaskBar).Value = num;
			progressTaskBar.IsIndeterminate = num == 0f;
		}

		private void Task_TaskComplete(object sender, EventArgs e)
		{
			if (sender is IAsyncTask item)
			{
				mCurrentTasks.Remove(item);
				updateTaskProgress();
			}
		}

		private void Task_ProgressChanged(object sender, EventArgs e)
		{
			updateTaskProgress();
		}

		private void OutputManager_notifyClearErrors(object sender, ErrorsClearedEventArgs e)
		{
			foreach (KeyValuePair<ErrorMessageType, List<IErrorItem>> item in mErrorMessages[e.Target])
			{
				if (e.Document != null)
				{
					item.Value.RemoveAll((IErrorItem item) => item.ErrorSource is DocumentErrorData documentErrorData && documentErrorData.Document == e.Document);
				}
				else
				{
					item.Value.Clear();
				}
			}
			updateOutputList();
		}

		private void writeOutput(string filter, string text)
		{
			if (!mOutputMessages.ContainsKey(filter))
			{
				mOutputMessages.Add(filter, string.Empty);
				((ItemsControl)outputSources).Items.Add((object)filter);
				if (((Selector)outputSources).SelectedIndex == -1)
				{
					((Selector)outputSources).SelectedIndex = 0;
				}
			}
			string text2 = text + Environment.NewLine;
			int num = mOutputMessages[filter].Length + text2.Length - 262144;
			if (num > 0)
			{
				num = (int)((double)mOutputMessages[filter].Length * 0.25);
				mOutputMessages[filter] = mOutputMessages[filter].Substring(num);
			}
			mOutputMessages[filter] += text2;
			if (((Selector)outputSources).SelectedItem is string text3 && text3 == filter)
			{
				if (outputTextBox.Text == null)
				{
					outputTextBox.Text = string.Empty;
				}
				bool flag = outputTextBox.SelectionStart == outputTextBox.Text.Length;
				if (num > 0)
				{
					outputTextBox.Text = outputTextBox.Text.Substring(num);
				}
				((TextBoxBase)outputTextBox).AppendText(text2);
				if (flag)
				{
					outputTextBox.SelectionStart = outputTextBox.Text.Length;
					((TextBoxBase)outputTextBox).ScrollToEnd();
				}
			}
		}

		private void clearCurrentOutputFilter()
		{
			outputTextBox.Text = string.Empty;
			string selFilter = ((Selector)outputSources).SelectedItem as string;
			if (selFilter == null)
			{
				return;
			}
			mOutputMessages.Remove(selFilter);
			foreach (KeyValuePair<ErrorMessageType, List<IErrorItem>> item in mErrorMessages[OutputMessageTarget.Build])
			{
				item.Value.RemoveAll((IErrorItem err) => err.Filter == selFilter);
			}
			((ItemsControl)outputSources).Items.Remove((object)selFilter);
			updateOutputList();
		}

		private void OutputManager_outputWritten(object sender, OutputReportedEventArgs e)
		{
			writeOutput(e.Filter, e.Line);
		}

		private void writeError(IErrorItem errorMessage)
		{
			mErrorMessages[errorMessage.Target][errorMessage.MessageType].Add(errorMessage);
			updateOutputList();
			if (errorMessage.Filter != null)
			{
				writeOutput(errorMessage.Filter, errorMessage.Message);
			}
		}

		private void updateOutputList()
		{
			if (!((FrameworkElement)this).IsInitialized)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			List<VMOutputMessage> list = new List<VMOutputMessage>();
			foreach (KeyValuePair<OutputMessageTarget, Dictionary<ErrorMessageType, List<IErrorItem>>> mErrorMessage in mErrorMessages)
			{
				foreach (KeyValuePair<ErrorMessageType, List<IErrorItem>> item2 in mErrorMessage.Value)
				{
					bool flag = false;
					switch (item2.Key)
					{
						case ErrorMessageType.Error:
							num += item2.Value.Count;
							flag = errorsToggleButton.IsChecked ?? false;
							break;
						case ErrorMessageType.Warning:
							num2 += item2.Value.Count;
							flag = warningsToggleButton.IsChecked ?? false;
							break;
						case ErrorMessageType.Information:
							num3 += item2.Value.Count;
							flag = messagesToggleButton.IsChecked ?? false;
							break;
					}
					if (!flag)
					{
						continue;
					}
					foreach (IErrorItem item3 in item2.Value)
					{
						VMOutputMessage item = new VMOutputMessage(item3);
						list.Add(item);
					}
				}
			}
			errorsButtonText.Text = string.Format(CultureInfo.CurrentCulture, "{0} Errors", new object[1] { num });
			warningsButtonText.Text = string.Format(CultureInfo.CurrentCulture, "{0} Warnings", new object[1] { num2 });
			messagesButtonText.Text = string.Format(CultureInfo.CurrentCulture, "{0} Messages", new object[1] { num3 });
			((ItemsControl)listView1).ItemsSource = list;
		}

		private void menuItemOptions_Click(object sender, RoutedEventArgs e)
		{
			OptionsWindow optionsWindow = new OptionsWindow();
			((Window)optionsWindow).Owner = (Window)(object)this;
			((Window)optionsWindow).ShowDialog();
		}

		private void directoryTreeView_FileSelected(object sender, DirectoryTreeViewFileSelectedArgs e)
		{
			OpenFile(e.FileName);
		}

		public void OpenFile(string fileName)
		{
			if (!openFileAsDocument(fileName))
			{
				openFileExternally(fileName);
			}
		}

		private bool openFileExternally(string fileName)
		{
			try
			{
				Process.Start(fileName);
			}
			catch (Exception e)
			{
				AppServices.ErrorManager.ErrorMessage("Failed to run associated process for " + fileName, e);
				return false;
			}
			return true;
		}

		private bool openFileAsDocument(string fileName)
		{
			IDocument document = documentTabControl.LoadDocument(fileName);
			if (document != null)
			{
				documentTabControl.FocusContent();
			}
			return document != null;
		}

		protected override void OnClosed(EventArgs e)
		{
			foreach (IAsyncTaskProvider mAsyncTaskProvider in mAsyncTaskProviders)
			{
				mAsyncTaskProvider.TaskAdded -= asTaskProvider_TaskAdded;
			}
			((ApplicationSettingsBase)Settings.Default).PropertyChanged -= Default_PropertyChanged;
			AppServices.OutputManager.ErrorReported -= OutputManager_errorReported;
			AppServices.OutputManager.NotifyClearErrors -= OutputManager_notifyClearErrors;
			AppServices.OutputManager.OutputWritten -= OutputManager_outputWritten;
			AppServices.OutputManager.NotifyClearOutput -= OutputManager_notifyClearOutput;
			AppServices.FileManager.RequestOpenFileExternally -= FileManager_RequestOpenFileExternally;
			AppServices.FileManager.RequestOpenFileInternally -= FileManager_RequestOpenFileInternally;
			AppServices.FileManager.RequestShowInTree -= FileManager_RequestShowInTree;
			AppServices.ShapeViewer.Close();
			AppServices.MainWindow = null;
			base.OnClosed(e);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			e.Cancel = !documentTabControl.CloseAll();
			base.OnClosing(e);
		}

		private void cut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.CanCut)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void cut_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			try
			{
				if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem)
				{
					string[] array = new string[1] { directoryTreeViewItem.ActualPath };
					IDataObject val = (IDataObject)new DataObject(DataFormats.FileDrop, (object)array);
					MemoryStream memoryStream = new MemoryStream(4);
					byte[] array2 = new byte[4] { 2, 0, 0, 0 };
					memoryStream.Write(array2, 0, array2.Length);
					val.SetData("Preferred DropEffect", (object)memoryStream);
					Clipboard.SetDataObject((object)val);
				}
				((RoutedEventArgs)e).Handled = true;
			}
			catch (Exception e2)
			{
				AppServices.ErrorManager.ErrorMessage("Could not cut file", e2);
			}
		}

		private void copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.CanCopy)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void copy_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			try
			{
				if (!(mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem))
				{
					return;
				}
				string[] array = new string[1] { directoryTreeViewItem.ActualPath };
				IDataObject val = (IDataObject)new DataObject(DataFormats.FileDrop, (object)array);
				MemoryStream memoryStream = new MemoryStream(4);
				byte[] array2 = new byte[4] { 5, 0, 0, 0 };
				memoryStream.Write(array2, 0, array2.Length);
				val.SetData("Preferred DropEffect", (object)memoryStream);
				DataUtil.GetProviderProductPath(directoryTreeViewItem.RelativePath, out var provider, out var product, out var outRelativePath);
				if (outRelativePath != null)
				{
					val.SetData(DataFormats.Text, (object)outRelativePath);
				}
				try
				{
					if (Path.GetExtension(directoryTreeViewItem.ActualPath).Equals(".xml", StringComparison.CurrentCultureIgnoreCase) && AppServices.TypeEditSchema.GetRootTypeFromFile(directoryTreeViewItem.ActualPath) != null)
					{
						ITypeDatum typeDatum = AppServices.TypeEditSchema.CreateDatum("iBlueprintLibrary-cAbsoluteBlueprintID");
						if (typeDatum != null)
						{
							TypeEditSchemaUtil.SetBlueprintID(typeDatum, provider, product, outRelativePath);
							string datumXML = TypeEditSchemaUtil.GetDatumXML(typeDatum);
							if (val != null)
							{
								val.SetData(TypeEditSchemaUtil.DatumXMLClipboardFormat, (object)datumXML);
							}
						}
						((RoutedEventArgs)e).Handled = true;
					}
				}
				catch (Exception e2)
				{
					AppServices.ErrorManager.ErrorMessage("Could not copy Blueprint ID", e2);
				}
				Clipboard.SetDataObject((object)val);
				((RoutedEventArgs)e).Handled = true;
			}
			catch (Exception e3)
			{
				AppServices.ErrorManager.ErrorMessage("Could not copy file", e3);
			}
		}

		private void paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.CanPaste && Clipboard.ContainsData(DataFormats.FileDrop) && ((TreeView)directoryTreeView).SelectedItem is DirectoryTreeViewItem)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void paste_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				if (!(mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem) || !Clipboard.ContainsData(DataFormats.FileDrop))
				{
					return;
				}
				StringCollection fileDropList = Clipboard.GetFileDropList();
				MemoryStream memoryStream = Clipboard.GetData("Preferred DropEffect") as MemoryStream;
				byte[] array = new byte[4];
				if (memoryStream.Length == 4)
				{
					memoryStream.Read(array, 0, 4);
				}
				bool flag = array[0] == 2;
				if (flag)
				{
					Clipboard.Clear();
				}
				StringEnumerator enumerator = fileDropList.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						string text = directoryTreeViewItem.ActualPath;
						if (!directoryTreeViewItem.IsDirectory)
						{
							text = Path.GetDirectoryName(text);
						}
						FileAttributes attributes = File.GetAttributes(current);
						string text2;
						if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
						{
							text2 = Path.Combine(text, Path.GetFileName(current));
							if (!Directory.Exists(text2))
							{
								Directory.CreateDirectory(text2);
							}
							if (flag)
							{
								FileSystem.MoveDirectory(current, text2, UIOption.OnlyErrorDialogs);
							}
							else
							{
								FileSystem.CopyDirectory(current, text2, UIOption.OnlyErrorDialogs);
							}
							continue;
						}
						string text3 = Path.Combine(text, Path.GetFileName(current));
						text2 = text3;
						if (flag && DataUtil.PathsEqual(current, text2))
						{
							continue;
						}
						if (File.Exists(text2))
						{
							string directoryName = Path.GetDirectoryName(text3);
							string extension = Path.GetExtension(text3);
							string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text3);
							for (int i = 1; i < 1000; i++)
							{
								text2 = Path.Combine(directoryName, string.Join(string.Empty, new string[4]
								{
									fileNameWithoutExtension,
									"_",
									i.ToString("D4", CultureInfo.InvariantCulture),
									extension
								}));
								if (!File.Exists(text2))
								{
									break;
								}
							}
						}
						if (flag)
						{
							FileSystem.MoveFile(current, text2, UIOption.OnlyErrorDialogs);
							((RoutedEventArgs)e).Handled = true;
						}
						else
						{
							FileSystem.CopyFile(current, text2, UIOption.OnlyErrorDialogs);
							((RoutedEventArgs)e).Handled = true;
						}
					}
				}
				finally
				{
					if (enumerator is IDisposable disposable)
					{
						disposable.Dispose();
					}
				}
			}
			catch (Exception e2)
			{
				AppServices.ErrorManager.ErrorMessage("Could not paste file", e2);
			}
		}

		private void delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.CanDelete)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void delete_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem)
				{
					if (directoryTreeViewItem.IsDirectory)
					{
						FileSystem.DeleteDirectory(directoryTreeViewItem.ActualPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
					}
					else
					{
						FileSystem.DeleteFile(directoryTreeViewItem.ActualPath, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
					}
				}
			}
			catch (Exception e2)
			{
				AppServices.ErrorManager.ErrorMessage("Could not delete file", e2);
			}
		}

		private void open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.CanOpen)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			directoryTreeView.OpenItem(mSelectedTreeViewItem.Target as DirectoryTreeViewItem);
		}

		private void properties_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.IsValid)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void properties_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.IsValid)
			{
				try
				{
					Native.ShowFileProperties(directoryTreeViewItem.ActualPath);
				}
				catch (Exception e2)
				{
					AppServices.ErrorManager.ErrorMessage("Could not show properties", e2);
				}
			}
		}

		private void addItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!(mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.CanAdd && directoryTreeViewItem.IsDirectory))
			{
				return;
			}
			bool flag = false;
			foreach (IFileTypeInfoProvider mFileTypeProvider in mFileTypeProviders)
			{
				if (mFileTypeProvider.FileTypes == null)
				{
					continue;
				}
				foreach (IFileTypeInfo fileType in mFileTypeProvider.FileTypes)
				{
					if (fileType is IEditableFileTypeInfo editableFileTypeInfo && editableFileTypeInfo.ValidFileLocation(directoryTreeViewItem.GetTopMostTreeViewItem().ActualPath, directoryTreeViewItem.RelativePath))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			e.CanExecute = flag;
			((RoutedEventArgs)e).Handled = true;
		}

		private void addItem_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (!(mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.IsDirectory))
			{
				return;
			}
			AddItemWindow addItemWindow = new AddItemWindow();
			((Window)addItemWindow).Owner = (Window)(object)this;
			Dictionary<string, FileTypeCategory> dictionary = new Dictionary<string, FileTypeCategory>();
			foreach (IFileTypeInfoProvider mFileTypeProvider in mFileTypeProviders)
			{
				if (mFileTypeProvider.FileTypes == null)
				{
					continue;
				}
				foreach (IFileTypeInfo fileType in mFileTypeProvider.FileTypes)
				{
					if (fileType is IEditableFileTypeInfo editableFileTypeInfo && editableFileTypeInfo.ValidFileLocation(directoryTreeViewItem.GetTopMostTreeViewItem().ActualPath, directoryTreeViewItem.RelativePath))
					{
						if (!dictionary.TryGetValue(editableFileTypeInfo.FileCategory, out var value))
						{
							value = new FileTypeCategory(editableFileTypeInfo.FileCategory);
							dictionary.Add(editableFileTypeInfo.FileCategory, value);
						}
						value.Items.Add(new FileType(editableFileTypeInfo));
					}
				}
			}
			addItemWindow.ItemsSource = dictionary.Values;
			if (((Window)addItemWindow).ShowDialog() ?? false)
			{
				FileType selectedFileType = addItemWindow.SelectedFileType;
				string text = Path.Combine(directoryTreeViewItem.ActualPath, addItemWindow.FileName);
				if (File.Exists(text))
				{
					string debugText = "Failed to create file \"" + Path.GetFileName(text) + "\": File already exists!";
					AppServices.OutputManager.WriteOutput("Editor", debugText);
				}
				else if (selectedFileType.FileTypeInfo.TryCreateFile(text))
				{
					directoryTreeView.SelectItem(text);
					OpenFile(text);
				}
			}
		}

		private void addFolder_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.CanAdd && directoryTreeViewItem.IsDirectory)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void addFolder_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (!(mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.IsDirectory))
			{
				return;
			}
			string text = "New Folder";
			string text2 = null;
			for (int i = 1; i < 100; i++)
			{
				string text3 = text;
				if (i > 1)
				{
					text3 = text3 + " " + i.ToString(CultureInfo.InvariantCulture);
				}
				string text4 = Path.Combine(directoryTreeViewItem.ActualPath, text3);
				if (!Directory.Exists(text4))
				{
					text2 = text4;
					break;
				}
			}
			if (text2 == null)
			{
				return;
			}
			try
			{
				Directory.CreateDirectory(text2);
				directoryTreeViewItem.Refresh();
				if (directoryTreeView.SelectItem(text2) && ((TreeView)directoryTreeView).SelectedItem is DirectoryTreeViewItem directoryTreeViewItem2 && DataUtil.PathsEqual(directoryTreeViewItem2.ActualPath, text2))
				{
					directoryTreeViewItem2.IsEditing = true;
				}
			}
			catch (Exception e2)
			{
				AppServices.ErrorManager.ErrorMessage("Could not create folder", e2);
			}
		}

		private void openInExplorer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void openInExplorer_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem)
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo("Explorer");
				processStartInfo.UseShellExecute = false;
				processStartInfo.Arguments = "/select,\"" + directoryTreeViewItem.ActualPath + "\"";
				try
				{
					Process.Start(processStartInfo);
				}
				catch (Exception e2)
				{
					AppServices.ErrorManager.ErrorMessage("Could not open file in explorer", e2);
				}
			}
		}

		private void rename_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.IsEditable)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void rename_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (mSelectedTreeViewItem.Target is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.IsEditable)
			{
				directoryTreeViewItem.IsEditing = true;
			}
		}

		private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
		{
			AboutWindow aboutWindow = new AboutWindow();
			((Window)aboutWindow).Owner = (Window)(object)this;
			((Window)aboutWindow).ShowDialog();
		}

		private void saveAll_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			documentTabControl.SaveAll();
		}

		private void saveAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (((ItemsControl)documentTabControl).HasItems)
			{
				e.CanExecute = true;
				((RoutedEventArgs)e).Handled = true;
			}
		}

		private void fileMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
		{
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Expected O, but got Unknown
			if (e.Source != fileMenuItem)
			{
				return;
			}
			((ItemsControl)fileMenuItem).Items.Clear();
			((ItemsControl)fileMenuItem).Items.Add((object)fileClose);
			((ItemsControl)fileMenuItem).Items.Add((object)fileCloseAll);
			((ItemsControl)fileMenuItem).Items.Add((object)fileSep1);
			((ItemsControl)fileMenuItem).Items.Add((object)fileSaveItem);
			((ItemsControl)fileMenuItem).Items.Add((object)fileSaveAllItem);
			((ItemsControl)fileMenuItem).Items.Add((object)fileSaveOutputItem);
			((ItemsControl)fileMenuItem).Items.Add((object)fileSep2);
			bool flag = false;
			if (((Selector)documentTabControl).SelectedItem is DocumentTabItem documentTabItem)
			{
				_ = documentTabItem.Document;
			}
			foreach (VMMenuItem mCustomMenuItem in mCustomMenuItems)
			{
				if (mCustomMenuItem != null && (mCustomMenuItem is VMDocumentMenuItem || mCustomMenuItem is VMAppMenuItem) && mCustomMenuItem.Command.CanExecute(null))
				{
					if (mCustomMenuItem.IsSeparator)
					{
						Separator val = new Separator();
						((FrameworkElement)val).DataContext = mCustomMenuItem;
						object obj = Application.Current.Resources[(object)"separatorActionItemStyle"];
						((FrameworkElement)val).Style = (Style)((obj is Style) ? obj : null);
						((ItemsControl)fileMenuItem).Items.Add((object)val);
					}
					else
					{
						mCustomMenuItem.UpdateText();
						flag = true;
						MenuItem val2 = new MenuItem();
						((FrameworkElement)val2).DataContext = mCustomMenuItem;
						object obj2 = Application.Current.Resources[(object)"menuItemActionItemStyle"];
						((FrameworkElement)val2).Style = (Style)((obj2 is Style) ? obj2 : null);
						object obj3 = ((FrameworkElement)this).Resources[(object)"menuItemStyleSelector"];
						((ItemsControl)val2).ItemContainerStyleSelector = (StyleSelector)((obj3 is StyleSelector) ? obj3 : null);
						((ItemsControl)fileMenuItem).Items.Add((object)val2);
					}
				}
			}
			((ItemsControl)fileMenuItem).Items.Add((object)fileSep3);
			((UIElement)fileSep3).Visibility = (Visibility)((!flag) ? 2 : 0);
			((ItemsControl)fileMenuItem).Items.Add((object)fileExitItem);
		}

		private void fileMenuItem_SubmenuClosed(object sender, RoutedEventArgs e)
		{
		}

		private void directoryTreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			DirectoryTreeViewItem directoryTreeViewItem = ((TreeView)directoryTreeView).SelectedItem as DirectoryTreeViewItem;
			mSelectedTreeViewItem = new WeakReference(directoryTreeViewItem);
			((ItemsControl)dirViewContextMenu).Items.Clear();
			if (directoryTreeViewItem == null)
			{
				return;
			}
			DirectoryTreeViewItem topMostTreeViewItem = directoryTreeViewItem.GetTopMostTreeViewItem();
			_ = topMostTreeViewItem.ActualPath;
			UIElement[] array = ((FrameworkElement)dirViewContextMenu).Resources[(object)"ctxMenuItems"] as UIElement[];
			List<object> list = new List<object>();
			foreach (VMMenuItem mCustomMenuItem in mCustomMenuItems)
			{
				if (mCustomMenuItem is VMFileMenuItem && mCustomMenuItem.Command.CanExecute(null))
				{
					mCustomMenuItem.UpdateText();
					if (mCustomMenuItem.IsSeparator)
					{
						Separator val = new Separator();
						((FrameworkElement)val).DataContext = mCustomMenuItem;
						object obj = Application.Current.Resources[(object)"separatorActionItemStyle"];
						((FrameworkElement)val).Style = (Style)((obj is Style) ? obj : null);
						list.Add(val);
					}
					else
					{
						MenuItem val2 = new MenuItem();
						((FrameworkElement)val2).DataContext = mCustomMenuItem;
						object obj2 = Application.Current.Resources[(object)"menuItemActionItemStyle"];
						((FrameworkElement)val2).Style = (Style)((obj2 is Style) ? obj2 : null);
						object obj3 = ((FrameworkElement)this).Resources[(object)"menuItemStyleSelector"];
						((ItemsControl)val2).ItemContainerStyleSelector = (StyleSelector)((obj3 is StyleSelector) ? obj3 : null);
						list.Add(val2);
					}
				}
			}
			if (array == null)
			{
				return;
			}
			UIElement[] array2 = array;
			foreach (UIElement val3 in array2)
			{
				if ((object)val3 == ctxAdditionalItemsSep)
				{
					val3.Visibility = (Visibility)((list.Count <= 0) ? 2 : 0);
					((ItemsControl)dirViewContextMenu).Items.Add((object)val3);
					foreach (object item in list)
					{
						((ItemsControl)dirViewContextMenu).Items.Add(item);
					}
				}
				else
				{
					((ItemsControl)dirViewContextMenu).Items.Add((object)val3);
				}
			}
		}

		private void errorsToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			updateOutputList();
		}

		private void warningsToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			updateOutputList();
		}

		private void informationToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			updateOutputList();
		}

		private void informationToggleButton_Unchecked(object sender, RoutedEventArgs e)
		{
			updateOutputList();
		}

		private void warningsToggleButton_Unchecked(object sender, RoutedEventArgs e)
		{
			updateOutputList();
		}

		private void errorsToggleButton_Unchecked(object sender, RoutedEventArgs e)
		{
			updateOutputList();
		}

		private void directoryTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			DirectoryTreeViewItem directoryTreeViewItem = ((TreeView)directoryTreeView).SelectedItem as DirectoryTreeViewItem;
			mSelectedTreeViewItem = new WeakReference(directoryTreeViewItem);
			if (directoryTreeViewItem != null)
			{
				DirectoryTreeViewItem topMostTreeViewItem = directoryTreeViewItem.GetTopMostTreeViewItem();
				string actualPath = topMostTreeViewItem.ActualPath;
				{
					foreach (VMMenuItem mCustomMenuItem in mCustomMenuItems)
					{
						if (mCustomMenuItem is VMFileMenuItem vMFileMenuItem)
						{
							vMFileMenuItem.Update(actualPath, directoryTreeViewItem.RelativePath, directoryTreeViewItem.IsDirectory);
						}
					}
					return;
				}
			}
			foreach (VMMenuItem mCustomMenuItem2 in mCustomMenuItems)
			{
				if (mCustomMenuItem2 is VMFileMenuItem vMFileMenuItem2)
				{
					vMFileMenuItem2.Update(null, null, isDirectory: false);
				}
			}
		}

		private void menuItemAddInManager_Click(object sender, RoutedEventArgs e)
		{
			AddInManagerWindow addInManagerWindow = new AddInManagerWindow();
			addInManagerWindow.AddInManager = mAddInManager;
			((Window)addInManagerWindow).Owner = (Window)(object)this;
			((Window)addInManagerWindow).ShowDialog();
		}

		private void saveOutput_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = outputTextBox.Text.Length > 0;
		}

		private void saveOutput_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			SaveFileDialog val = new SaveFileDialog();
			((FileDialog)val).FileName = "BPE2_Output_" + DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
			((FileDialog)val).DefaultExt = ".txt";
			((FileDialog)val).Filter = "Text Documents (.txt)|*.txt";
			if (((CommonDialog)val).ShowDialog() != true)
			{
				return;
			}

			using (FileStream stream = new FileStream(((FileDialog)val).FileName, FileMode.Create, FileAccess.Write))
			{
				using (StreamWriter streamWriter = new StreamWriter(stream))
				{
					streamWriter.Write(outputTextBox.Text);
					streamWriter.Close();
				}
			}
		}

		private void reloadBlueprintSchema_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void reloadBlueprintSchema_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			AppServices.ResourceManager.UpdateCurrentDeploymentFolder(force: true);
		}

		private void runScript_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void runScript_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (CurrentRunScriptWindow == null)
			{
				CurrentRunScriptWindow = new RunScriptWindow();
				((Window)CurrentRunScriptWindow).Owner = (Window)(object)this;
				((Window)CurrentRunScriptWindow).Show();
				((Window)CurrentRunScriptWindow).Closed += delegate
				{
					CurrentRunScriptWindow = null;
				};
			}
			else
			{
				((Window)CurrentRunScriptWindow).Activate();
			}
		}

		private void outputSources_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (((Selector)outputSources).SelectedItem is string key && mOutputMessages.TryGetValue(key, out var value))
			{
				outputTextBox.Text = value;
			}
			else
			{
				outputTextBox.Text = string.Empty;
			}
			outputTextBox.SelectionStart = outputTextBox.Text.Length;
			((TextBoxBase)outputTextBox).ScrollToEnd();
		}

		private void outputClearButton_Click(object sender, RoutedEventArgs e)
		{
			clearCurrentOutputFilter();
		}

		private void listViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DependencyObject val = (DependencyObject)((sender is DependencyObject) ? sender : null);
			if (val != null && ((ItemsControl)listView1).ItemContainerGenerator.ItemFromContainer(val) is VMOutputMessage vMOutputMessage && vMOutputMessage.ErrorItem.ErrorSource is DocumentErrorData documentErrorData)
			{
				documentTabControl.CurrentFocusedDocument = documentErrorData.Document;
				documentTabControl.CurrentFocusedDocument.SelectComponent(documentErrorData.ErrorSource);
			}
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri uri = new Uri("/BlueprintEditor2;component/mainwindow.xaml", UriKind.Relative);
				Application.LoadComponent((object)this, uri);
			}
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Expected O, but got Unknown
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Expected O, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Expected O, but got Unknown
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Expected O, but got Unknown
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Expected O, but got Unknown
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Expected O, but got Unknown
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Expected O, but got Unknown
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Expected O, but got Unknown
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Expected O, but got Unknown
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Expected O, but got Unknown
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Expected O, but got Unknown
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Expected O, but got Unknown
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Expected O, but got Unknown
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Expected O, but got Unknown
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Expected O, but got Unknown
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Expected O, but got Unknown
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Expected O, but got Unknown
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Expected O, but got Unknown
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Expected O, but got Unknown
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Expected O, but got Unknown
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Expected O, but got Unknown
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Expected O, but got Unknown
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Expected O, but got Unknown
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Expected O, but got Unknown
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e7: Expected O, but got Unknown
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Expected O, but got Unknown
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Expected O, but got Unknown
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Expected O, but got Unknown
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Expected O, but got Unknown
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Expected O, but got Unknown
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Expected O, but got Unknown
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Expected O, but got Unknown
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Expected O, but got Unknown
			//IL_037a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Expected O, but got Unknown
			//IL_0391: Unknown result type (might be due to invalid IL or missing references)
			//IL_039b: Expected O, but got Unknown
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Expected O, but got Unknown
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Expected O, but got Unknown
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Expected O, but got Unknown
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Expected O, but got Unknown
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Expected O, but got Unknown
			//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0407: Expected O, but got Unknown
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0414: Expected O, but got Unknown
			//IL_0417: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Expected O, but got Unknown
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Expected O, but got Unknown
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Expected O, but got Unknown
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Expected O, but got Unknown
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Expected O, but got Unknown
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Expected O, but got Unknown
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_049e: Expected O, but got Unknown
			//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b6: Expected O, but got Unknown
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cd: Expected O, but got Unknown
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04db: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e5: Expected O, but got Unknown
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Expected O, but got Unknown
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0514: Expected O, but got Unknown
			//IL_0515: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_052b: Expected O, but got Unknown
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Expected O, but got Unknown
			//IL_0544: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_055a: Expected O, but got Unknown
			//IL_055c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0568: Unknown result type (might be due to invalid IL or missing references)
			//IL_0572: Expected O, but got Unknown
			//IL_0573: Unknown result type (might be due to invalid IL or missing references)
			//IL_057f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Expected O, but got Unknown
			//IL_058b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a1: Expected O, but got Unknown
			//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b8: Expected O, but got Unknown
			//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d0: Expected O, but got Unknown
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Expected O, but got Unknown
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ff: Expected O, but got Unknown
			//IL_0600: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0616: Expected O, but got Unknown
			//IL_0618: Unknown result type (might be due to invalid IL or missing references)
			//IL_0624: Unknown result type (might be due to invalid IL or missing references)
			//IL_062e: Expected O, but got Unknown
			//IL_062f: Unknown result type (might be due to invalid IL or missing references)
			//IL_063b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0645: Expected O, but got Unknown
			//IL_0647: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_065d: Expected O, but got Unknown
			//IL_065e: Unknown result type (might be due to invalid IL or missing references)
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0674: Expected O, but got Unknown
			//IL_0677: Unknown result type (might be due to invalid IL or missing references)
			//IL_0681: Expected O, but got Unknown
			//IL_0684: Unknown result type (might be due to invalid IL or missing references)
			//IL_068e: Expected O, but got Unknown
			//IL_0691: Unknown result type (might be due to invalid IL or missing references)
			//IL_069b: Expected O, but got Unknown
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Expected O, but got Unknown
			//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b5: Expected O, but got Unknown
			//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Expected O, but got Unknown
			//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_06da: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e4: Expected O, but got Unknown
			//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fc: Expected O, but got Unknown
			//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_0713: Expected O, but got Unknown
			//IL_0715: Unknown result type (might be due to invalid IL or missing references)
			//IL_0721: Unknown result type (might be due to invalid IL or missing references)
			//IL_072b: Expected O, but got Unknown
			//IL_072c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0738: Unknown result type (might be due to invalid IL or missing references)
			//IL_0742: Expected O, but got Unknown
			switch (connectionId)
			{
				case 1:
					topGrid = (Grid)target;
					break;
				case 2:
					mainMenu = (Menu)target;
					break;
				case 3:
					fileMenuItem = (MenuItem)target;
					fileMenuItem.SubmenuOpened += new RoutedEventHandler(fileMenuItem_SubmenuOpened);
					fileMenuItem.SubmenuClosed += new RoutedEventHandler(fileMenuItem_SubmenuClosed);
					break;
				case 4:
					fileClose = (MenuItem)target;
					break;
				case 5:
					fileCloseAll = (MenuItem)target;
					break;
				case 6:
					fileSep1 = (Separator)target;
					break;
				case 7:
					fileSaveItem = (MenuItem)target;
					break;
				case 8:
					fileSaveAllItem = (MenuItem)target;
					break;
				case 9:
					fileSaveOutputItem = (MenuItem)target;
					break;
				case 10:
					fileSep2 = (Separator)target;
					break;
				case 11:
					fileSep3 = (Separator)target;
					break;
				case 12:
					fileExitItem = (MenuItem)target;
					fileExitItem.Click += new RoutedEventHandler(menuItemExit_Click);
					break;
				case 13:
					editMenuItem = (MenuItem)target;
					break;
				case 14:
					toolsMenuItem = (MenuItem)target;
					break;
				case 15:
					menuItemOptions = (MenuItem)target;
					menuItemOptions.Click += new RoutedEventHandler(menuItemOptions_Click);
					break;
				case 16:
					menuItemAddInManager = (MenuItem)target;
					menuItemAddInManager.Click += new RoutedEventHandler(menuItemAddInManager_Click);
					break;
				case 17:
					helpMenuItem = (MenuItem)target;
					break;
				case 18:
					((MenuItem)target).Click += new RoutedEventHandler(aboutMenuItem_Click);
					break;
				case 19:
					fullGrid = (Grid)target;
					break;
				case 20:
					toolBarTray = (ToolBarTray)target;
					break;
				case 21:
					contentGrid = (Grid)target;
					break;
				case 22:
					mainContentRow = (RowDefinition)target;
					break;
				case 23:
					outputExpanderRow = (RowDefinition)target;
					break;
				case 24:
					documentTabControl = (DocumentTabControl)target;
					break;
				case 25:
					((Thumb)(GridSplitter)target).DragStarted += new DragStartedEventHandler(GridSplitter_DragStarted);
					((Thumb)(GridSplitter)target).DragDelta += new DragDeltaEventHandler(GridSplitter_DragDelta);
					break;
				case 26:
					outputExpander = (Expander)target;
					outputExpander.Collapsed += new RoutedEventHandler(outputExpander_Collapsed);
					outputExpander.Expanded += new RoutedEventHandler(outputExpander_Expanded);
					break;
				case 27:
					outputTabControl = (TabControl)target;
					break;
				case 28:
					errorsToggleButton = (ToggleButton)target;
					errorsToggleButton.Checked += new RoutedEventHandler(errorsToggleButton_Checked);
					errorsToggleButton.Unchecked += new RoutedEventHandler(errorsToggleButton_Unchecked);
					break;
				case 29:
					errorsButtonText = (TextBlock)target;
					break;
				case 30:
					warningsToggleButton = (ToggleButton)target;
					warningsToggleButton.Checked += new RoutedEventHandler(warningsToggleButton_Checked);
					warningsToggleButton.Unchecked += new RoutedEventHandler(warningsToggleButton_Unchecked);
					break;
				case 31:
					warningsButtonText = (TextBlock)target;
					break;
				case 32:
					messagesToggleButton = (ToggleButton)target;
					messagesToggleButton.Checked += new RoutedEventHandler(informationToggleButton_Checked);
					messagesToggleButton.Unchecked += new RoutedEventHandler(informationToggleButton_Unchecked);
					break;
				case 33:
					messagesButtonText = (TextBlock)target;
					break;
				case 34:
					listView1 = (ListView)target;
					break;
				case 36:
					outputTab = (TabItem)target;
					break;
				case 37:
					outputSources = (ComboBox)target;
					((Selector)outputSources).SelectionChanged += new SelectionChangedEventHandler(outputSources_SelectionChanged);
					break;
				case 38:
					((ButtonBase)(Button)target).Click += new RoutedEventHandler(outputClearButton_Click);
					break;
				case 39:
					outputTextBox = (TextBox)target;
					break;
				case 40:
					directoryTreeView = (DirectoryTreeView)target;
					break;
				case 41:
					typeEditFileIconUriProvider = (TypeEditFileIconUriProvider)target;
					break;
				case 42:
					dirViewContextMenu = (ContextMenu)target;
					break;
				case 43:
					ctxAdditionalItemsSep = (Separator)target;
					break;
				case 44:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(addItem_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(addItem_Executed);
					break;
				case 45:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(addFolder_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(addFolder_Executed);
					break;
				case 46:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(openInExplorer_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(openInExplorer_Executed);
					break;
				case 47:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(rename_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(rename_Executed);
					break;
				case 48:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(properties_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(properties_Executed);
					break;
				case 49:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(open_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(open_Executed);
					break;
				case 50:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(cut_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(cut_Executed);
					break;
				case 51:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(copy_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(copy_Executed);
					break;
				case 52:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(paste_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(paste_Executed);
					break;
				case 53:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(delete_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(delete_Executed);
					break;
				case 54:
					statusBar = (StatusBar)target;
					break;
				case 55:
					statusBarText = (StatusBarItem)target;
					break;
				case 56:
					progressStatusBarItem = (StatusBarItem)target;
					break;
				case 57:
					progressTaskText = (TextBlock)target;
					break;
				case 58:
					progressTaskBar = (ProgressBar)target;
					break;
				case 59:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(saveOutput_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(saveOutput_Executed);
					break;
				case 60:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(reloadBlueprintSchema_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(reloadBlueprintSchema_Executed);
					break;
				case 61:
					((CommandBinding)target).CanExecute += new CanExecuteRoutedEventHandler(runScript_CanExecute);
					((CommandBinding)target).Executed += new ExecutedRoutedEventHandler(runScript_Executed);
					break;
				default:
					_contentLoaded = true;
					break;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			if (connectionId == 35)
			{
				EventSetter val = new EventSetter();
				val.Event = Control.MouseDoubleClickEvent;
				val.Handler = (Delegate)new MouseButtonEventHandler(listViewItem_MouseDoubleClick);
				((Collection<SetterBase>)(object)((Style)target).Setters).Add((SetterBase)(object)val);
			}
		}
	}
}
