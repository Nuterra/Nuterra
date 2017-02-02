using Nuterra;
using System;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public sealed class SmileyBlock : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Cockpit/GSO_Observatory_icon.png";
		public static readonly string ModelFile = @"Assets/Blocks/Cockpit/TT_GSO_Observatory_Block.blend";

		public static readonly int BlockID = 9000;
		int CustomBlock.BlockID => BlockID;
		public string Name => "GSO Cockpit";
		public string Description => "Pop in here and have a first-person look at the world from this block";
		public FactionSubTypes Faction => FactionSubTypes.GSO;
		public BlockCategories Category => BlockCategories.Accessories;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);

		public SmileyBlock()
		{
			Prefab = new GameObject();
			GameObject.DontDestroyOnLoad(Prefab);
			Prefab.name = Name;
			Prefab.tag = "TankBlock";
			Prefab.layer = Globals.inst.layerTank;

			Visible vis = Prefab.EnsureComponent<Visible>();
			vis.m_ItemType = new ItemTypeInfo(ObjectTypes.Block, BlockID);

			Damageable dmg = Prefab.EnsureComponent<Damageable>();
			ModuleDamage modDamage = Prefab.EnsureComponent<ModuleDamage>();
			AutoSpriteRenderer spriteRenderer = Prefab.EnsureComponent<AutoSpriteRenderer>();
			Prefab.EnsureComponent<TankBlock>();
			TankBlock tankBlock = Prefab.GetComponent<TankBlock>();

			tankBlock.m_BlockCategory = Category;
			tankBlock.attachPoints = new Vector3[]{
				new Vector3(0, -0.5f, 0),
			};
			tankBlock.filledCells = new Vector3[] { new Vector3(0, 0, 0) };
			tankBlock.partialCells = new Vector3[] { };

			GameObject renderObject = AssetBundleImport.Load<GameObject>(ModelFile);
			renderObject.transform.parent = Prefab.transform;
			renderObject.name = $"{Name}.Model";
			renderObject.layer = Globals.inst.layerTank;
			var collider = renderObject.EnsureComponent<BoxCollider>();
			collider.size = Vector3.one;
		}
	}
}