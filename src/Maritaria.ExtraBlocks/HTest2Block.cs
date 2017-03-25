using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuterra;
using UnityEngine;

namespace Maritaria.ExtraBlocks
{
	public sealed class HTest2Block : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Test/HTest2Block.png";
		public static readonly string ModelFile = @"Assets/Blocks/Test/HTest2Block.prefab";

		public static readonly int BlockID = 9015;

		int CustomBlock.BlockID => BlockID;
		public string Name => "EXP HTest2Block";
		public string Description => "Thanks to @NGreyswandir";
		public int Price => 500;
		public FactionSubTypes Faction => FactionSubTypes.EXP;
		public BlockCategories Category => BlockCategories.Standard;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);

		public HTest2Block()
		{
			Prefab = new BlockPrefabBuilder()
				.SetBlockID(BlockID)
				.SetName(Name)
				.SetCategory(Category)
				.SetModel(ModelFile)
				.SetSize(new Vector3I(3, 2, 1))
				.Build();
		}
	}
}
