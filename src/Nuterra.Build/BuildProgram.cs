using System;
using System.IO;
using System.Linq;

namespace Nuterra.Build
{
	internal static class BuildProgram
	{
		private static void Main(string[] args)
		{
			ModificationInfo info = new ModificationInfo();
			for (int i = 0; i < args.Length; i++)
			{
				string param = args[i];
				string next = (i < args.Length) ? args[i + 1] : null;
				bool skipNext = false;

				if (param.StartsWith(CommonSwitches.TerraTechDir, StringComparison.OrdinalIgnoreCase))
				{
					info.TerraTechRoot = next;
					skipNext = true;
				}

				if (param.StartsWith(CommonSwitches.AssemblyHash, StringComparison.OrdinalIgnoreCase))
				{
					info.ExpectedHash = next;
					skipNext = true;
				}

				if (param.StartsWith(CommonSwitches.AssemblyOutput, StringComparison.OrdinalIgnoreCase))
				{
					info.AssemblyCSharpOutputPath = next;
					skipNext = true;
				}

				if (param.StartsWith(CommonSwitches.AccessFile, StringComparison.OrdinalIgnoreCase))
				{
					info.AccessFilePath = next;
					skipNext = true;
				}

				if (skipNext)
				{
					i++;
				}
			}

			if (info.ExpectedHash != null && info.ExpectedHash.Contains('.'))
			{
				//*.* treat as file
				if (!File.Exists(info.ExpectedHash))
				{
					Error.MissingFile(info.ExpectedHash);
					return;
				}
				info.ExpectedHash = File.ReadAllText(info.ExpectedHash);
			}

			if (info.ExpectedHash == null)
			{
				Error.NoHashSpecified();
				return;
			}

			var modificationChain = new VerifyRootDirectory();
			modificationChain
				.SetNext(new FindCleanAssembly())
				.SetNext(new LoadAssemblyCSharp())
				.SetNext(new ModifyAccessors())
				.SetNext(new SaveAssemblyCSharp());

			if (modificationChain.ExecuteSteps(info))
			{
				Console.WriteLine("Install successfull");
			}
			else
			{
				Console.WriteLine("Install failed");
			}
			Console.WriteLine("Press enter to finish program");
			Console.ReadLine();
		}
	}
}