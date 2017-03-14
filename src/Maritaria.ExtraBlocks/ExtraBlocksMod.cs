using System;
using Nuterra;

namespace Maritaria.ExtraBlocks
{
	public sealed class ExtraBlocksMod : TerraTechMod
	{
		public override string Name => nameof(ExtraBlocksMod);
		public override string Description => "Adds more blocks to the game";

		public override void Load()
		{
			base.Load();
			BlockLoader.Register(new BaconBlock());
			BlockLoader.Register(new CheatBlocks.GodModeBlock());
			BlockLoader.Register(new CheatBlocks.InfiniteFuelBlock());
		}
	}
}