using System;

namespace Nuterra.Build
{
	public static class Error
	{

		public static void InvalidRootDirectory()
		{
			DoError("Please start the game from the TerraTech root directory (TerraTech*_Data could not be found).");
		}

		public static void MissingPackage()
		{
			DoError("The selected release has no installable package associated with it. Please contact the modteam to have it fixed.");
			Console.ReadLine();
		}

		public static void MissingModdedAssembly(string filename)
		{
			DoError($"Missing modded assembly. Should be named '{filename}'.");
			Console.ReadLine();
		}

		public static void MissingAccessorFile(string filename)
		{
			DoError($"Missing access mod file to use for modding the assembly. Should be named '{filename}'.");
			Console.ReadLine();
		}

		public static void MissingFile(string fileName)
		{
			DoError($"The downloaded package is missing a required file: '{fileName}'. Please contact the modteam to have it fixed.");
			Console.ReadLine();
		}

		public static void NoCleanBackup()
		{
			DoError("Your local install cannot be used to install Nuterra into, and no compatible backup exists");
		}

		public static void NoHashSpecified()
		{
			DoError($"No hash has been specified, this is required to find a clean version of TerraTech. Please specify a hash using '{CommonSwitches.AssemblyHash}'");
		}

		private static void DoError(string message)
		{
			throw new ModificationException(message);
		}
	}
}