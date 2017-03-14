using System;
using Nuterra;
using UnityEngine;

namespace Maritaria.CheatBlocks
{
	public sealed class GodModeBlock : CustomBlock
	{
		public static readonly int BlockID = 9003;
		public static readonly string ModelPrefab = @"Assets/Blocks/Powerups/Shield/GodBlock.prefab";
		public static readonly string SpritePrefab = @"Assets/Blocks/Powerups/Shield/block_icon.png";

		int CustomBlock.BlockID => BlockID;
		public string Name => "Cheat: Godmode";
		public string Description => "Protects all attached blocks from all incomming damage";
		public int Price => 9000;
		public BlockCategories Category => BlockCategories.Accessories;
		public FactionSubTypes Faction => FactionSubTypes.EXP;
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpritePrefab);
		public GameObject Prefab { get; }

		public GodModeBlock()
		{
			Prefab = new BlockPrefabBuilder()
				.SetBlockID(BlockID)
				.SetName(Name)
				.SetCategory(Category)
				.SetModel(ModelPrefab)
				.SetSize(new Vector3I(1, 2, 1))
				.SetMass(0.01f)
				.Build();

			ModuleGodMode godModule = Prefab.EnsureComponent<ModuleGodMode>();
		}
	}
}