using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nuterra.InstallProcess
{
	/// <summary>
	/// This class is used by the installer to hook up the code, this is done after merging nuterra into Assembly-CSharp.dll
	/// At this point the installer should open the current file with Mono.Cecil and apply changes.
	/// 
	/// The installer should load the image into the appdomain and call FinalizeInstall()
	/// TODO: Make this a seperate project so it can be loaded by the installer cleanly
	/// </summary>
	public static class AfterMerge
	{
		public static void FinalizeInstall(string searchDir, string assemblyPath, string assemblyOutputPath)
		{
			AssemblyDefinition assemblyCSharp = GetAssemblyCSharp(searchDir, assemblyPath);
			FixAssemblyName(assemblyCSharp);
			HookNuterra(assemblyCSharp);
			SelfDestruct(assemblyCSharp);
			using (FileStream output = File.OpenWrite(assemblyOutputPath))
			{
				assemblyCSharp.Write(output);
			}
		}

		private static void FixAssemblyName(AssemblyDefinition assemblyCSharp)
		{
			const string assemblyName = "Assembly-CSharp";

			assemblyCSharp.Name = new AssemblyNameDefinition(assemblyName, new Version(0, 0, 0, 0));
			assemblyCSharp.MainModule.Name = assemblyName;
		}

		private static void HookNuterra(AssemblyDefinition assembly)
		{
			Hooker.Apply(assembly);
		}

		private static void SelfDestruct(AssemblyDefinition assembly)
		{

		}

		private static AssemblyDefinition GetAssemblyCSharp(string searchDir, string assemblyPath)
		{
			var resolver = new DefaultAssemblyResolver();
			resolver.AddSearchDirectory(searchDir);
			return AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters { AssemblyResolver = resolver });
		}



	}
}
