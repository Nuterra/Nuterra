using System;
using UnityEngine;

namespace Sylver
{
	public static class Mod
	{
		public static void Init()
		{
			Console.WriteLine("Sylver.Mod.Init()");
			Mod.BehaviorHolder = new GameObject();
			Mod.BehaviorHolder.AddComponent<SylverMod>();
			Mod.BehaviorHolder.AddComponent<GUIRenderer>();
			Mod.BehaviorHolder.AddComponent<SylverSpawn>();
			UnityEngine.Object.DontDestroyOnLoad(Mod.BehaviorHolder);
		}

		static Mod()
		{
		}

		public static GameObject BehaviorHolder;
	}
}
