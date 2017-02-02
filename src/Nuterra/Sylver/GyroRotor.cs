using Nuterra;
using Maritaria;
using System;

using UnityEngine;



namespace Sylver

{

	public sealed class GyroRotor : CustomBlock

	{
	
		public static readonly string SpriteFile = @"Assets/Blocks/Bacon/bacon_icon.png";

		public static readonly int BlockID = 8000;



		int CustomBlock.BlockID => BlockID;

		public string Name => "Hawkeye Gyro Fan Rotor";

		public string Description => "An improved version of the standard Hawkeye Rotor Fan which now have an integrated Gyro-Stabilizer";

		public FactionSubTypes Faction => FactionSubTypes.HE;

		public BlockCategories Category => BlockCategories.Flight;

		public GameObject Prefab { get; }

		public Sprite DisplaySprite { get; } = AssetBundleImport.Load<Sprite>(SpriteFile);



		public GyroRotor()

		{
			Prefab = UnityEngine.GameObject.Instantiate(Singleton.Manager<ManSpawn>.inst.m_BlockPrefabs[414]);
			
            Prefab.SetActive(false);
			
			GameObject.DontDestroyOnLoad(Prefab);

			Prefab.name = Name;

			Prefab.tag = "TankBlock";

			Prefab.layer = Globals.inst.layerTank;



			Visible vis = Prefab.EnsureComponent<Visible>();
			
			SylverGyro gyro = Prefab.EnsureComponent<SylverGyro>();

			vis.m_ItemType = new ItemTypeInfo(ObjectTypes.Block, BlockID);
		}

	}

}	