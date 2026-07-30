using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using S9BEditor.Properties;

namespace S9BEditor
{
	public class OptionsWindow : CustomWindow, IComponentConnector
	{
		internal Button button2;

		internal Button button3;

		internal TabControl tabControl1;

		internal TabItem tabItem1;

		internal TextBox tbDeploymentPath;

		internal TextBox tbThreadCount;

		internal CheckBox chkSysSettingsFormat;

		internal TabItem tabItem2;

		internal CheckBox chkAdvancedEditor;

		private bool _contentLoaded;

		public OptionsWindow()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			FolderBrowserDialog val = new FolderBrowserDialog();
			if (Directory.Exists(AppServices.ResourceManager.DeploymentPath))
			{
				val.SelectedPath = AppServices.ResourceManager.DeploymentPath;
			}
			else
			{
				val.SelectedPath = string.Empty;
			}
			if ((int)((CommonDialog)val).ShowDialog((IWin32Window)(object)this) == 1)
			{
				((DependencyObject)tbDeploymentPath).SetCurrentValue(TextBox.TextProperty, (object)val.SelectedPath);
			}
		}

		private void button3_Click(object sender, RoutedEventArgs e)
		{
			bool flag = false;
			BindingExpression bindingExpression = ((FrameworkElement)tbDeploymentPath).GetBindingExpression(TextBox.TextProperty);
			((BindingExpressionBase)bindingExpression).UpdateSource();
			bindingExpression = ((FrameworkElement)chkAdvancedEditor).GetBindingExpression(ToggleButton.IsCheckedProperty);
			((BindingExpressionBase)bindingExpression).UpdateSource();
			bindingExpression = ((FrameworkElement)chkSysSettingsFormat).GetBindingExpression(ToggleButton.IsCheckedProperty);
			((BindingExpressionBase)bindingExpression).UpdateSource();
			if (int.TryParse(tbThreadCount.Text, out var result))
			{
				result = Math.Min(Math.Max(1, result), 12);
				Settings.Default.ThreadCount = result;
				Settings.Default.ThreadCountSet = true;
			}
			else
			{
				((Control)tbThreadCount).Background = (Brush)(object)Brushes.Pink;
				flag = true;
			}
			if (!flag)
			{
				((SettingsBase)Settings.Default).Save();
				((Window)this).DialogResult = true;
			}
			else
			{
				SystemSounds.Beep.Play();
			}
		}

		private void tbThreadCount_TextChanged(object sender, TextChangedEventArgs e)
		{
			((Control)tbThreadCount).Background = null;
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri uri = new Uri("/BlueprintEditor2;component/optionswindow.xaml", UriKind.Relative);
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
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected O, but got Unknown
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Expected O, but got Unknown
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
					tabControl1 = (TabControl)target;
					break;
				case 4:
					tabItem1 = (TabItem)target;
					break;
				case 5:
					tbDeploymentPath = (TextBox)target;
					break;
				case 6:
					((ButtonBase)(Button)target).Click += new RoutedEventHandler(button1_Click);
					break;
				case 7:
					tbThreadCount = (TextBox)target;
					((TextBoxBase)tbThreadCount).TextChanged += new TextChangedEventHandler(tbThreadCount_TextChanged);
					break;
				case 8:
					chkSysSettingsFormat = (CheckBox)target;
					break;
				case 9:
					tabItem2 = (TabItem)target;
					break;
				case 10:
					chkAdvancedEditor = (CheckBox)target;
					break;
				default:
					_contentLoaded = true;
					break;
			}
		}
	}
}
