using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using RailWorks;

namespace S9BEditor
{
	public partial class AboutWindow : CustomWindow, IComponentConnector
	{
		public string Version => RailWorks.Properties.Version;

		public AboutWindow()
		{
			InitializeComponent();
		}
	}
}
