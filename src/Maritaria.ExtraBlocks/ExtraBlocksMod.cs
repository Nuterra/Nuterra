using System;
using Maritaria.CheatBlocks;
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

			new BlockPrefabBuilder()
				.SetBlockID(9001)
				.SetName("GSO Bacon strip")
				.SetDescription("A long strip of bacon with bullet absoring grease")
				.SetPrice(500)
				.SetFaction(FactionSubTypes.GSO)
				.SetCategory(BlockCategories.Accessories)
				.SetSize(new Vector3I(4, 1, 2))
				.SetModel(@"Assets/Blocks/Bacon/BaconBlock.prefab")
				.SetIcon(@"Assets/Blocks/Bacon/bacon_icon.png")
				.Register();

			new BlockPrefabBuilder()
				.SetBlockID(9002)
				.SetName("EXP Quart Hat Block")
				.SetDescription("Thanks to @NGreyswandir")
				.SetPrice(500)
				.SetFaction(FactionSubTypes.EXP)
				.SetCategory(BlockCategories.Standard)
				.SetSize(new Vector3I(1, 3, 1))
				.SetModel(@"Assets/Blocks/Test/QuarterHatBlock.prefab")
				.SetIcon(@"Assets/Blocks/Test/QuarterHatBlock.png")
				.Register();

			new BlockPrefabBuilder()
				.SetBlockID(9003)
				.SetName("EXP PinBlock")
				.SetDescription("Thanks to @NGreyswandir")
				.SetPrice(500)
				.SetFaction(FactionSubTypes.EXP)
				.SetCategory(BlockCategories.Standard)
				.SetSize(new Vector3I(3, 1, 1))
				.SetModel(@"Assets/Blocks/Test/PinBlock.prefab")
				.SetIcon(@"Assets/Blocks/Test/PinBlock.png")
				.Register();

			new BlockPrefabBuilder()
				.SetBlockID(9004)
				.SetName("EXP HoleBlock")
				.SetDescription("Thanks to @NGreyswandir")
				.SetPrice(500)
				.SetFaction(FactionSubTypes.EXP)
				.SetCategory(BlockCategories.Standard)
				.SetSize(new Vector3I(1, 1, 1))
				.SetModel(@"Assets/Blocks/Test/HoleBlock.prefab")
				.SetIcon(@"Assets/Blocks/Test/HoleBlock.png")
				.Register();

			new BlockPrefabBuilder()
				.SetBlockID(9006)
				.SetName("EXP CapBlock")
				.SetDescription("Thanks to @NGreyswandir")
				.SetPrice(500)
				.SetFaction(FactionSubTypes.EXP)
				.SetCategory(BlockCategories.Standard)
				.SetSize(new Vector3I(1, 1, 1))
				.SetModel(@"Assets/Blocks/Test/CapBlock.prefab")
				.SetIcon(@"Assets/Blocks/Test/CapBlock.png")
				.Register();

			new BlockPrefabBuilder()
				.SetBlockID(9007)
				.SetName("EXP HTest1Block")
				.SetDescription("Thanks to @NGreyswandir")
				.SetPrice(500)
				.SetFaction(FactionSubTypes.EXP)
				.SetCategory(BlockCategories.Standard)
				.SetSize(new Vector3I(3, 2, 1))
				.SetModel(@"Assets/Blocks/Test/HTest1Block.prefab")
				.SetIcon(@"Assets/Blocks/Test/HTest1Block.png")
				.Register();

			new BlockPrefabBuilder()
				.SetBlockID(9008)
				.SetName("EXP HTest2Block")
				.SetDescription("Thanks to @NGreyswandir")
				.SetPrice(500)
				.SetFaction(FactionSubTypes.EXP)
				.SetCategory(BlockCategories.Standard)
				.SetSize(new Vector3I(3, 2, 1))
				.SetModel(@"Assets/Blocks/Test/HTest2Block.prefab")
				.SetIcon(@"Assets/Blocks/Test/HTest2Block.png")
				.Register();

			new BlockPrefabBuilder()
				.SetBlockID(9009)
				.SetName("Cheat: Godmode")
				.SetDescription("Protects all attached blocks from all incomming damage")
				.SetPrice(9000)
				.SetFaction(FactionSubTypes.EXP)
				.SetCategory(BlockCategories.Accessories)
				.SetSize(new Vector3I(1, 2, 1))
				.SetMass(0.01f)
				.SetModel(@"Assets/Blocks/Powerups/Shield/GodBlock.prefab")
				.SetIcon(@"Assets/Blocks/Powerups/Shield/block_icon.png")
				.AddComponent<ModuleGodMode>()
				.Register();

			new BlockPrefabBuilder()
				.SetBlockID(9010)
				.SetName("Cheat: Fuel")
				.SetDescription("Gives infinite fuel to any tech placed on it")
				.SetPrice(9000)
				.SetFaction(FactionSubTypes.EXP)
				.SetCategory(BlockCategories.Accessories)
				.SetSize(new Vector3I(1, 2, 1))
				.SetMass(0.01f)
				.SetModel(@"Assets/Blocks/Powerups/Fuel/FuelBlock.prefab")
				.SetIcon(@"Assets/Blocks/Powerups/Fuel/block_icon.png")
				.AddComponent<ModuleInfiniteFuel>()
				.Register();
		}
	}
}