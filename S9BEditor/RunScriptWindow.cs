using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using S9BEditor.Scripts;
using S9BEditor.ViewModels;
using TypeEdit.Interfaces;

namespace S9BEditor;

public class RunScriptWindow : CustomWindow, IComponentConnector
{
	public static readonly DependencyProperty ScriptsProperty;

	internal ListBox scriptsListBox;

	private bool _contentLoaded;

	internal ObservableCollection<VMScript> Scripts
	{
		get
		{
			return (ObservableCollection<VMScript>)((DependencyObject)this).GetValue(ScriptsProperty);
		}
		private set
		{
			((DependencyObject)this).SetValue(ScriptsProperty, (object)value);
		}
	}

	public RunScriptWindow()
	{
		InitializeComponent();
	}

	protected override void OnSourceInitialized(EventArgs e)
	{
		RefreshScripts();
		base.OnSourceInitialized(e);
	}

	private void RefreshScripts()
	{
		Scripts = new ObservableCollection<VMScript>();
		if (!Directory.Exists(AppServices.ResourceManager.ScriptsPath))
		{
			return;
		}
		string[] files = Directory.GetFiles(AppServices.ResourceManager.ScriptsPath, "*.*", SearchOption.AllDirectories);
		string[] array = files;
		foreach (string text in array)
		{
			if (text.EndsWith(".bpes.cs", StringComparison.OrdinalIgnoreCase) || text.EndsWith(".bpes.vb", StringComparison.OrdinalIgnoreCase))
			{
				ScriptDefinition scriptDefinition = ScriptDefinition.FromFile(text);
				if (scriptDefinition != null)
				{
					VMScript item = new VMScript(scriptDefinition);
					Scripts.Add(item);
				}
			}
		}
	}

	private void Run_Click(object sender, RoutedEventArgs e)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		if (((Selector)scriptsListBox).SelectedItem is VMScript vMScript)
		{
			try
			{
				AppServices.OutputManager.ClearErrors("Script", OutputMessageTarget.Build);
				AppServices.OutputManager.ClearOutput("Script");
				AppServices.OutputManager.ShowOutput("Script");
				IScript script = vMScript.ScriptDefinition.CreateScript();
				ScriptContext context = new ScriptContext();
				script.Run(context);
				AppServices.OutputManager.WriteOutput("Script", "Script \"" + vMScript.ScriptDefinition.Name + "\" completed!");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Script threw an exception: " + ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}
	}

	private void Reload_Click(object sender, RoutedEventArgs e)
	{
		RefreshScripts();
	}

	private void Cancel_Click(object sender, RoutedEventArgs e)
	{
		((Window)this).Close();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/BlueprintEditor2;component/runscriptwindow.xaml", UriKind.Relative);
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
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			scriptsListBox = (ListBox)target;
			break;
		case 2:
			((ButtonBase)(Button)target).Click += new RoutedEventHandler(Reload_Click);
			break;
		case 3:
			((ButtonBase)(Button)target).Click += new RoutedEventHandler(Run_Click);
			break;
		case 4:
			((ButtonBase)(Button)target).Click += new RoutedEventHandler(Cancel_Click);
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}

	static RunScriptWindow()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		ScriptsProperty = DependencyProperty.Register("Scripts", typeof(ObservableCollection<VMScript>), typeof(RunScriptWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
	}
}
