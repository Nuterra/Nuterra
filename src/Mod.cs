using System;
using UnityEngine;

namespace Maritaria
{
	public static class Mod
	{
		public static ModConfig Config;
		public static GameObject BehaviorHolder;
		
		public static void Init()
		{
			Console.WriteLine("Maritaria.Mod.Init();");
			
			Config = new ModConfig();
			Config.Load();
			
			BehaviorHolder = new GameObject();
			BehaviorHolder.AddComponent<MagnetToggleKeyBehaviour>();
			BehaviorHolder.AddComponent<RenameTechKeyBehaviour>();
			UnityEngine.Object.DontDestroyOnLoad(Mod.BehaviorHolder);
			
			SplashScreenHandler.Init();
		}

		public static void LogGameObject(GameObject inst)
		{
			if (inst == null)
			{
				return;
			}
			Visible component = inst.GetComponent<Visible>();
			if (component != null && component.m_ItemType.ObjectType == ObjectTypes.Block && component.m_ItemType.ItemType == 3)
			{
				Mod.LogGameObject(inst, "");
				Transform expr_4D = inst.transform.GetChild(0);
				expr_4D.localPosition=(expr_4D.localPosition + new Vector3(1f, 0f, 0f));
			}
		}

		public static void LogGameObject(GameObject inst, string prefix)
		{
			if (inst == null)
			{
				return;
			}
			Console.WriteLine(string.Concat(new string[]
			{
				prefix,
				"GameObject: ",
				inst.name,
				" ",
				inst.transform.localPosition.ToString()
			}));
			prefix += "\t";
			Component[] components = inst.GetComponents<Component>();
			for (int i = 0; i < components.Length; i++)
			{
				Mod.LogComponent(components[i], prefix);
			}
			for (int j = 0; j < inst.transform.childCount; j++)
			{
				Mod.LogGameObject(inst.transform.GetChild(j).gameObject, prefix);
			}
		}

		public static void LogComponent(Component inst, string prefix)
		{
			if (inst == null)
			{
				return;
			}
			Console.Write(prefix + "Component: " + inst.GetType().FullName + " ");
			if (inst is TankBlock)
			{
				Mod.LogComponent_TankBlock((TankBlock)inst);
				return;
			}
			if (inst is Visible)
			{
				Mod.LogComponent_Visible((Visible)inst);
				return;
			}
			if (inst is MeshFilter)
			{
				Mod.LogComponent_MeshFilter((MeshFilter)inst);
				return;
			}
			Console.WriteLine();
		}

		public static void LogComponent_TankBlock(TankBlock tankBlock)
		{
			Console.WriteLine(tankBlock.BlockCellBounds);
		}

		public static void LogComponent_Visible(Visible visible)
		{
			Console.WriteLine(visible.Flags);
		}

		public static void LogComponent_MeshFilter(MeshFilter mesh)
		{
			Console.WriteLine(mesh.mesh.name);
		}
	}
}
