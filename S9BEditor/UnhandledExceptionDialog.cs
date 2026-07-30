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
	public class UnhandledExceptionDialog : CustomWindow, IComponentConnector
	{
		public static readonly DependencyProperty ExceptionTextProperty;

		internal Button button4;

		internal Button button3;

		private bool _contentLoaded;

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

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri uri = new Uri("/BlueprintEditor2;component/unhandledexceptiondialog.xaml", UriKind.Relative);
				Application.LoadComponent((object)this, uri);
			}
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			switch (connectionId)
			{
				case 1:
					button4 = (Button)target;
					((ButtonBase)button4).Click += new RoutedEventHandler(button3_Click);
					break;
				case 2:
					button3 = (Button)target;
					((ButtonBase)button3).Click += new RoutedEventHandler(button4_Click);
					break;
				default:
					_contentLoaded = true;
					break;
			}
		}

		static UnhandledExceptionDialog()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			ExceptionTextProperty = DependencyProperty.Register("ExceptionText", typeof(string), typeof(UnhandledExceptionDialog), (PropertyMetadata)new UIPropertyMetadata((object)string.Empty));
		}
	}
}
