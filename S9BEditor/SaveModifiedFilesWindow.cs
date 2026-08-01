using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace S9BEditor
{
	public partial class SaveModifiedFilesWindow : CustomWindow, IComponentConnector
	{

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
	}
}
