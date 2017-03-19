using System;
using System.IO;
using System.Security.Cryptography;

namespace Nuterra.Build
{
	public static class AssemblyCSharpUtil
	{
		public static string GetFileHash(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return null;
			}
			using (var md5 = MD5.Create())
			using (var stream = File.OpenRead(filePath))
			{
				return Convert.ToBase64String(md5.ComputeHash(stream));
			}
		}

		public static string CreateAssemblyBackup(string sourceAssembly, string assemblyBackupDir, string hash)
		{
			string targetFile = FormatBackupPath(assemblyBackupDir, hash);
			if (!Directory.Exists(assemblyBackupDir))
			{
				Directory.CreateDirectory(assemblyBackupDir);
			}
			if (!File.Exists(targetFile))
			{
				File.Copy(sourceAssembly, targetFile);
			}
			return targetFile;
		}

		public static string FormatBackupPath(string assemblyBackupDir, string hash)
		{
			hash = hash.Replace('/', '-');
			return Path.Combine(assemblyBackupDir, $"{hash}.dll");
		}

		public static string FindCleanAssembly(string terraTechManagedDir, string expectedHash)
		{
			string assemblyCSharpPath = Path.Combine(terraTechManagedDir, "Assembly-CSharp.dll");
			string assemblyBackupDir = Path.Combine(terraTechManagedDir, "NuterraBackups");
			string assemblyHash = AssemblyCSharpUtil.GetFileHash(assemblyCSharpPath);
			Console.WriteLine($"Raw assembly hash: `{assemblyHash}`");
			if (assemblyHash == expectedHash)
			{
				//Current assembly is clean install, make backup
				return AssemblyCSharpUtil.CreateAssemblyBackup(assemblyCSharpPath, assemblyBackupDir, assemblyHash);
			}
			else
			{
				//Current assembly is dirty, check for backup and otherwise warn
				string backupPath = AssemblyCSharpUtil.FormatBackupPath(assemblyBackupDir, expectedHash);
				if (File.Exists(backupPath))
				{
					return backupPath;
				}
			}
			return null;
		}
	}
}