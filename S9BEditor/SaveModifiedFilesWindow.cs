using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace S9BEditor;

public class SaveModifiedFilesWindow : CustomWindow, IComponentConnector
{
	internal ListBox listBox1;

	internal Label label1;

	internal Button button1;

	internal Button button2;

	internal Button button3;

	private bool _contentLoaded;

	public IEnumerable DocumentsFileNames
	{
		get
		{
			return ((ItemsControl)listBox1).ItemsSource;
		}
		set
		{
			((ItemsControl)listBox1).ItemsSource = value;
		}
	}

	public bool ShouldSaveFiles { get; private set; }

	public SaveModifiedFilesWindow()
	{
		InitializeComponent();
	}

	private void button3_Click(object sender, RoutedEventArgs e)
	{
		ShouldSaveFiles = true;
		((Window)this).DialogResult = true;
	}

	private void button2_Click(object sender, RoutedEventArgs e)
	{
		ShouldSaveFiles = false;
		((Window)this).DialogResult = true;
	}

	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[DebuggerNonUserCode]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/BlueprintEditor2;component/savemodifiedfileswindow.xaml", UriKind.Relative);
			Application.LoadComponent((object)this, uri);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			listBox1 = (ListBox)target;
			break;
		case 2:
			label1 = (Label)target;
			break;
		case 3:
			button1 = (Button)target;
			break;
		case 4:
			button2 = (Button)target;
			((ButtonBase)button2).Click += new RoutedEventHandler(button2_Click);
			break;
		case 5:
			button3 = (Button)target;
			((ButtonBase)button3).Click += new RoutedEventHandler(button3_Click);
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
