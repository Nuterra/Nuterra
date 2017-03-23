using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuterra;
using UnityEngine;

namespace Maritaria.ExtraBlocks
{
	class HoleBlock : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Test/HoleBlock.png";
		public static readonly string ModelFile = @"Assets/Blocks/Test/HoleBlock.prefab";

		public static readonly int BlockID = 9013;

		int CustomBlock.BlockID => BlockID;
		public string Name => "EXP HoleBlock";
		public string Description => "Thanks to @NGreyswandir";
		public int Price => 500;
		public FactionSubTypes Faction => FactionSubTypes.EXP;
		public BlockCategories Category => BlockCategories.Standard;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);

		public HoleBlock()
		{
			Prefab = new BlockPrefabBuilder()
				.SetCategory(Category)
				.SetModel(ModelFile)
				.Build();
		}
	}
}
