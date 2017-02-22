using System;
using System.IO;
using System.Linq;
using Nuterra.Build;

namespace Nuterra.Installer
{
	internal static class InstallProgram
	{
		//Install data
		public static readonly string NuterraDataDir = "Nuterra_Data";

		public static readonly string NuterraAssemblyFile = "Nuterra.dll";
		public static readonly string ExpectedHashFile = Path.Combine(NuterraDataDir, "installer.hash.txt");
		public static readonly string AccessFile = Path.Combine(NuterraDataDir, "installer.access.txt");
		public static readonly string TempOutputFile = Path.Combine(NuterraDataDir, "installer.modded.dll");
		public static readonly string GalaxyAssemblyFile = "GalaxyCSharp.dll";

		//Command line switches
		public static readonly string OverrideTerraTechDirSwitch = "--dir";
		public static readonly string OverrideAssemblyOutputSwitch = "--out";
		public static readonly string OverrideAccessFileSwitch = "--accessfile";

		internal static void Main(string[] args)
		{
			InstallMode installMode = InstallMode.FullInstall;
			string terraTechRoot = Directory.GetCurrentDirectory();
			string assemblyCSharpOutputPathOverride = null;
			string accessFileOverride = null;

			for (int i = 0; i < args.Length; i++)
			{
				string param = args[i];
				string next = (i < args.Length) ? args[i + 1] : null;
				bool skipNext = false;

				//--dir
				if (param.StartsWith(OverrideTerraTechDirSwitch, StringComparison.OrdinalIgnoreCase))
				{
					terraTechRoot = next;
					skipNext = true;
				}

				//--out
				if (param.StartsWith(OverrideAssemblyOutputSwitch, StringComparison.OrdinalIgnoreCase))
				{
					assemblyCSharpOutputPathOverride = next;
					skipNext = true;
				}

				//--accessfile
				if (param.StartsWith(OverrideAccessFileSwitch, StringComparison.OrdinalIgnoreCase))
				{
					accessFileOverride = next;
					skipNext = true;
				}

				if (skipNext)
				{
					i++;
				}
			}

			Console.WriteLine("Verifying install location");
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
			string assemblyHash = AssemblyCSharpUtil.GetFileHash(assemblyCSharpPath);
			string assemblyBackupPath;
			if (assemblyHash == expectedHash)
			{
				//Current assembly is clean install, make backup
				assemblyBackupPath = AssemblyCSharpUtil.CreateAssemblyBackup(assemblyCSharpPath, assemblyBackupDir, assemblyHash);
			}
			else
			{
				//Current assembly is dirty, check for backup and otherwise warn
				string backupPath = AssemblyCSharpUtil.FormatBackupPath(assemblyBackupDir, expectedHash);
				if (!File.Exists(backupPath))
				{
					Error.NoCleanBackup();
					return;
				}
				assemblyBackupPath = backupPath;
			}

			Console.WriteLine("Modding Assembly-CSharp.dll");
			AssemblyModifier modder = new AssemblyModifier(terraTechManagedDir, assemblyBackupPath);
			switch (installMode)
			{
				case InstallMode.FullInstall:
					modder.ApplyAccessorMod(accessFileOverride ?? AccessFile);
					modder.MergeNuterra(NuterraAssemblyFile);
					modder.HookNuterra();
					break;

				case InstallMode.AccessorOnly:
					modder.ApplyAccessorMod(accessFileOverride ?? AccessFile);
					break;

				default:
					throw new NotSupportedException();
			}
			modder.Write(TempOutputFile);

			if (assemblyCSharpOutputPathOverride != null)
			{
				Console.WriteLine("Copying Assembly-CSharp.dll to target destination");
				File.Copy(TempOutputFile, assemblyCSharpOutputPathOverride, overwrite: true);
			}
			else
			{
				Console.WriteLine("Installing modded Assembly-CSharp.dll");
				File.Copy(TempOutputFile, assemblyCSharpPath, overwrite: true);
			}

			if (installMode == InstallMode.FullInstall)
			{
				Console.WriteLine("Installing dependency: GalaxyCSharp.dll");
				File.Copy(Path.Combine(NuterraDataDir, GalaxyAssemblyFile), Path.Combine(terraTechManagedDir, GalaxyAssemblyFile), overwrite: true);
			}

			Console.WriteLine("Install completed, have fun :3 (press enter to finish)");
			Console.ReadLine();
		}
	}
}