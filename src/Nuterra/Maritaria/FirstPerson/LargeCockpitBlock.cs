using System;
using System.Collections.Generic;
using Nuterra;
using UnityEngine;

namespace Maritaria.FirstPerson
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
			ModuleFirstPerson firstPerson = Prefab.EnsureComponent<ModuleFirstPerson>();

			TankBlock tankBlock = Prefab.EnsureComponent<TankBlock>();

			tankBlock.m_BlockCategory = Category;
			List<Vector3> points = new List<Vector3>();

			for (int x = 0; x < 2; x++)
			{
				for (int z = 0; z < 2; z++)
				{
					points.Add(new Vector3(x, -0.5f, z));
				}
			}
			tankBlock.attachPoints = points.ToArray();

			List<Vector3> cells = new List<Vector3>();
			for (int x = 0; x < 2; x++)
			{
				for (int y = 0; y < 2; y++)
				{
					for (int z = 0; z < 2; z++)
					{
						cells.Add(new Vector3(x, y, z));
					}
				}
			}
			tankBlock.filledCells = cells.ToArray();
			tankBlock.partialCells = new Vector3[] { };

			GameObject renderObject = GameObject.Instantiate(AssetBundleImport.Load<GameObject>(ModelFile));
			renderObject.transform.parent = Prefab.transform;
			renderObject.name = $"{Name}.Model";
			renderObject.layer = Globals.inst.layerTank;
			var collider = renderObject.EnsureComponent<BoxCollider>();
			collider.size = new Vector3(2, 2, 2);
			collider.center = Vector3.one / 2;
		}
	}
}