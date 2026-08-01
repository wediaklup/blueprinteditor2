using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace S9BEditor
{
	public partial class UnhandledExceptionDialog : CustomWindow, IComponentConnector
	{
		public static readonly DependencyProperty ExceptionTextProperty;

		public string ExceptionText
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(ExceptionTextProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(ExceptionTextProperty, (object)value);
			}
		}

		public UnhandledExceptionDialog(Exception e)
		{
			InitializeComponent();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(e.Message);
			stringBuilder.AppendLine(e.StackTrace);
			ExceptionText = stringBuilder.ToString();
		}

		private void button3_Click(object sender, RoutedEventArgs e)
		{
			((Window)this).Close();
		}

		private void button4_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown(-1);
		}

		static UnhandledExceptionDialog()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			ExceptionTextProperty = DependencyProperty.Register("ExceptionText", typeof(string), typeof(UnhandledExceptionDialog), (PropertyMetadata)new UIPropertyMetadata((object)string.Empty));
		}
	}
}
