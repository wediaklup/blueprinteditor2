using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TypeEdit.DataHandling;

namespace S9BEditor
{
	internal class DirectoryTreeViewItem : TreeViewImageItem
	{
		private string mActualPath;

		public static readonly DependencyProperty ActualDisplayNameProperty;

		public static readonly DependencyProperty DisplayNameProperty;

		private string mCachedRelativePath;

		public string ActualPath
		{
			get
			{
				return mActualPath;
			}
			set
			{
				if (mActualPath != value)
				{
					mCachedRelativePath = null;
					mActualPath = value;
					OnActualPathChanged(EventArgs.Empty);
				}
			}
		}

		public string ActualDisplayName
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(ActualDisplayNameProperty);
			}
			private set
			{
				((DependencyObject)this).SetValue(ActualDisplayNameProperty, (object)value);
			}
		}

		public string DisplayName
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(DisplayNameProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(DisplayNameProperty, (object)value);
			}
		}

		public bool IsDirectory { get; private set; }

		public bool IsValid { get; private set; }

		public string RelativePath
		{
			get
			{
				if (mCachedRelativePath == null)
				{
					if (((FrameworkElement)this).Parent is DirectoryTreeViewItem)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(Path.GetFileName(mActualPath));
						DirectoryTreeViewItem directoryTreeViewItem = ((FrameworkElement)this).Parent as DirectoryTreeViewItem;
						while (directoryTreeViewItem != null && ((FrameworkElement)directoryTreeViewItem).Parent != null && ((FrameworkElement)directoryTreeViewItem).Parent is DirectoryTreeViewItem)
						{
							stringBuilder.Insert(0, Path.GetFileName(directoryTreeViewItem.ActualPath) + "\\");
							directoryTreeViewItem = ((FrameworkElement)directoryTreeViewItem).Parent as DirectoryTreeViewItem;
						}
						mCachedRelativePath = stringBuilder.ToString();
					}
					else
					{
						mCachedRelativePath = "";
					}
				}
				return mCachedRelativePath;
			}
		}

		public bool CanCopy => RelativePath != string.Empty;

		public bool CanOpen => true;

		public bool CanPaste => true;

		public bool CanCut => RelativePath != string.Empty;

		public bool CanDelete => RelativePath != string.Empty;

		public bool CanAdd => IsDirectory;

		public event EventHandler ActualPathChanged;

		static DirectoryTreeViewItem()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			ActualDisplayNameProperty = DependencyProperty.Register("ActualDisplayName", typeof(string), typeof(DirectoryTreeViewItem), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
			DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(DirectoryTreeViewItem), (PropertyMetadata)new UIPropertyMetadata((object)string.Empty));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DirectoryTreeViewItem), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(DirectoryTreeViewItem)));
		}

		protected override void OnInitialized(EventArgs e)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			((FrameworkElement)this).OnInitialized(e);
			((Collection<SortDescription>)(object)((CollectionView)((ItemsControl)this).Items).SortDescriptions).Clear();
			((Collection<SortDescription>)(object)((CollectionView)((ItemsControl)this).Items).SortDescriptions).Add(new SortDescription("IsDirectory", ListSortDirection.Descending));
			((Collection<SortDescription>)(object)((CollectionView)((ItemsControl)this).Items).SortDescriptions).Add(new SortDescription("ActualDisplayName", ListSortDirection.Ascending));
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(HeaderedItemsControl.HeaderProperty, typeof(DirectoryTreeViewItem))).AddValueChanged((object)this, (EventHandler)headerChanged);
		}

		protected virtual void OnActualPathChanged(EventArgs e)
		{
			((ItemsControl)this).Items.Clear();
			string empty = string.Empty;
			string fileName = Path.GetFileName(ActualPath);
			empty = ((fileName.Length <= 0) ? ActualPath : fileName);
			DirectoryInfo directoryInfo = new DirectoryInfo(ActualPath);
			if (directoryInfo.Exists)
			{
				IsDirectory = true;
				IsValid = true;
				((ItemsControl)this).Items.Add((object)new DirectoryTreeViewItem());
			}
			else
			{
				FileInfo fileInfo = new FileInfo(ActualPath);
				if (fileInfo.Exists)
				{
					IsDirectory = false;
					IsValid = true;
				}
			}
			if (!IsValid)
			{
				empty += " - (Invalid)";
			}
			if (DisplayName != null)
			{
				updateDisplayNameFromFileName();
			}
			if (ActualPathChanged != null)
			{
				ActualPathChanged(this, e);
			}
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			if (((DependencyPropertyChangedEventArgs)(e)).NewValue != ((DependencyPropertyChangedEventArgs)(e)).OldValue)
			{
				if (((DependencyPropertyChangedEventArgs)(e)).Property == TreeViewImageItem.IsEditingProperty)
				{
					if (IsValid)
					{
						base.EditText = Path.GetFileName(ActualPath);
					}
				}
				else if (((DependencyPropertyChangedEventArgs)(e)).Property == TreeViewImageItem.EditTextProperty)
				{
					string text = (string)((DependencyPropertyChangedEventArgs)(e)).NewValue;
					string actualPath = ActualPath;
					if (Path.GetFileName(actualPath) != text)
					{
						string directoryName = Path.GetDirectoryName(actualPath);
						string text2 = Path.Combine(directoryName, text);
						bool flag = false;
						try
						{
							if (text.Length > 0 && text.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
							{
								FileInfo fileInfo = new FileInfo(text2);
								if (fileInfo.Exists)
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
						}
						catch (Exception)
						{
							flag = true;
						}
						if (!flag)
						{
							try
							{
								if (IsDirectory)
								{
									Directory.Move(actualPath, text2);
								}
								else
								{
									File.Move(actualPath, text2);
								}
								if (((FrameworkElement)this).Parent is DirectoryTreeViewItem directoryTreeViewItem)
								{
									directoryTreeViewItem.Refresh();
									foreach (object item in (IEnumerable)((ItemsControl)directoryTreeViewItem).Items)
									{
										if (item is DirectoryTreeViewItem directoryTreeViewItem2 && DataUtil.PathsEqual(text2, directoryTreeViewItem2.ActualPath))
										{
											((TreeViewItem)directoryTreeViewItem2).IsSelected = true;
											break;
										}
									}
								}
							}
							catch (Exception)
							{
							}
						}
					}
				}
				else if (((DependencyPropertyChangedEventArgs)(e)).Property == DisplayNameProperty)
				{
					string text3 = (string)((DependencyPropertyChangedEventArgs)(e)).NewValue;
					if (text3 == null)
					{
						updateDisplayNameFromFileName();
					}
					else
					{
						ActualDisplayName = DisplayName;
					}
				}
			}
			base.OnPropertyChanged(e);
		}

		private void updateDisplayNameFromFileName()
		{
			string fileName = Path.GetFileName(ActualPath);
			string text = fileName;
			if (!IsValid)
			{
				text += " - (Invalid)";
			}
			ActualDisplayName = text;
		}

		private void headerChanged(object sender, EventArgs e)
		{
		}

		protected override void OnExpanded(RoutedEventArgs e)
		{
			Refresh();
			((TreeViewItem)this).OnExpanded(e);
		}

		protected override void OnCollapsed(RoutedEventArgs e)
		{
			Refresh();
			((TreeViewItem)this).OnCollapsed(e);
		}

		public void Refresh()
		{
			if (!IsDirectory)
			{
				return;
			}
			if (((TreeViewItem)this).IsExpanded)
			{
				List<object> list = new List<object>();
				foreach (object item in (IEnumerable)((ItemsControl)this).Items)
				{
					if (!(item is DirectoryTreeViewItem directoryTreeViewItem))
					{
						continue;
					}
					if (directoryTreeViewItem.IsDirectory)
					{
						if (!Directory.Exists(directoryTreeViewItem.ActualPath))
						{
							list.Add(item);
						}
					}
					else if (!File.Exists(directoryTreeViewItem.ActualPath))
					{
						list.Add(item);
					}
				}
				foreach (object item2 in list)
				{
					((ItemsControl)this).Items.Remove(item2);
				}
				try
				{
					foreach (string item3 in Directory.EnumerateDirectories(ActualPath, "*", SearchOption.TopDirectoryOnly))
					{
						bool flag = false;
						foreach (object item4 in (IEnumerable)((ItemsControl)this).Items)
						{
							if (item4 is DirectoryTreeViewItem directoryTreeViewItem2 && directoryTreeViewItem2.IsDirectory && DataUtil.PathsEqual(directoryTreeViewItem2.ActualPath, item3))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							DirectoryTreeViewItem directoryTreeViewItem3 = new DirectoryTreeViewItem();
							directoryTreeViewItem3.ActualPath = item3;
							((ItemsControl)this).Items.Add((object)directoryTreeViewItem3);
						}
					}
					foreach (string item5 in Directory.EnumerateFiles(ActualPath, "*", SearchOption.TopDirectoryOnly))
					{
						bool flag2 = false;
						foreach (object item6 in (IEnumerable)((ItemsControl)this).Items)
						{
							if (item6 is DirectoryTreeViewItem directoryTreeViewItem4 && !directoryTreeViewItem4.IsDirectory && DataUtil.PathsEqual(directoryTreeViewItem4.ActualPath, item5))
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							DirectoryTreeViewItem directoryTreeViewItem5 = new DirectoryTreeViewItem();
							directoryTreeViewItem5.ActualPath = item5;
							((ItemsControl)this).Items.Add((object)directoryTreeViewItem5);
						}
					}
				}
				catch (Exception ex)
				{
					((HeaderedItemsControl)this).Header = string.Concat(((HeaderedItemsControl)this).Header, string.Format(CultureInfo.CurrentCulture, "({0})", new object[1] { ex.Message }));
				}
				foreach (object item7 in (IEnumerable)((ItemsControl)this).Items)
				{
					if (item7 is DirectoryTreeViewItem directoryTreeViewItem6)
					{
						directoryTreeViewItem6.Refresh();
					}
				}
			}
			if (((CollectionView)((ItemsControl)this).Items).NeedsRefresh)
			{
				((CollectionView)((ItemsControl)this).Items).Refresh();
			}
		}

		public DirectoryTreeView GetParentTreeView()
		{
			Control val = (Control)(object)this;
			while (val != null && !(val is DirectoryTreeView))
			{
				DependencyObject parent = ((FrameworkElement)val).Parent;
				val = (Control)(object)((parent is Control) ? parent : null);
			}
			return val as DirectoryTreeView;
		}

		public DirectoryTreeViewItem GetTopMostTreeViewItem()
		{
			Control val = (Control)(object)this;
			while (val != null && !(((FrameworkElement)val).Parent is DirectoryTreeView))
			{
				DependencyObject parent = ((FrameworkElement)val).Parent;
				val = (Control)(object)((parent is Control) ? parent : null);
			}
			return val as DirectoryTreeViewItem;
		}

		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{
			GetParentTreeView()?.UpdateIcon(this);
			((TreeViewItem)this).OnVisualParentChanged(oldParent);
		}
	}
}
