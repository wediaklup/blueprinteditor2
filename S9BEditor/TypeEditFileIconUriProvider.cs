using System;
using System.IO;
using TypeEdit.Interfaces.Data.Descriptors;

namespace S9BEditor;

internal class TypeEditFileIconUriProvider
{
	private const string IMAGES_FOLDER = "pack://application:,,,/Resources/";

	public event EventHandler IconsChanged;

	public TypeEditFileIconUriProvider()
	{
		AppServices.TypeEditSchema.Initialised += TypeEditSchema_Initialised;
		if (AppServices.TypeEditSchema.IsInitialised)
		{
			onTypeEditSchemaInitialised();
		}
	}

	private void TypeEditSchema_Initialised(object sender, EventArgs e)
	{
		onTypeEditSchemaInitialised();
	}

	private void onTypeEditSchemaInitialised()
	{
		IconsChanged?.Invoke(this, EventArgs.Empty);
	}

	public Uri GetIconUriForDirectory(string dirPath, string dirRelativePath)
	{
		if (dirRelativePath.Trim().Length > 0)
		{
			Uri uri = new Uri(Path.GetFullPath(dirPath) + "\\");
			Uri uri2 = new Uri(Path.GetFullPath(Path.Combine(dirPath, dirRelativePath)));
			Uri uri3 = uri.MakeRelativeUri(uri2);
			string text = uri3.ToString();
			Uri.UnescapeDataString(text);
			string[] array = text.Split(new char[2]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			});
			if (array.Length == 3)
			{
				switch (array[2].ToUpperInvariant())
				{
				case "AUDIO":
					return new Uri("pack://application:,,,/Resources/dir_audio.ico");
				case "CAMERAS":
					return new Uri("pack://application:,,,/Resources/dir_cameras.ico");
				case "ENVIRONMENT":
					return new Uri("pack://application:,,,/Resources/dir_environment.ico");
				case "PARTICLES":
					return new Uri("pack://application:,,,/Resources/dir_particles.ico");
				case "RAILNETWORK":
					return new Uri("pack://application:,,,/Resources/dir_railnetwork.ico");
				case "RAILVEHICLES":
					return new Uri("pack://application:,,,/Resources/dir_railvehicles.ico");
				case "ROUTEMARKERS":
					return new Uri("pack://application:,,,/Resources/dir_routemarkers.ico");
				case "SCENERY":
					return new Uri("pack://application:,,,/Resources/dir_scenery.ico");
				case "STATIONS":
					return new Uri("pack://application:,,,/Resources/dir_stations.ico");
				case "SYSTEM":
					return new Uri("pack://application:,,,/Resources/dir_system.ico");
				case "TEMPLATEROUTES":
				case "TEMPLATECONSISTS":
				case "PRELOAD":
					return new Uri("pack://application:,,,/Resources/dir_templateroutes.ico");
				case "TIMEOFDAY":
					return new Uri("pack://application:,,,/Resources/dir_timeofday.ico");
				case "WEATHER":
					return new Uri("pack://application:,,,/Resources/dir_weather.ico");
				}
			}
		}
		return new Uri("pack://application:,,,/Resources/folder_open.ico");
	}

	public Uri GetIconUriForFile(string dirPath, string fileNameRelativePath)
	{
		switch (Path.GetExtension(fileNameRelativePath).ToUpperInvariant())
		{
		case ".XML":
			try
			{
				ITypeDescriptor rootTypeFromFile = AppServices.TypeEditSchema.GetRootTypeFromFile(Path.Combine(dirPath, fileNameRelativePath));
				if (rootTypeFromFile != null)
				{
					if (rootTypeFromFile.GetEditHint("ShapeBlueprint") != null)
					{
						return new Uri("pack://application:,,,/Resources/shape_blueprint.ico");
					}
					if (rootTypeFromFile.GetEditHint("AudioControlBlueprint") != null)
					{
						return new Uri("pack://application:,,,/Resources/sound_blueprint.ico");
					}
					if (rootTypeFromFile.GetEditHint("Blueprint") != null)
					{
						return new Uri("pack://application:,,,/Resources/blueprint.ico");
					}
				}
			}
			catch (Exception)
			{
			}
			break;
		case ".LUA":
			return new Uri("pack://application:,,,/Resources/luafile.ico");
		case ".IGS":
			return new Uri("pack://application:,,,/Resources/object.ico");
		case ".WAV":
			return new Uri("pack://application:,,,/Resources/icon.ico");
		case ".CSV":
			return new Uri("pack://application:,,,/Resources/csvfile.ico");
		case ".IA":
			return new Uri("pack://application:,,,/Resources/animation.ico");
		case ".ACE":
		case ".KIF":
		case ".DDS":
			return new Uri("pack://application:,,,/Resources/acefile.ico");
		}
		return null;
	}
}
