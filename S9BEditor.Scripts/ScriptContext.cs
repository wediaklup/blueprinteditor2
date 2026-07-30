using TypeEdit.Base;
using TypeEdit.Interfaces;

namespace S9BEditor.Scripts
{
	internal class ScriptContext : IScriptContext
	{
		public IServiceLocator ServiceLocator => Singleton<IServiceLocator>.Instance;

		public string SourcePath => AppServices.ResourceManager.SourcePath;

		public string DevSourcePath => AppServices.ResourceManager.DevSourcePath;

		public string DeploymentPath => AppServices.ResourceManager.DeploymentPath;

		public string AssetsPath => AppServices.ResourceManager.AssetsPath;

		public string ContentPath => AppServices.ResourceManager.ContentPath;

		public void ReportError(string error, ErrorMessageType messageType = ErrorMessageType.Error)
		{
			AppServices.OutputManager.ReportError("Script", OutputMessageTarget.Build, messageType, error);
		}

		public void WriteOutput(string message)
		{
			AppServices.OutputManager.WriteOutput("Script", message);
		}
	}
}
