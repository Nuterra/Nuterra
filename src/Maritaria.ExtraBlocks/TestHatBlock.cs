using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuterra;
using UnityEngine;

namespace Maritaria.ExtraBlocks
{
	class TestHatBlock : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Test/QuarterHatBlock.png";
		public static readonly string ModelFile = @"Assets/Blocks/Test/QuarterHatBlock.prefab";

		public static readonly int BlockID = 9010;

		int CustomBlock.BlockID => BlockID;
		public string Name => "EXP Quart Hat Block";
		public string Description => "Thanks to @NGreyswandir";
		public int Price => 500;
		public FactionSubTypes Faction => FactionSubTypes.EXP;
		public BlockCategories Category => BlockCategories.Standard;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);

		public TestHatBlock()
		{
			Prefab = new BlockPrefabBuilder()
				.SetBlockID(BlockID)
				.SetName(Name)
				.SetCategory(Category)
				.SetModel(ModelFile)
				.SetSize(new Vector3I(1, 3, 1))
				.Build();
		}
	}
}
