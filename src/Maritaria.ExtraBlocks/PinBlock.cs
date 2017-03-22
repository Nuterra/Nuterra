using System;
using Nuterra;
using UnityEngine;

namespace Maritaria.ExtraBlocks
{
	public sealed class PinBlock : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Test/PinBlock.png";
		public static readonly string ModelFile = @"Assets/Blocks/Test/PinBlock.prefab";

		public static readonly int BlockID = 9012;

		int CustomBlock.BlockID => BlockID;
		public string Name => "EXP PinBlock";
		public string Description => "Thanks to @NGreyswandir";
		public int Price => 500;
		public FactionSubTypes Faction => FactionSubTypes.EXP;
		public BlockCategories Category => BlockCategories.Standard;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);

		public PinBlock()
		{
			Prefab = new BlockPrefabBuilder()
				.SetBlockID(BlockID)
				.SetName(Name)
				.SetCategory(Category)
				.SetModel(ModelFile)
				.SetSize(new Vector3I(3, 1, 1))
				.Build();
		}
	}
}