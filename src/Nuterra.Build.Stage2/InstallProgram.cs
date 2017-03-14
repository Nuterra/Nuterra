using System;

namespace Nuterra.Build
{
	public static class InstallProgram
	{
		internal static void Main(string[] args)
		{
			try
			{
				var info = ModificationInfo.Parse(args);
				PerformInstall(info);
				Console.WriteLine("Install successfull");
			}
			catch (ModificationException ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine("Install failed");
			}
		}

		public static void PerformInstall(ModificationInfo info)
		{
			if (info == null) throw new ArgumentNullException(nameof(info));
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