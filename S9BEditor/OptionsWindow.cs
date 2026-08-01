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
//using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using S9BEditor.Properties;

namespace S9BEditor
{
	public partial class OptionsWindow : CustomWindow, IComponentConnector
	{
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
			System.Windows.Forms.FolderBrowserDialog val = new System.Windows.Forms.FolderBrowserDialog();
			if (Directory.Exists(AppServices.ResourceManager.DeploymentPath))
			{
				val.SelectedPath = AppServices.ResourceManager.DeploymentPath;
			}
			else
			{
				val.SelectedPath = string.Empty;
			}
			if ((int)((System.Windows.Forms.CommonDialog)val).ShowDialog((System.Windows.Forms.IWin32Window)(object)this) == 1)
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
	}
}
