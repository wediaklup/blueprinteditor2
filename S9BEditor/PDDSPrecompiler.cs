using System;
using System.IO;
using System.Diagnostics;

namespace S9BEditor
{
    public class PDDSPrecompiler
    {
        private static string GetRelativePath(string basePath, string fullPath)
        {
            if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                basePath += Path.DirectorySeparatorChar;

            Uri baseUri = new Uri(basePath, UriKind.Absolute);
            Uri fullUri = new Uri(fullPath, UriKind.Absolute);

            return Uri.UnescapeDataString(
                baseUri.MakeRelativeUri(fullUri).ToString()
            ).Replace('/', Path.DirectorySeparatorChar);
        }
        
        public static void PrecompileDirectory(String sourcePath, String assetsPath)
        {
            // Path to RailWorks + ConvertToTG.exe
            String utilityPath = Path.Combine(AppServices.ResourceManager.DeploymentPath, "ConvertToTG.exe");

            foreach (String sourceFile in Directory.EnumerateFiles(sourcePath, "*.dds", SearchOption.AllDirectories))
            {
                PLogger.Write("PPDSPrecompiler: source " + sourceFile);
                // Relativen Pfad zum Source-Verzeichnis bestimmen,
                // z.B. "Textures\Foo\bar.dds"
                String relativePath = GetRelativePath(sourcePath, sourceFile);
                PLogger.Write("PPDSPrecompiler: relative path " + relativePath);

                // Daraus den äquivalenten Pfad im Target-Verzeichnis bauen
                String targetFile = Path.Combine(assetsPath, relativePath);
                targetFile = targetFile.Remove(targetFile.Length - 3, 3) + "TgPcDx";
                PLogger.Write("PPDSPrecompiler: target " + targetFile);

                // Test ob Datei konvertiert werden muss
                if (File.Exists(targetFile) && File.GetLastWriteTimeUtc(sourceFile) < File.GetLastWriteTimeUtc(targetFile)) continue;
                
                // Zielverzeichnis sicherstellen
                String targetDirectory = Path.GetDirectoryName(targetFile);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                    PLogger.Write("PPDSPrecompiler: Creating directory " + targetDirectory);
                }
                
                String relativeSourceFile = GetRelativePath(AppServices.ResourceManager.DeploymentPath, sourceFile);
                String relativeTargetFile = GetRelativePath(AppServices.ResourceManager.DeploymentPath, targetFile);

                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = utilityPath;
                processStartInfo.Arguments = "-i \"" + relativeSourceFile + "\" -o \"" + relativeTargetFile + "\"";  // optional -nowindow
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;

                try
                {
                    Process process = Process.Start(processStartInfo);
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        PLogger.Write("PPDSPrecompiler: Process endet with nonzero exit code " + process.ExitCode);
                    }
                }
                catch (Exception e)
                {
                    PLogger.Write("PPDSPrecompiler: Exception starting process " + e.ToString());
                }
            }
        }
    }
}