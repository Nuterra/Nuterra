using System;
using System.IO;
using Nuterra.Internal;
using UnityEngine;

namespace Nuterra
{
	public static class NuterraApi
	{
		public static readonly Version CurrentVersion = new Version(0, 4, 2, 1);

		internal static ConfigManager Configuration { get; } = new ConfigManager(FolderStructure.ConfigFolder);

		internal static void Start()
		{
			CleanLogger.Install();
			BugReportFlagger.Init();
			Console.WriteLine($"Nuterra.NuterraApi.Start({CurrentVersion})");

			if (!Directory.Exists(FolderStructure.DataFolder))
			{
				Console.WriteLine("Nuterra data folder is missing! Mods won't be loaded.");
				return;
			}

			Directory.CreateDirectory(FolderStructure.ModsFolder);
			Directory.CreateDirectory(FolderStructure.ConfigFolder);

			ModLoader.Instance.LoadAllMods(FolderStructure.ModsFolder);
			BlockLoader.PostModsLoaded();
		}
	}
}