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
				throw new ModificationException("Please start the game from the TerraTech root directory (TerraTech*_Data could not be found).");
			}
			info.TerraTechData = terraTechData;
		}
	}
}