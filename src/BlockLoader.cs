using System;
using UnityEngine;

namespace Maritaria
{
	public static class BlockLoader
	{
		public static readonly int StarBlockID = (int)BlockTypes.GSOStarBlock;//Add this to the enum
		public static readonly FactionSubTypes StarBlockFaction = FactionSubTypes.GSO;
		public static readonly BlockCategories StarBlockCategory = BlockCategories.Accessories;
		public static GameObject StarPrefab;
		//Hook to be called at the end of ManSpawn.Start
		public static void Init()
		{
			StarPrefab = InitStarPrefab();
			ManSpawn spawnManager = Singleton.Manager<ManSpawn>.inst;
			spawnManager.AddBlockToDictionary(StarPrefab);
			//Because of ManSpawn.CacheCorporationBlocks()
			//TODO: Override ManSpawn.GetBlockCorperation(BlockTypes) to pass through here
			int hashCode = ItemTypeInfo.GetHashCode(ObjectTypes.Block, StarBlockID);
			Console.WriteLine($"BlockCategoryAfter registration {spawnManager.VisibleTypeInfo.GetDescriptor<FactionSubTypes>(hashCode)}");
			spawnManager.VisibleTypeInfo.SetDescriptor<FactionSubTypes>(hashCode, StarBlockFaction);
			spawnManager.VisibleTypeInfo.SetDescriptor<BlockCategories>(hashCode, StarBlockCategory);			
		}
		//Hook to be called at the beginning of BlockUnlockTable.Init()
		public static void BlockUnlockTable_Init(BlockUnlockTable unlockTable)
		{
			BlockUnlockTable.CorpBlockData[] blockList = unlockTable.m_CorpBlockList;
			
			BlockUnlockTable.CorpBlockData gso = blockList[(int)StarBlockFaction];
			BlockUnlockTable.UnlockData[] unlocked = gso.m_GradeList[0].m_BlockList;
			
			Array.Resize(ref unlocked, unlocked.Length + 1);
			unlocked[unlocked.Length - 1] = new BlockUnlockTable.UnlockData {
				m_BlockType = (BlockTypes)StarBlockID,
				m_BasicBlock = true,
				m_DontRewardOnLevelUp = true,
			};
			
			gso.m_GradeList[0].m_BlockList = unlocked;
		}
		//StarBlock category: Null
		
		//TODO: Remove by references of StarBlockID
		//TestCode in ManTechBuilder.UpdateAttachParticles()
		
		public static void ManLicenses_SetupLicenses(ManLicenses licenses)
		{
			ManLicenses.BlockState state = licenses.GetBlockState((BlockTypes)StarBlockID);
			Console.WriteLine($"StarBlock: {state}");
			licenses.DiscoverBlock((BlockTypes)StarBlockID);
		}
		
		public static GameObject InitStarPrefab()
		{
			GameObject obj = new GameObject();
			GameObject.DontDestroyOnLoad(obj);
			obj.name = "GSOStarBlock(111)";
			obj.tag = "TankBlock";
			obj.layer = Globals.inst.layerTank;//8
			
			Visible vis = obj.EnsureComponent<Visible>();
			vis.m_ItemType = new ItemTypeInfo(ObjectTypes.Block, StarBlockID);
			
			Damageable dmg = obj.EnsureComponent<Damageable>();
			ModuleDamage modDamage = obj.EnsureComponent<ModuleDamage>();
			AutoSpriteRenderer spriteRenderer = obj.EnsureComponent<AutoSpriteRenderer>();
			obj.EnsureComponent<TankBlock>();
			TankBlock tankBlock = obj.GetComponent<TankBlock>();
			Console.WriteLine($"TB: {tankBlock}");
			tankBlock.m_BlockCategory = StarBlockCategory;
			tankBlock.attachPoints = new Vector3[]{ new Vector3(0, -0.5f, 0) };
			tankBlock.filledCells = new Vector3[]{ new Vector3(0, 0, 0) };
			tankBlock.partialCells = new Vector3[]{ };
				
			GameObject renderObject = new GameObject();
			renderObject.transform.parent = obj.transform;
			renderObject.name = "GSO_StarBlock_111";
			renderObject.layer = Globals.inst.layerTank;//8
			
			MeshRenderer renderer = renderObject.EnsureComponent<MeshRenderer>();
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			
			MeshFilter filter = renderObject.EnsureComponent<MeshFilter>();
			filter.mesh = new ObjImporter().ImportFile(@"D:\Personal\Documents\Visual Studio 2015\Projects\terratech-mod\data\my_first_block.obj");
			
			Vector3 bounds = filter.mesh.bounds.size;
			float largestSide = Mathf.Max(bounds.x, bounds.y, bounds.z, 1f);
			float scale = 1f / largestSide;
			renderObject.transform.localScale = new Vector3(scale, scale, scale);
			
			BoxCollider collider = renderObject.EnsureComponent<BoxCollider>();
			collider.center = Vector3.zero;
			collider.size = new Vector3(largestSide,largestSide,largestSide);
			
			// Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
			Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);

			// set the pixel values
			texture.SetPixel(0, 0, Color.red);
			texture.SetPixel(1, 0, Color.blue);
			texture.SetPixel(0, 1, Color.yellow);
			texture.SetPixel(1, 1, Color.green);

			// Apply all SetPixel calls
			texture.Apply();

			// connect texture to material of GameObject this script is attached to
			renderer.material.mainTexture = texture;
			
			return obj;
		}
	}
}