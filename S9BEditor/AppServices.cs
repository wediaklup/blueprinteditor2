using System.Windows.Forms;
using TypeEdit.DataHandling;
using TypeEdit.DataHandling.Exporting;
using TypeEdit.Diagnostics;
using TypeEdit.Interfaces;
using TypeEdit.Interfaces.Data;
using TypeEdit.Interfaces.Data.Exporting;
using TypeEdit.Interfaces.Diagnostics;
using TypeEdit.Interfaces.UI;

namespace S9BEditor;

internal class AppServices : IServiceLocator
{
	public static NativeWindow MainWindow { get; internal set; }

	public static SourceFileExporter Exporter { get; private set; }

	public static ShapeViewerProxy ShapeViewer { get; private set; }

	public static ResourceManager ResourceManager { get; private set; }

	public static FileManager FileManager { get; private set; }

	public static ErrorManager ErrorManager { get; private set; }

	public static ITypeEditSchema TypeEditSchema { get; private set; }

	public static UIManager UIManager { get; private set; }

	public static TimingManager TimingManager { get; private set; }

	public static OutputManager OutputManager { get; private set; }

	IWin32Window IServiceLocator.MainWindow => (IWin32Window)(object)MainWindow;

	IFileManager IServiceLocator.FileManager => FileManager;

	IErrorManager IServiceLocator.ErrorManager => ErrorManager;

	ITypeEditSchema IServiceLocator.TypeEditSchema => TypeEditSchema;

	IUIManager IServiceLocator.UIManager => UIManager;

	ITimingManager IServiceLocator.TimingManager => TimingManager;

	IOutputManager IServiceLocator.OutputManager => OutputManager;

	ISourceFileExporter IServiceLocator.Exporter => Exporter;

	static AppServices()
	{
		Exporter = new SourceFileExporter();
		ShapeViewer = new ShapeViewerProxy();
		ResourceManager = new ResourceManager();
		FileManager = new FileManager();
		TypeEditSchema = TypeEditSchemaUtil.CreateSchema();
		ErrorManager = new ErrorManager();
		OutputManager = new OutputManager();
		TimingManager = new TimingManager();
		UIManager = new UIManager();
	}
}
