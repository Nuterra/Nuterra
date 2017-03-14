using System;
using System.IO;
using System.Linq;

namespace Nuterra.Build
{
	public class VerifyRootDirectory : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			string terraTechData = Directory.EnumerateDirectories(info.TerraTechRoot, "TerraTech*_Data").SingleOrDefault();
			if (terraTechData == null)
			{
				Error.InvalidRootDirectory();
			}
			info.TerraTechData = terraTechData;
		}
	}
}