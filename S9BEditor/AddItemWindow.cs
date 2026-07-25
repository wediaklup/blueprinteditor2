using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;

namespace S9BEditor;

public class AddItemWindow : CustomWindow, IComponentConnector
{
	public static readonly DependencyProperty ItemsSourceProperty;

	public static readonly DependencyProperty SelectedCategoryProperty;

	public static readonly DependencyProperty SelectedFileTypeProperty;

	public static readonly DependencyProperty FileNameProperty;

	internal Button button2;

	internal Button button3;

	private bool _contentLoaded;

	public IEnumerable ItemsSource
	{
		get
		{
			return (IEnumerable)((DependencyObject)this).GetValue(ItemsSourceProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ItemsSourceProperty, (object)value);
		}
	}

	public ObservableCollection<FileTypeCategory> Items { get; private set; }

	public FileTypeCategory SelectedCategory
	{
		get
		{
			return (FileTypeCategory)((DependencyObject)this).GetValue(SelectedCategoryProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(SelectedCategoryProperty, (object)value);
		}
	}

	public FileType SelectedFileType
	{
		get
		{
			return (FileType)((DependencyObject)this).GetValue(SelectedFileTypeProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(SelectedFileTypeProperty, (object)value);
		}
	}

	public string FileName
	{
		get
		{
			return (string)((DependencyObject)this).GetValue(FileNameProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(FileNameProperty, (object)value);
		}
	}

	public AddItemWindow()
	{
		Items = new ObservableCollection<FileTypeCategory>();
		InitializeComponent();
	}

	protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		if (((DependencyPropertyChangedEventArgs)(e)).Property == ItemsSourceProperty)
		{
			SelectedCategory = null;
			Items.Clear();
			if (ItemsSource != null)
			{
				foreach (object item2 in ItemsSource)
				{
					if (item2 is FileTypeCategory item)
					{
						Items.Add(item);
					}
				}
				object obj = ((FrameworkElement)this).Resources[(object)"sortedCategories"];
				CollectionViewSource val = (CollectionViewSource)((obj is CollectionViewSource) ? obj : null);
				if (val != null && val.View != null)
				{
					{
						IEnumerator enumerator2 = ((IEnumerable)val.View).GetEnumerator();
						try
						{
							if (enumerator2.MoveNext())
							{
								object current2 = enumerator2.Current;
								SelectedCategory = current2 as FileTypeCategory;
							}
						}
						finally
						{
							IDisposable disposable = enumerator2 as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
				}
			}
		}
		else if (((DependencyPropertyChangedEventArgs)(e)).Property == SelectedCategoryProperty)
		{
			if (SelectedCategory != null && SelectedCategory.Items.Count > 0)
			{
				object obj2 = ((FrameworkElement)this).Resources[(object)"sortedFileTypes"];
				CollectionViewSource val2 = (CollectionViewSource)((obj2 is CollectionViewSource) ? obj2 : null);
				if (val2 != null && val2.View != null)
				{
					{
						IEnumerator enumerator3 = ((IEnumerable)val2.View).GetEnumerator();
						try
						{
							if (enumerator3.MoveNext())
							{
								object current3 = enumerator3.Current;
								SelectedFileType = current3 as FileType;
							}
						}
						finally
						{
							IDisposable disposable2 = enumerator3 as IDisposable;
							if (disposable2 != null)
							{
								disposable2.Dispose();
							}
						}
					}
				}
			}
			else
			{
				SelectedFileType = null;
			}
		}
		else if (((DependencyPropertyChangedEventArgs)(e)).Property == SelectedFileTypeProperty && ((DependencyPropertyChangedEventArgs)(e)).NewValue != null && (FileName == null || FileName == ((FileType)((DependencyPropertyChangedEventArgs)(e)).OldValue).DefaultFileName))
		{
			FileName = ((FileType)((DependencyPropertyChangedEventArgs)(e)).NewValue).DefaultFileName;
		}
		base.OnPropertyChanged(e);
	}

	private void button3_Click(object sender, RoutedEventArgs e)
	{
		((Window)this).DialogResult = true;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/BlueprintEditor2;component/additemwindow.xaml", UriKind.Relative);
			Application.LoadComponent((object)this, uri);
		}
	}

	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[DebuggerNonUserCode]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			button2 = (Button)target;
			break;
		case 2:
			button3 = (Button)target;
			((ButtonBase)button3).Click += new RoutedEventHandler(button3_Click);
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}

	static AddItemWindow()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AddItemWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		SelectedCategoryProperty = DependencyProperty.Register("SelectedCategory", typeof(FileTypeCategory), typeof(AddItemWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		SelectedFileTypeProperty = DependencyProperty.Register("SelectedFileType", typeof(FileType), typeof(AddItemWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(AddItemWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
	}
}
