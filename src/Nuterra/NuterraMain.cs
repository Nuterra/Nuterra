using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Nuterra
{
	public static class NuterraMain
	{
		public static readonly Version CurrentVersion = new Version(0, 3, 0);
		public static readonly string DataFolder = Path.Combine(Application.dataPath, "..\\Nuterra_Data");
		public static readonly string ModsFolder = Path.Combine(Application.dataPath, "..\\Nuterra_Data\\Mods");//TODO: Start using this
		public static readonly string TerraTechAssemblyName = "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";

		internal static void Start()
		{
			CleanLogger.Install();
			Console.WriteLine($"Nuterra.Nuterra.Startup({CurrentVersion})");

			if (!Directory.Exists(DataFolder))
			{
				Console.WriteLine("Nuterra_Data is missing! mods won't be loaded");
				return;
			}

			Assembly terraTech = FindTerraTechAssembly();
			if (terraTech != null)
			{
				ModLoader.Instance.LoadAllMods(terraTech);
			}
		}

		private static Assembly FindTerraTechAssembly()
		{
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (asm.FullName.Equals(TerraTechAssemblyName))
				{
					return asm;
				}
			}
			Console.WriteLine($"Unable to find '{TerraTechAssemblyName}'");
			return null;
		}
	}
}