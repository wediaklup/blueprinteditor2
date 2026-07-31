using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Markup;
using System.Xml;
using S9BEditor.Properties;
using ScintillaNET;
using ScintillaNET.Configuration;

namespace S9BEditor
{
	[ContentProperty("Text")]
	//NOFIX [Localizability(/*Could not decode attribute arguments.*/)]
	internal class ScintillaHost : ContentControl, IDisposable
	{
		private string mErr;

		private Scintilla mSCI;

		private WindowsFormsHost mWFH;

		public static readonly DependencyProperty TextProperty;

		private int mLastLineCount;

		//NOFIX [Localizability(/*Could not decode attribute arguments.*/)]
		public string Text
		{
			get
			{
				return (string)((DependencyObject)this).GetValue(TextProperty);
			}
			set
			{
				((DependencyObject)this).SetValue(TextProperty, (object)value);
			}
		}

		public ScintillaHost()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			((ContentControl)this)._002Ector();
			try
			{
				if (mWFH == null)
				{
					mWFH = new WindowsFormsHost();
				}
				if (mSCI == null)
				{
					mSCI = new Scintilla();
				}
				mWFH.Child = (Control)(object)mSCI;
				((FrameworkElement)mWFH).Loaded += new RoutedEventHandler(mWFH_Loaded);
				applyLua();
				mSCI.BorderStyle = (BorderStyle)0;
			}
			catch (Exception ex)
			{
				mErr = ex.Message;
				mSCI = null;
				mWFH = null;
			}
		}

		private void mWFH_Loaded(object sender, RoutedEventArgs e)
		{
			((Control)mSCI).TextChanged += mSCI_TextChanged;
		}

		private void mSCI_TextChanged(object sender, EventArgs e)
		{
			int count = mSCI.Lines.Count;
			if (mLastLineCount != count)
			{
				int num = 10;
				string text = "_9";
				while (count >= num)
				{
					text += "9";
					num *= 10;
				}
				mSCI.Margins[0].Width = mSCI.NativeInterface.TextWidth(33, text);
				mLastLineCount = count;
			}
			Text = ((Control)mSCI).Text;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (((DependencyPropertyChangedEventArgs)(e)).Property == TextProperty && ((DependencyPropertyChangedEventArgs)(e)).NewValue != ((DependencyPropertyChangedEventArgs)(e)).OldValue && mSCI != null)
			{
				((Control)mSCI).Text = (string)((DependencyPropertyChangedEventArgs)(e)).NewValue;
			}
			base.OnPropertyChanged(e);
		}

		protected override void OnInitialized(EventArgs e)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			base.OnInitialized(e);
			if ((mSCI != null) & (mWFH != null))
			{
				((ContentControl)this).Content = mWFH;
				return;
			}
			TextBlock val = new TextBlock();
			val.Text = Text;
			val.TextWrapping = (TextWrapping)2;
			((ContentControl)this).Content = val;
		}

		private void applyLua()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(Resources.LuaXML);
			mSCI.ConfigurationManager.Language = "lua";
			if (xmlDocument != null)
			{
				Configuration val = new Configuration(xmlDocument, "lua");
				mSCI.ConfigurationManager.Configure(val);
			}
		}

		public void Dispose()
		{
			if (mSCI != null)
			{
				if (mSCI.FindReplace.Window != null)
				{
					((Form)mSCI.FindReplace.Window).Close();
				}
				((Component)(object)mSCI).Dispose();
			}
		}

		static ScintillaHost()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ScintillaHost), (PropertyMetadata)new UIPropertyMetadata((object)string.Empty));
		}
	}
}
