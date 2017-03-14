using Nuterra;
using System;
using UnityEngine;

namespace Maritaria
{
	public sealed class BaconBlock : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Bacon/bacon_icon.png";
		public static readonly string ModelFile = @"Assets/Blocks/Bacon/BaconBlock.prefab";

		public static readonly int BlockID = 9001;

		int CustomBlock.BlockID => BlockID;
		public string Name => "GSO Bacon strip";
		public string Description => "A long strip of bacon with bullet absoring grease";
		public int Price => 500;
		public FactionSubTypes Faction => FactionSubTypes.GSO;
		public BlockCategories Category => BlockCategories.Accessories;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);

		public BaconBlock()
		{
			Prefab = new BlockPrefabBuilder()
				.SetBlockID(BlockID)
				.SetName(Name)
				.SetCategory(Category)
				.SetModel(ModelFile)
				.SetSize(new Vector3I(4, 1, 2))
				.Build();
		}
	}
}