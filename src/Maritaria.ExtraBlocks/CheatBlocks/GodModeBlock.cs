using System;
using UnityEngine;
using Nuterra;

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
			ModuleGodMode godModule = Prefab.EnsureComponent<ModuleGodMode>();

			TankBlock tankBlock = Prefab.EnsureComponent<TankBlock>();
			tankBlock.m_BlockCategory = Category;

			tankBlock.attachPoints = new Vector3[]{
				Vector3.down / 2,
			};
			tankBlock.filledCells = new Vector3[]{
				new Vector3(0, 0, 0),
			};
			tankBlock.partialCells = new Vector3[] { };
			tankBlock.m_DefaultMass = 0.01f;

			GameObject renderObject = GameObject.Instantiate(AssetBundleImport.Load<GameObject>(ModelPrefab));
			renderObject.transform.parent = Prefab.transform;
			renderObject.name = $"{Name}.Model";
			renderObject.layer = Globals.inst.layerTank;
			renderObject.transform.localPosition = Vector2.down / 2;

			BoxCollider collider = renderObject.EnsureComponent<BoxCollider>();
			collider.size = new Vector3(1, 1, 1);
			collider.center = collider.size / 2;
		}
	}
}