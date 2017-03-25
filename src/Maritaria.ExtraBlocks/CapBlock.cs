using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuterra;
using UnityEngine;

namespace Maritaria.ExtraBlocks
{
	public sealed class CapBlock : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Test/CapBlock.png";
		public static readonly string ModelFile = @"Assets/Blocks/Test/CapBlock.prefab";

		public static readonly int BlockID = 9011;

		int CustomBlock.BlockID => BlockID;
		public string Name => "EXP CapBlock";
		public string Description => "Thanks to @NGreyswandir";
		public int Price => 500;
		public FactionSubTypes Faction => FactionSubTypes.EXP;
		public BlockCategories Category => BlockCategories.Standard;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);

		public CapBlock()
		{
			Prefab = new BlockPrefabBuilder()
				.SetBlockID(BlockID)
				.SetName(Name)
				.SetCategory(Category)
				.SetModel(ModelFile)
				.SetSize(new Vector3I(1, 1, 1))
				.Build();
		}
	}
}
