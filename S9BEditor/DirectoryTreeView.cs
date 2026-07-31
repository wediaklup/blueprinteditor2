using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TypeEdit.DataHandling;

namespace S9BEditor
{
	internal class DirectoryTreeView : TreeView
	{
		public static readonly DependencyProperty FileIconUriProviderProperty;

		private Dictionary<DirectoryTreeViewItem, FileSystemWatcher> mWatchedItems;

		public TypeEditFileIconUriProvider FileIconUriProvider
		{
			get
			{
				return (TypeEditFileIconUriProvider)((DependencyObject)this).GetValue(FileIconUriProviderProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(FileIconUriProviderProperty, (object)value);
			}
		}

		private event EventHandler FileIconUriProviderChanged;

		public event EventHandler<DirectoryTreeViewFileSelectedArgs> FileSelected;

		static DirectoryTreeView()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			FileIconUriProviderProperty = DependencyProperty.Register("FileIconUriProvider", typeof(TypeEditFileIconUriProvider), typeof(DirectoryTreeView), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DirectoryTreeView), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(DirectoryTreeView)));
		}

		public DirectoryTreeView()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			mWatchedItems = new Dictionary<DirectoryTreeViewItem, FileSystemWatcher>();
			((Collection<SortDescription>)(object)((CollectionView)((ItemsControl)this).Items).SortDescriptions).Add(new SortDescription("IsDirectory", ListSortDirection.Ascending));
			((Collection<SortDescription>)(object)((CollectionView)((ItemsControl)this).Items).SortDescriptions).Add(new SortDescription("ActualDisplayName", ListSortDirection.Ascending));
		}

		protected virtual void OnFileIconUriProviderChanged(EventArgs e)
		{
			if (FileIconUriProviderChanged != null)
			{
				FileIconUriProviderChanged(this, e);
			}
			updateAllIcons();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (((DependencyPropertyChangedEventArgs)(e)).Property == FileIconUriProviderProperty)
			{
				TypeEditFileIconUriProvider typeEditFileIconUriProvider = (TypeEditFileIconUriProvider)((DependencyPropertyChangedEventArgs)(e)).OldValue;
				if (typeEditFileIconUriProvider != null)
				{
					typeEditFileIconUriProvider.IconsChanged -= newVal_IconsChanged;
				}
				TypeEditFileIconUriProvider typeEditFileIconUriProvider2 = (TypeEditFileIconUriProvider)((DependencyPropertyChangedEventArgs)(e)).NewValue;
				if (typeEditFileIconUriProvider2 != null)
				{
					typeEditFileIconUriProvider2.IconsChanged += newVal_IconsChanged;
				}
				OnFileIconUriProviderChanged(EventArgs.Empty);
			}
			base.OnPropertyChanged(e);
		}

		private void newVal_IconsChanged(object sender, EventArgs e)
		{
			updateAllIcons();
		}

		private void updateAllIcons()
		{
			foreach (object item in (IEnumerable)((ItemsControl)this).Items)
			{
				UpdateIconsRecursive(item as DirectoryTreeViewItem);
			}
		}

		private void UpdateIconsRecursive(DirectoryTreeViewItem dtvi)
		{
			if (dtvi == null)
			{
				return;
			}
			foreach (object item in (IEnumerable)((ItemsControl)dtvi).Items)
			{
				UpdateIconsRecursive(item as DirectoryTreeViewItem);
			}
			UpdateIcon(dtvi);
		}

		public void UpdateIcon(DirectoryTreeViewItem dtvi)
		{
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			if (dtvi.IsValid)
			{
				if (FileIconUriProvider != null)
				{
					DirectoryTreeViewItem topMostTreeViewItem = dtvi.GetTopMostTreeViewItem();
					if (dtvi.IsDirectory)
					{
						dtvi.IconSourceUri = FileIconUriProvider.GetIconUriForDirectory(topMostTreeViewItem.ActualPath, dtvi.RelativePath);
					}
					else
					{
						dtvi.IconSourceUri = FileIconUriProvider.GetIconUriForFile(topMostTreeViewItem.ActualPath, dtvi.RelativePath);
					}
				}
				else
				{
					dtvi.IconSourceUri = null;
				}
				if (!(dtvi.IconSourceUri == null))
				{
					return;
				}
				try
				{
					Icon fileIcon = S9BEUtil.GetFileIcon(dtvi.ActualPath, S9BEUtil.FileIconFlags.SmallSize);
					if (fileIcon != null)
					{
						Bitmap val = fileIcon.ToBitmap();
						IntPtr hbitmap = val.GetHbitmap();
						if (hbitmap != IntPtr.Zero)
						{
							dtvi.ImageSource = (ImageSource)(object)Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
							Native.DeleteObject(hbitmap);
						}
					}
					return;
				}
				catch (Exception ex)
				{
					AppServices.OutputManager.WriteOutput("Application", "Failed to create icon for " + dtvi.ActualPath + ": " + ex.Message);
					return;
				}
			}
			dtvi.IconSourceUri = null;
		}

		protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
		{
			if (((RoutedEventArgs)e).Source is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.IsValid && !directoryTreeViewItem.IsDirectory)
			{
				OpenItem(directoryTreeViewItem);
			}
			base.OnMouseDoubleClick(e);
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (object newItem in e.NewItems)
				{
					if (newItem is DirectoryTreeViewItem directoryTreeViewItem && directoryTreeViewItem.IsDirectory)
					{
						FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
						fileSystemWatcher.Path = directoryTreeViewItem.ActualPath;
						fileSystemWatcher.EnableRaisingEvents = true;
						fileSystemWatcher.IncludeSubdirectories = true;
						FileSystemWatcher fileSystemWatcher2 = fileSystemWatcher;
						fileSystemWatcher2.Changed += fsw_Changed;
						fileSystemWatcher2.Deleted += fsw_Deleted;
						fileSystemWatcher2.Renamed += fsw_Renamed;
						fileSystemWatcher2.Created += fsw_Created;
						mWatchedItems.Add(directoryTreeViewItem, fileSystemWatcher2);
					}
				}
			}
			if (e.OldItems != null)
			{
				foreach (object oldItem in e.OldItems)
				{
					if (oldItem is DirectoryTreeViewItem directoryTreeViewItem2 && directoryTreeViewItem2.IsDirectory)
					{
						if (mWatchedItems.TryGetValue(directoryTreeViewItem2, out var value))
						{
							value.EnableRaisingEvents = false;
							value.Changed -= fsw_Changed;
							value.Deleted -= fsw_Deleted;
							value.Renamed -= fsw_Renamed;
							value.Created -= fsw_Created;
							value.Dispose();
						}
						mWatchedItems.Remove(directoryTreeViewItem2);
					}
				}
			}
			base.OnItemsChanged(e);
		}

		private void fsw_Created(object sender, FileSystemEventArgs e)
		{
			((DispatcherObject)this).Dispatcher.Invoke((Delegate)(Action)delegate
			{
				updateNode(Path.GetDirectoryName(e.FullPath));
			}, (object[])null);
		}

		private void fsw_Renamed(object sender, RenamedEventArgs e)
		{
			((DispatcherObject)this).Dispatcher.Invoke((Delegate)(Action)delegate
			{
				updateNode(Path.GetDirectoryName(e.FullPath));
			}, (object[])null);
		}

		private void fsw_Deleted(object sender, FileSystemEventArgs e)
		{
			((DispatcherObject)this).Dispatcher.Invoke((Delegate)(Action)delegate
			{
				updateNode(Path.GetDirectoryName(e.FullPath));
			}, (object[])null);
		}

		private void fsw_Changed(object sender, FileSystemEventArgs e)
		{
		}

		private bool updateNodeRecursive(string path, DirectoryTreeViewItem dtvi)
		{
			if (DataUtil.PathsEqual(path, dtvi.ActualPath))
			{
				dtvi.Refresh();
				return true;
			}
			foreach (object item in (IEnumerable)((ItemsControl)dtvi).Items)
			{
				if (item is DirectoryTreeViewItem dtvi2 && updateNodeRecursive(path, dtvi2))
				{
					return true;
				}
			}
			return false;
		}

		private bool updateNode(string path)
		{
			foreach (object item in (IEnumerable)((ItemsControl)this).Items)
			{
				if (item is DirectoryTreeViewItem dtvi && updateNodeRecursive(path, dtvi))
				{
					return true;
				}
			}
			return false;
		}

		public void OpenItem(DirectoryTreeViewItem item)
		{
			if (item != null && item.IsValid && item.CanOpen)
			{
				if (item.IsDirectory)
				{
					((TreeViewItem)item).IsExpanded = true;
				}
				else if (FileSelected != null)
				{
					FileSelected(this, new DirectoryTreeViewFileSelectedArgs(item.ActualPath));
				}
			}
		}

		private bool selectItem(DirectoryTreeViewItem startItem, string path)
		{
			if (DataUtil.PathsEqual(startItem.ActualPath, path))
			{
				((TreeViewItem)startItem).IsSelected = true;
				return true;
			}
			foreach (object item in (IEnumerable)((ItemsControl)startItem).Items)
			{
				if (!(item is DirectoryTreeViewItem directoryTreeViewItem))
				{
					continue;
				}
				string text = path;
				string text2 = null;
				while (text.Length > 0 && text2 != text)
				{
					if (DataUtil.PathsEqual(text, directoryTreeViewItem.ActualPath))
					{
						((TreeViewItem)directoryTreeViewItem).IsExpanded = true;
						directoryTreeViewItem.Refresh();
						if (!selectItem(directoryTreeViewItem, path))
						{
							break;
						}
						return true;
					}
					text2 = text;
					text = Path.GetDirectoryName(text);
					if (text == null)
					{
						break;
					}
				}
			}
			return false;
		}

		internal bool SelectItem(string fullPath)
		{
			foreach (object item in (IEnumerable)((ItemsControl)this).Items)
			{
				if (item is DirectoryTreeViewItem startItem && selectItem(startItem, fullPath))
				{
					return true;
				}
			}
			return false;
		}
	}
}
