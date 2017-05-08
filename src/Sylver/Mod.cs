using System;
using Nuterra;
using UnityEngine;

namespace Sylver
{
	[Mod]
	public class Mod : TerraTechMod
	{
		public static GameObject BehaviorHolder;

		public override string Name => "SylverMod";

		public override string Description => "A mod made by Sylver/Exund";

		public override void Load()
		{
			base.Load();
			Console.WriteLine("Sylver.Mod.Init()");
			Mod.BehaviorHolder = new GameObject();
			Mod.BehaviorHolder.AddComponent<SylverMod>();
			Mod.BehaviorHolder.AddComponent<GUIRenderer>();
			Mod.BehaviorHolder.AddComponent<SylverSpawn>();
			Mod.BehaviorHolder.AddComponent<SylverGyro>();
			UnityEngine.Object.DontDestroyOnLoad(Mod.BehaviorHolder);

			new BlockPrefabBuilder(BlockTypes.HE_Fan_16_111)
				.SetBlockID(9100)
				.SetName("Hawkeye Gyro Fan Rotor")
				.SetDescription("An improved version of the standard Hawkeye Rotor Fan which now have an integrated Gyro-Stabilizer")
				.SetFaction(FactionSubTypes.HE)
				.SetCategory(BlockCategories.Flight)
				.AddComponent<SylverGyro>()
				.Register();
		}
	}
}