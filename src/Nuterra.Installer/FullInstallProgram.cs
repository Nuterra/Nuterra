using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nuterra.Installer
{
	internal static class FullInstallProgram
	{
		public static readonly string NuterraDataDir = "Nuterra_Data";
		public static readonly string NuterraAssemblyFile = "Nuterra.dll";
		public static readonly string ExpectedHashFile = Path.Combine(NuterraDir, "installer.hash.txt");
		public static readonly string AccessFile = Path.Combine(NuterraDir, "installer.access.txt");
		public static readonly string TempOutputFile = Path.Combine(NuterraDir, "installer.modded.dll");
		public static readonly string GalaxyAssemblyFile = "GalaxyCSharp.dll";

		internal static void Main(string[] args)
		{
			Console.WriteLine("Verifying install location");

			//Verify current directory
			string terraTechRoot = Directory.GetCurrentDirectory();
			string terraTechData = Directory.EnumerateDirectories(terraTechRoot, "TerraTech*_Data").SingleOrDefault();

			if (terraTechData == null)
			{
				Error.InvalidStartupLocation();
				return;
			}

			//Check required files from package
			if (!File.Exists(ExpectedHashFile))
			{
				Error.MissingFile(ExpectedHashFile);
				return;
			}

			//Find original assembly
			string expectedHash = File.ReadAllText(ExpectedHashFile);
			string terraTechManagedDir = Path.Combine(terraTechData, "Managed");
			string assemblyCSharpPath = Path.Combine(terraTechManagedDir, "Assembly-CSharp.dll");
			string assemblyBackupDir = Path.Combine(terraTechManagedDir, "NuterraBackups");
			string assemblyHash = InstallerUtil.GetFileHash(assemblyCSharpPath);
			string assemblyBackupPath;
			if (assemblyHash == expectedHash)
			{
				//Current assembly is clean install, make backup
				assemblyBackupPath = InstallerUtil.CreateAssemblyBackup(assemblyCSharpPath, assemblyBackupDir, assemblyHash);
			}
			else
			{
				//Current assembly is dirty, check for backup and otherwise warn
				string backupPath = InstallerUtil.FormatBackupPath(assemblyBackupDir, expectedHash);
				if (!File.Exists(backupPath))
				{
					Error.NoCleanBackup();
					return;
				}
				assemblyBackupPath = backupPath;
			}

			//Mod assembly
			Console.WriteLine("Modding Assembly-CSharp.dll");
			Merger.MergeNuterra(terraTechManagedDir, assemblyBackupPath, NuterraAssemblyFile, AccessFile, TempOutputFile);

			Console.WriteLine("Installing modded Assembly-CSharp.dll");
			File.Copy(TempOutputFile, assemblyCSharpPath, overwrite: true);

			//Install GalaxyCSharp.dll dependency
			File.Copy(Path.Combine(NuterraDataDir, GalaxyAssemblyFile), Path.Combine(terraTechManagedDir, GalaxyAssemblyFile), overwrite: true);

			Console.WriteLine("Install completed, have fun :3");
			Console.ReadLine();
		}
	}
}