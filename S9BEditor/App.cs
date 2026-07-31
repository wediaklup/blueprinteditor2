using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Threading;
using S9BEditor.Properties;
using TypeEdit.Base;
using TypeEdit.Interfaces;

namespace S9BEditor
{
	public class App : Application
	{
		private bool mUseVCRailWorks;

		private bool mUseICRailWorks;

		private bool _contentLoaded;

		public App()
		{
			InitializeComponent();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			processCommandLineArgs(e.Args);
			if (!Debugger.IsAttached)
			{
				((Application)this).DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
			}
			Application.EnableVisualStyles();
			initSingletons();
			if (!Settings.Default.UseSystemDataFormat)
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			}
			if (!Settings.Default.ThreadCountSet)
			{
				Settings.Default.ThreadCount = Environment.ProcessorCount;
				Settings.Default.ThreadCountSet = true;
				((SettingsBase)Settings.Default).Save();
			}
			base.OnStartup(e);
		}

		private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			try
			{
				UnhandledExceptionDialog unhandledExceptionDialog = new UnhandledExceptionDialog(e.Exception);
				((Window)unhandledExceptionDialog).ShowDialog();
				e.Handled = true;
			}
			catch (Exception)
			{
				e.Handled = false;
			}
		}

		private void processCommandLineArgs(string[] args)
		{
			foreach (string text in args)
			{
				if (text.Equals("-VC", StringComparison.InvariantCultureIgnoreCase))
				{
					mUseVCRailWorks = true;
				}
				else if (text.Equals("-IC", StringComparison.InvariantCultureIgnoreCase))
				{
					mUseICRailWorks = true;
				}
			}
		}

		protected override void OnExit(ExitEventArgs e)
		{
			((SettingsBase)Settings.Default).Save();
			base.OnExit(e);
		}

		private void initSingletons()
		{
			AppServices instance = new AppServices();
			Singleton<IServiceLocator>.SetInstance(instance);
			AppServices.ResourceManager.DirectoriesChanged += ResourceManager_DirectoriesChanged;
			loadSchema();
			AppServices.ShapeViewer.HasVCArgument = mUseVCRailWorks;
			AppServices.ShapeViewer.HasICArgument = mUseICRailWorks;
		}

		private void ResourceManager_DirectoriesChanged(object sender, EventArgs e)
		{
			loadSchema();
		}

		private void loadSchema()
		{
			AppServices.OutputManager.ClearErrors("TypeEdit", OutputMessageTarget.Application);
			string schemaPath = AppServices.ResourceManager.SchemaPath;
			AppServices.TypeEditSchema.SetSchemaFileName(schemaPath);
			if (!File.Exists(schemaPath))
			{
				AppServices.OutputManager.ReportError("TypeEdit", OutputMessageTarget.Application, ErrorMessageType.Error, schemaPath + " is missing. Run RailWorks at least once to create this file");
			}
		}

		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[DebuggerNonUserCode]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				((Application)this).StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
				Uri uri = new Uri("/BlueprintEditor2;component/app.xaml", UriKind.Relative);
				Application.LoadComponent((object)this, uri);
			}
		}

		[STAThread]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public static void Main()
		{
			App app = new App();
			app.InitializeComponent();
			((Application)app).Run();
		}
	}
}
