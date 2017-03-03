using System;
using System.IO;
using System.Linq;
using Nuterra.Build;

namespace Nuterra.Installer
{
	internal static class InstallProgram
	{
		internal static void Main(string[] args)
		{
			try
			{
				MainInternal(args);
				Console.WriteLine("Install successfull");
			}
			catch (ModificationException ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine("Install failed");
			}
		}

		private static void MainInternal(string[] args)
		{
			var info = ModificationInfo.Parse(args);
			var modificationChain = new VerifyRootDirectory();
			modificationChain
				.SetNext(new FindCleanAssembly())
				.SetNext(new LoadAssemblyCSharp())
				.SetNext(new ModifyAccessors())
				.SetNext(new AbsorbNuterra())
				.SetNext(new HookNuterra())
				.SetNext(new SaveAssemblyCSharp())
				.SetNext(new FixMissingDependencies());

			modificationChain.ExecuteSteps(info);
		}
	}
}