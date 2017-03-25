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
		}
	}
}