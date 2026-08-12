using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using S9BEditor.Properties;
using TypeEdit.DataHandling;

namespace S9BEditor
{
	internal class ResourceManager : INotifyPropertyChanged
	{
		private bool mSchemaExists;

		private string mInitialCurrentDirAbsolutePath;

		private string mCurrentDeploymentFolderAbsolutePath;

		public string ScriptsPath => Path.Combine(mCurrentDeploymentFolderAbsolutePath, "TypeEditScripts");

		public string AssetsPath => Path.Combine(mCurrentDeploymentFolderAbsolutePath, "Assets");

		public string SourcePath => Path.Combine(mCurrentDeploymentFolderAbsolutePath, "Source");

		public string DevSourcePath => Path.Combine(mCurrentDeploymentFolderAbsolutePath, "..\\Source");

		public string ContentPath => Path.Combine(mCurrentDeploymentFolderAbsolutePath, "Content");

		public string SchemaPath => Path.Combine(mCurrentDeploymentFolderAbsolutePath, "Temp\\BlueprintSchema.xml");

		public string DeploymentPath => mCurrentDeploymentFolderAbsolutePath;

		public event EventHandler DirectoriesChanged;

		public event PropertyChangedEventHandler PropertyChanged;

		public ResourceManager()
		{
			mInitialCurrentDirAbsolutePath = Path.GetFullPath(Environment.CurrentDirectory);
			mSchemaExists = false;
			UpdateCurrentDeploymentFolder(force: true);
			((ApplicationSettingsBase)Settings.Default).PropertyChanged += Default_PropertyChanged;
		}

		public void UpdateCurrentDeploymentFolder(bool force)
		{
			string path = mCurrentDeploymentFolderAbsolutePath ?? string.Empty;
			bool flag = mSchemaExists;
			string deploymentFolder = Settings.Default.DeploymentFolder;
			if (string.IsNullOrWhiteSpace(deploymentFolder))
			{
				deploymentFolder = mInitialCurrentDirAbsolutePath;
			}
			else
			{
				if (Directory.Exists(mInitialCurrentDirAbsolutePath))
				{
					Environment.CurrentDirectory = mInitialCurrentDirAbsolutePath;
				}
				deploymentFolder = Path.GetFullPath(deploymentFolder);
				if (!Directory.Exists(deploymentFolder))
				{
					deploymentFolder = mInitialCurrentDirAbsolutePath;
				}
			}
			mCurrentDeploymentFolderAbsolutePath = deploymentFolder;
			mSchemaExists = File.Exists(SchemaPath);
			try
			{
				if (force || flag != mSchemaExists || !DataUtil.PathsEqual(Path.GetFullPath(mCurrentDeploymentFolderAbsolutePath), Path.GetFullPath(path)))
				{
					OnDirectoriesChanged();
				}
			}
			catch (Exception)
			{
				OnDirectoriesChanged();
			}
		}

		private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "DeploymentFolder")
			{
				UpdateCurrentDeploymentFolder(force: false);
			}
		}

		protected virtual void OnDirectoriesChanged()
		{
			RaisePropertyChanged("AssetsPath");
			RaisePropertyChanged("SourcePath");
			RaisePropertyChanged("DevSourcePath");
			RaisePropertyChanged("ContentPath");
			RaisePropertyChanged("SchemaPath");
			RaisePropertyChanged("DeploymentFolder");
			RaisePropertyChanged("TypeEditScripts");
			DirectoriesChanged?.Invoke(this, EventArgs.Empty);
		}

		protected void RaisePropertyChanged(string name)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(name));
		}

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			PropertyChanged?.Invoke(this, args);
		}
	}
}
