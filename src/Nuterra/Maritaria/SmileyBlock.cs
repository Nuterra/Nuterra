using Nuterra;
using System;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public sealed class SmileyBlock : CustomBlock
	{
		public static readonly string SpriteFile = @"Assets/Blocks/Cockpit/cockpit_icon.png";
		public static readonly string ModelFile = @"Assets/Blocks/Cockpit/cockpit.blend";
		public static readonly string MaterialFile = @"Assets/Blocks/Cockpit/cockpit_material.mat";

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
			tankBlock.attachPoints = new Vector3[]{//TODO: bottom only
				new Vector3(0, 0, 0.5f), //Top
				new Vector3(0, 0, -0.5f), //Bottom
				//new Vector3(0, 0.5f, 0), //Front
				new Vector3(0, -0.5f, 0), //Back
				new Vector3(0.5f, 0, 0), //Left
				new Vector3(-0.5f, 0, 0), //Right
			};
			tankBlock.filledCells = new Vector3[] { new Vector3(0, 0, 0) };
			tankBlock.partialCells = new Vector3[] { };

			GameObject renderObject = new GameObject();
			renderObject.transform.parent = Prefab.transform;
			renderObject.name = $"{Name}.Model";
			renderObject.layer = Globals.inst.layerTank;

			MeshRenderer renderer = renderObject.EnsureComponent<MeshRenderer>();
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

			MeshFilter filter = renderObject.EnsureComponent<MeshFilter>();
			filter.mesh = AssetBundleImport.Load<Mesh>(ModelFile);

			Vector3 bounds = filter.mesh.bounds.size;
			float largestSide = Mathf.Max(bounds.x, bounds.y, bounds.z, 1f);
			float scale = 1f / largestSide;
			renderObject.transform.localScale = new Vector3(scale, scale, scale);

			BoxCollider collider = renderObject.EnsureComponent<BoxCollider>();
			collider.center = Vector3.zero;
			collider.size = new Vector3(largestSide, largestSide, largestSide);

			renderer.material = AssetBundleImport.Load<Material>(MaterialFile);
		}
	}
}