using Nuterra;
using System;
using UnityEngine;

namespace Maritaria.Cockpit
{
	public sealed class CockpitBlock : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Cockpit/block_icon.png";
		public static readonly string ModelFile = @"Assets/Blocks/Cockpit/CockpitBlock.prefab";

		public static readonly int BlockID = 9000;
		int CustomBlock.BlockID => BlockID;
		public string Name => "GSO Cockpit";
		public string Description => "Pop in here and have a first-person look at the world from this block";
		public int Price => 300;
		public FactionSubTypes Faction => FactionSubTypes.GSO;
		public BlockCategories Category => BlockCategories.Accessories;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);

		public CockpitBlock()
		{
			Prefab = new BlockPrefabBuilder()
				.SetBlockID(BlockID)
				.SetName(Name)
				.SetCategory(Category)
				.SetModel(ModelFile)
				.SetSize(new Vector3I(1, 1, 1))
				.Build();

			var block = Prefab.GetComponent<TankBlock>();
			Console.WriteLine("block: " + (block != null));
			var moduleDamage = Prefab.GetComponent<ModuleDamage>();
			Console.WriteLine("ModuleDamage: " + (moduleDamage != null));
			Console.WriteLine("ModuleDamage.Block: " + (moduleDamage?.block));
			ModuleFirstPerson firstPerson = Prefab.EnsureComponent<ModuleFirstPerson>();
		}
	}
}