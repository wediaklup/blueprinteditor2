using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace S9BEditor;

public class TypeSelectionWindow : CustomWindow, IComponentConnector
{
	public static readonly DependencyProperty ItemsSourceProperty;

	public static readonly DependencyProperty DisplayMemberPathProperty;

	public static readonly DependencyProperty SelectedItemProperty;

	internal Button button2;

	internal Button button3;

	internal ListBox listBox1;

	internal Label label1;

	private bool _contentLoaded;

	public object ItemsSource
	{
		get
		{
			return ((DependencyObject)this).GetValue(ItemsSourceProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ItemsSourceProperty, value);
		}
	}

	public string DisplayMemberPath
	{
		get
		{
			return (string)((DependencyObject)this).GetValue(DisplayMemberPathProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(DisplayMemberPathProperty, (object)value);
		}
	}

	public object SelectedItem
	{
		get
		{
			return ((DependencyObject)this).GetValue(SelectedItemProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(SelectedItemProperty, value);
		}
	}

	public TypeSelectionWindow()
	{
		InitializeComponent();
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
			Uri uri = new Uri("/BlueprintEditor2;component/typeselectionwindow.xaml", UriKind.Relative);
			Application.LoadComponent((object)this, uri);
		}
	}

	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			button2 = (Button)target;
			break;
		case 2:
			button3 = (Button)target;
			((ButtonBase)button3).Click += new RoutedEventHandler(button3_Click);
			break;
		case 3:
			listBox1 = (ListBox)target;
			break;
		case 4:
			label1 = (Label)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}

	static TypeSelectionWindow()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(TypeSelectionWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(TypeSelectionWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(TypeSelectionWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
	}
}
