using System;

namespace Nuterra.Build
{
	internal static class BuildProgram
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
				.SetNext(new SaveAssemblyCSharp());

			modificationChain.ExecuteSteps(info);
		}
	}
}