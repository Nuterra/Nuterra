using System;
using System.IO;
using System.Security.Cryptography;

namespace Nuterra.Installer
{
	public static class InstallerUtil
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
			string targetFile = Path.Combine(assemblyBackupDir, $"{hash}.dll");
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
			return Path.Combine(assemblyBackupDir, $"{hash}.dll");
		}
	}
}