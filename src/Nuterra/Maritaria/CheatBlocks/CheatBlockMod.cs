using System;
using Nuterra;

namespace Maritaria.CheatBlocks
{
	[Mod]
	public sealed class CheatBlockMod : TerraTechMod
	{
		public override string Name => nameof(CheatBlockMod);
		public override string Description => "Adds a set of blocks to R&D that activate cheats when put onto a tech";

		public override void Load()
		{
			base.Load();
			BlockLoader.Register(new GodModeBlock());
		}

	}
}