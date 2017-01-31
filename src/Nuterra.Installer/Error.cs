using System;

namespace Nuterra.Installer
{
	public static class Error
	{
		public static void InvalidStartupLocation()
		{
			Console.WriteLine("Please start the game from the TerraTech root directory (TerraTech*_Data could not be found).");
			Console.ReadLine();
		}

		public static void MissingPackage()
		{
			Console.WriteLine("The selected release has no installable package associated with it. Please contact the modteam to have it fixed.");
			Console.ReadLine();
		}

		public static void MissingModdedAssembly(string filename)
		{
			Console.WriteLine($"Missing modded assembly. Should be named '{filename}'.");
			Console.ReadLine();
		}

		public static void MissingAccessorFile(string filename)
		{
			Console.WriteLine($"Missing access mod file to use for modding the assembly. Should be named '{filename}'.");
			Console.ReadLine();
		}

		public static void MissingFile(string fileName)
		{
			Console.WriteLine($"The downloaded package is missing a required file: '{fileName}'. Please contact the modteam to have it fixed.");
			Console.ReadLine();
		}

		public static void NoCleanBackup()
		{
			Console.WriteLine("Your local install cannot be used to install Nuterra into, and no compatible backup exists");
			Console.ReadLine();
		}

		public static void InvalidInstallMode(string installMode)
		{
			Console.WriteLine($"Invalid installmode specified, valid values are: '{string.Join("', '", Enum.GetNames(typeof(InstallMode)))}'");
			Console.ReadLine();
		}
	}
}