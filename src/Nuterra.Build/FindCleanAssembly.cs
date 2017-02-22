using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
