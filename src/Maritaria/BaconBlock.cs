using System;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public sealed class BaconBlock : CustomBlock
	{
		public static readonly string SpriteFile = Path.Combine(Mod.DataDirectory, "bacon_block\\icon.png");
		public static readonly string ModelFile = Path.Combine(Mod.DataDirectory, "bacon_block\\model.ply");
		
		public static readonly int BlockID = 9001;
		
		int CustomBlock.BlockID => BlockID;
		public string Name => "BaconBlock";
		public string Description => "A long strip of bacon with bullet absoring grease";
		public FactionSubTypes Faction => FactionSubTypes.GSO;
		public BlockCategories Category => BlockCategories.Accessories;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = new SpriteFactory().CreateSprite(SpriteFile);
		
		public BaconBlock()
		{
			GameObject obj = new GameObject();
			GameObject.DontDestroyOnLoad(obj);
			obj.name = Name;
			obj.tag = "TankBlock";
			obj.layer = Globals.inst.layerTank;
			
			Visible vis = obj.EnsureComponent<Visible>();
			vis.m_ItemType = new ItemTypeInfo(ObjectTypes.Block, BlockID);
			
			Damageable dmg = obj.EnsureComponent<Damageable>();
			ModuleDamage modDamage = obj.EnsureComponent<ModuleDamage>();
			AutoSpriteRenderer spriteRenderer = obj.EnsureComponent<AutoSpriteRenderer>();
			obj.EnsureComponent<TankBlock>();
			TankBlock tankBlock = obj.GetComponent<TankBlock>();
			
			tankBlock.m_BlockCategory = Category;
			
			//z is forward
			
			
			//Bacon layout:
			// 0 1 2 3
			// 4 5 6 7
			// Origin is centered on bacon center
						
			tankBlock.attachPoints = new Vector3[]{
				new Vector3(0f, 0.5f, 0f), //0
				new Vector3(1f, 0.5f, 0f), //1
				new Vector3(2f, 0.5f, 0f), //2
				new Vector3(3f, 0.5f, 0f), //3
				new Vector3(0f, 0.5f, 1f), //4
				new Vector3(1f, 0.5f, 1f), //5
				new Vector3(2f, 0.5f, 1f), //6
				new Vector3(3f, 0.5f, 1f), //7
			};
			tankBlock.filledCells = new Vector3[]{
				new Vector3(0, 0, 0),
				new Vector3(1, 0, 0),
				new Vector3(2, 0, 0),
				new Vector3(3, 0, 0),
				new Vector3(0, 0, 1),
				new Vector3(1, 0, 1),
				new Vector3(2, 0, 1),
				new Vector3(3, 0, 1),
			};
			tankBlock.partialCells = new Vector3[]{ };
				
			GameObject renderObject = new GameObject();
			renderObject.transform.parent = obj.transform;
			renderObject.name = $"{Name}.Model";
			renderObject.layer = Globals.inst.layerTank;
			
			MeshRenderer renderer = renderObject.EnsureComponent<MeshRenderer>();
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			
			MeshFilter filter = renderObject.EnsureComponent<MeshFilter>();
			
			AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "mod-maritaria-bundle"));
			if (myLoadedAssetBundle == null)
			{
				Debug.Log("Failed to load AssetBundle!");
				throw new Exception("failed to load asset bundle");
				return;
			}
			else
			{
			}

			filter.mesh = myLoadedAssetBundle.LoadAsset<Mesh>("Assets/Models/bacon_block/bacon_model.blend");
			
			Vector3 targetSize = new Vector3(4, 0.5f, 2);
			Vector3 modelOffset = new Vector3(0, 0.25f, 0.5f);
			Vector3 targetModelCenter = (targetSize / 2) - modelOffset;
			
			renderObject.transform.localPosition = targetModelCenter;
			Vector3 originalSize = filter.mesh.bounds.size;
			renderObject.transform.localScale = new Vector3(targetSize.x / originalSize.x, targetSize.y / originalSize.y, targetSize.z / originalSize.z);
			
			BoxCollider collider = renderObject.EnsureComponent<BoxCollider>();
			collider.center = filter.mesh.bounds.center;
			collider.size = filter.mesh.bounds.size;
			
			renderer.material = myLoadedAssetBundle.LoadAsset<Material>("Assets/Models/bacon_block/Materials/bacon_material.mat");
			
			Prefab = obj;
		}
	}
}