using System;

namespace Nuterra.Build
{
	public sealed class FindCleanAssembly : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			foreach(string hash in info.AcceptedHashes)
			{
				string cleanAssemblyPath = AssemblyCSharpUtil.FindCleanAssembly(info.TerraTechManaged, hash);
				if (cleanAssemblyPath != null)
				{
					info.CleanAssemblyPath = cleanAssemblyPath;
					return;
				}
			}
			Error.NoCleanBackup();
		}
	}
}