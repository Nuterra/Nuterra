using System;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public sealed class SmileyBlock : CustomBlock
	{
		public static readonly string SpriteFile = Path.Combine(Mod.DataDirectory, "face_block\\face_block.png");
		public static readonly string ModelFile = Path.Combine(Mod.DataDirectory, "face_block\\face_block.obj");
		
		public static readonly int BlockID = 9000;
		
		int CustomBlock.BlockID => BlockID;
		public string Name => "SmileyBlock";
		public string Description => "A block that provides a POV camera, somehow";
		public FactionSubTypes Faction => FactionSubTypes.GSO;
		public BlockCategories Category => BlockCategories.Accessories;
		public GameObject Prefab { get; }
		public Sprite DisplaySprite { get; } = new SpriteFactory().CreateSprite(SpriteFile);
		
		public SmileyBlock()
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
			tankBlock.attachPoints = new Vector3[]{
				new Vector3(0, 0, 0.5f), //Top
				new Vector3(0, 0, -0.5f), //Bottom
				//new Vector3(0, 0.5f, 0), //Front
				new Vector3(0, -0.5f, 0), //Back
				new Vector3(0.5f, 0, 0), //Left
				new Vector3(-0.5f, 0, 0), //Right
			};
			tankBlock.filledCells = new Vector3[]{ new Vector3(0, 0, 0) };
			tankBlock.partialCells = new Vector3[]{ };
				
			GameObject renderObject = new GameObject();
			renderObject.transform.parent = obj.transform;
			renderObject.name = $"{Name}.Model";
			renderObject.layer = Globals.inst.layerTank;
			
			MeshRenderer renderer = renderObject.EnsureComponent<MeshRenderer>();
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			
			MeshFilter filter = renderObject.EnsureComponent<MeshFilter>();
			filter.mesh = new ObjImporter().ImportFile(ModelFile);
			
			Vector3 bounds = filter.mesh.bounds.size;
			float largestSide = Mathf.Max(bounds.x, bounds.y, bounds.z, 1f);
			float scale = 1f / largestSide;
			renderObject.transform.localScale = new Vector3(scale, scale, scale);
			
			BoxCollider collider = renderObject.EnsureComponent<BoxCollider>();
			collider.center = Vector3.zero;
			collider.size = new Vector3(largestSide,largestSide,largestSide);
			
			// Create a new 1x1 texture ARGB32 (32 bit with alpha) and no mipmaps
			Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

			// set the pixel values
			texture.SetPixel(0, 0, Color.white);
			//texture.SetPixel(1, 0, Color.white);
			//texture.SetPixel(0, 1, Color.white);
			//texture.SetPixel(1, 1, Color.white);

			// Apply all SetPixel calls
			texture.Apply();

			// connect texture to material of GameObject this script is attached to
			renderer.material.mainTexture = texture;
			
			Prefab = obj;
		}
	}
}