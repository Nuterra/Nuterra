using System;
using dnlib.DotNet;

namespace Nuterra.Build
{
	public sealed class LoadAssemblyCSharp : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			AssemblyResolver resolver = new AssemblyResolver();
			resolver.PreSearchPaths.Add(info.TerraTechManaged);
			info.AssemblyCSharp = ModuleDefMD.Load(info.CleanAssemblyPath);
		}
	}
}