using System;
using System.IO;

namespace Nuterra.InstallProcess
{
	public static class AfterHooking
	{
		public static void InstallDataDirectory(string installDataDir, string terraTechBaseDir)
		{
			string dataDir = Path.Combine(terraTechBaseDir, "Nuterra_Data");
			if (!Directory.Exists(dataDir))
			{
				CreateFromScratch(installDataDir, dataDir);
			}
			else
			{
				UpdateDataDirectory(installDataDir, dataDir);
			}
		}

		private static void CreateFromScratch(string installDataDir, string dataDir)
		{
			Directory.CreateDirectory(dataDir);
			CopyOverDataFiles(installDataDir, dataDir);
			//Directory.CreateDirectory(Path.Combine(dataDir, "Mods"));
		}

		private static void UpdateDataDirectory(string installDataDir, string dataDir)
		{
			CopyOverDataFiles(installDataDir, dataDir);
		}

		private static void CopyOverDataFiles(string installDataDir, string dataDir)
		{
			CopyOverDataFile(installDataDir, dataDir, "mod-nuterra");
			CopyOverDataFile(installDataDir, dataDir, "mod-nuterra.manifest");
		}

		private static void CopyOverDataFile(string installDataDir, string dataDir, string filePath)
		{
			File.Copy(Path.Combine(installDataDir, filePath), Path.Combine(dataDir, filePath), true);
		}
	}
}