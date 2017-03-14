using System;

namespace Nuterra.Build
{
	public sealed class FindCleanAssembly : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			string cleanAssemblyPath = AssemblyCSharpUtil.FindCleanAssembly(info.TerraTechManaged, info.ExpectedHash);
			if (cleanAssemblyPath == null)
			{
				Error.NoCleanBackup();
			}
			info.CleanAssemblyPath = cleanAssemblyPath;
		}
	}
}