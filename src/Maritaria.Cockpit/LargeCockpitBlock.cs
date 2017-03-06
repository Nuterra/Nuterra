using System;
using System.Collections.Generic;
using Nuterra;
using UnityEngine;

namespace Maritaria.Cockpit
{
	public sealed class LargeCockpitBlock : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Cockpit/Cockpit.Large.Icon.png";
		public static readonly string ModelFile = @"Assets/Blocks/Cockpit/Cockpit.Large.prefab";

		public static readonly int BlockID = 9005;
		int CustomBlock.BlockID => BlockID;
		public string Name => "GSO Observatory";
		public string Description => "Mount this gigantic hamsterball to your tech to be right in the action!";
		public int Price => 300;
		public FactionSubTypes Faction => FactionSubTypes.GSO;
		public BlockCategories Category => BlockCategories.Accessories;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);

		public LargeCockpitBlock()
		{
			Prefab = new BlockPrefabBuilder()
				.SetBlockID(BlockID)
				.SetName(Name)
				.SetCategory(Category)
				.SetModel(ModelFile)
				.SetSize(new Vector3I(2, 2, 2))
				.Build();

			ModuleFirstPerson firstPerson = Prefab.EnsureComponent<ModuleFirstPerson>();

			//var collider = renderObject.EnsureComponent<BoxCollider>();
			//collider.size = new Vector3(2, 2, 2);
			//collider.center = Vector3.one / 2;
		}
	}
}