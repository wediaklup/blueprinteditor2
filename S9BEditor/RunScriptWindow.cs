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

namespace S9BEditor
{
	public partial class RunScriptWindow : CustomWindow, IComponentConnector
	{
		public static readonly DependencyProperty ScriptsProperty;

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

		static RunScriptWindow()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			ScriptsProperty = DependencyProperty.Register("Scripts", typeof(ObservableCollection<VMScript>), typeof(RunScriptWindow), (PropertyMetadata)new UIPropertyMetadata((PropertyChangedCallback)null));
		}
	}
}
