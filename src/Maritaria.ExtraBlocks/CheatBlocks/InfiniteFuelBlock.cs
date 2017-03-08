using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuterra;
using UnityEngine;

namespace Maritaria.CheatBlocks
{
	public sealed class InfiniteFuelBlock : CustomBlock
	{
		public static readonly int BlockID = 9004;
		public static readonly string ModelPrefab = @"Assets/Blocks/Powerups/Fuel/FuelBlock.prefab";
		public static readonly string SpritePrefab = @"Assets/Blocks/Powerups/Fuel/block_icon.png";

		int CustomBlock.BlockID => BlockID;
		public string Name => "Cheat: Fuel";
		public string Description => "Gives infinite fuel to any tech placed on it";
		public int Price => 9000;
		public BlockCategories Category => BlockCategories.Accessories;
		public FactionSubTypes Faction => FactionSubTypes.EXP;
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpritePrefab);
		public GameObject Prefab { get; }

		public InfiniteFuelBlock()
		{
			Prefab = new BlockPrefabBuilder()
				.SetBlockID(BlockID)
				.SetName(Name)
				.SetCategory(Category)
				.SetModel(ModelPrefab)
				.SetSize(new Vector3I(1, 2, 1))
				.SetMass(0.01f)
				.Build();

			ModuleInfiniteFuel fuelModule = Prefab.EnsureComponent<ModuleInfiniteFuel>();
		}
	}
}
