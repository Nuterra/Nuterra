﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public static class Mod
	{
		public static readonly Version CurrentVersion = new Version(0, 2, 4);
		public static readonly string DataDirectory = "maritaria";
		public static ModConfig Config;
		public static GameObject BehaviorHolder;
		
		//Hook to be called at the beginning of ManUI.Start
		public static void Init()
		{
			Console.WriteLine($"Maritaria.Mod.Init({CurrentVersion})");
			CleanLogger.Install();
			
			EnsureDirectoryStructure();
			
			Config = new ModConfig();
			Config.Load();
			
			BehaviorHolder = new GameObject();
			BehaviorHolder.AddComponent<MagnetToggleKeyBehaviour>();
			BehaviorHolder.AddComponent<TimeOfDayKeysBehaviour>();
			BehaviorHolder.AddComponent<FirstPersonKeyBehaviour>();
			BehaviorHolder.AddComponent<ProductionToggleKeyBehaviour>();
			BehaviorHolder.AddComponent<Gameslynx.SuicideKeyBehaviour>();
			UnityEngine.Object.DontDestroyOnLoad(Mod.BehaviorHolder);
			
			SplashScreenHandler.Init();
		}
		
		private static void EnsureDirectoryStructure()
		{
			if (!Directory.Exists(DataDirectory))
			{
				Directory.CreateDirectory(DataDirectory);
			}
		}

		public static void LogGameObject(GameObject inst)
		{
			if (inst == null)
			{
				return;
			}
			LogGameObject(inst, string.Empty);
		}

		public static void LogGameObject(GameObject inst, string prefix)
		{
			if (inst == null)
			{
				return;
			}
			Console.WriteLine($"{prefix}GameObject name: ({inst.name}) pos: {inst.transform.localPosition}");
			prefix += "\t";
			Console.WriteLine($"{prefix}activeSelf: {inst.activeSelf} activeInHierarchy: {inst.activeInHierarchy}");
			Console.WriteLine($"{prefix}layer: ({inst.layer}) tag: ({inst.tag})");
			Console.WriteLine($"{prefix}Components:");
			Component[] components = inst.GetComponents<Component>();
			for (int i = 0; i < components.Length; i++)
			{
				Component component = components[i];
				Mod.LogComponent(component, prefix);
			}
			Console.WriteLine($"{prefix}GameObjects:");
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
			Console.Write($"{prefix}{inst.GetType().FullName} ");
			if (inst is TankBlock)
			{
				Mod.LogComponent_TankBlock((TankBlock)inst, prefix);
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
			if (inst is BoxCollider)
			{
				Mod.LogComponent_BoxCollider((BoxCollider)inst);
				return;
			}
			Console.WriteLine();
		}

		public static void LogComponent_TankBlock(TankBlock tankBlock, string prefix)
		{
			Console.WriteLine();
			Console.WriteLine($"{prefix}\tattachPoints:");
			foreach(Vector3 vec in tankBlock.attachPoints)
			{
				Console.WriteLine($"{prefix}\t\t- {vec}");
			}
			Console.WriteLine($"{prefix}\tfilledCells:");
			foreach(Vector3 vec in tankBlock.filledCells)
			{
				Console.WriteLine($"{prefix}\t\t- {vec}");
			}
			Console.WriteLine($"{prefix}\tpartialCells:");
			foreach(Vector3 vec in tankBlock.partialCells)
			{
				Console.WriteLine($"{prefix}\t\t- {vec}");
			}
			Console.WriteLine($"{prefix}\tapGroups:");
			foreach(TankBlock.APGroup group in tankBlock.apGroups)
			{
				Console.WriteLine($"{prefix}\t\t- Count: {group.Count}");
				foreach(int i in group.Enumerate())
				{
					Console.WriteLine($"{prefix}\t\t\t{i}");
				}
			}
		}

		public static void LogComponent_Visible(Visible visible)
		{
			Console.WriteLine(visible.Flags);
		}

		public static void LogComponent_MeshFilter(MeshFilter mesh)
		{
			Console.WriteLine(mesh.mesh.name);
		}

		public static void LogComponent_BoxCollider(BoxCollider box)
		{
			Console.WriteLine($"{box.center} {box.size}");
		}
		
		public static void LogPrivates(object obj, string prefix)
		{
			return;
			foreach(FieldInfo prop in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object value = prop.GetValue(obj);
				if (value!=null)
				{
					if (value.GetType().IsArray || typeof(ICollection).IsAssignableFrom(value.GetType()))
					{
						value = $"{((IEnumerable)value).Cast<object>().Count()} ({value.GetType()})";
					}
				}
				Console.WriteLine($"{prefix}{prop.Name} = {value}");
			}
			foreach(PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				object value;
				try
				{
					value = prop.GetValue(obj, null);
				}
				catch(Exception ex)
				{
					value = ex;
				}
				Console.WriteLine($"{prefix}{prop.Name} = {value}");
			}
		}
	}
}