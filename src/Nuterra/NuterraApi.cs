using System;
using System.IO;
using UnityEngine;

namespace Nuterra
{
	public static class NuterraApi
	{
		public static readonly Version CurrentVersion = new Version(0, 4, 0);
		public static readonly string RootFolder = Path.Combine(Application.dataPath, "..");
		public static readonly string DataFolder = Path.Combine(RootFolder, "Nuterra_Data");
		public static readonly string ModsFolder = Path.Combine(DataFolder, "Mods");
		public static readonly string ConfigFolder = Path.Combine(DataFolder, "Config");

		internal static ConfigManager Configuration { get; } = new ConfigManager(ConfigFolder);

		internal static void Start()
		{
			CleanLogger.Install();
			BugReportFlagger.Init();
			Console.WriteLine($"Nuterra.NuterraApi.Start({CurrentVersion})");

			if (!Directory.Exists(DataFolder))
			{
				Console.WriteLine("Nuterra data folder is missing! Mods won't be loaded.");
				return;
			}

			Directory.CreateDirectory(ModsFolder);
			Directory.CreateDirectory(ConfigFolder);

			Console.WriteLine("Check: " + ManSpawn.inst.m_BlockPrefabs);
			ModLoader.Instance.LoadAllMods(ModsFolder);
			BlockLoader.PostModsLoaded();
		}
	}
}