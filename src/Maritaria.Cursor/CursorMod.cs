using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Nuterra;
using UnityEngine;

namespace Maritaria.Cursor
{
	[Mod]
	public sealed class CursorMod : TerraTechMod
	{
		public static readonly string HotspotFileName = "hotspots.json";

		private CursorManager _manager;

		public override string Name => nameof(CursorMod);
		public override string Description => "Allows the cursor to be changed using a png image";

		public override void Load()
		{
			base.Load();

			if (Config == null)
			{
				LogError($"No configuration section");
				return;
			}
			if (_manager == null)
			{
				MousePointer pointer = FindMousePointer();
				if (pointer == null)
				{
					LogError($"Unable to find MousePointer instance");
					return;
				}
				_manager = CursorManager.Install(pointer);
			}
			OverrideCursors();
		}

		private static MousePointer FindMousePointer()
		{
			var objects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
			return objects.SelectMany(o => o.GetComponentsInChildren<MousePointer>()).SingleOrDefault();
		}

		private void OverrideCursors()
		{
			string cursorFolder = Config.GetValue("Folder", StringComparison.OrdinalIgnoreCase)?.ToObject<string>();
			if (cursorFolder == null)
			{
				LogWarning($"No folder specified, mod is disabled");
				return;
			}
			if (!Directory.Exists(cursorFolder))
			{
				LogError($"Missing cursor directory '{cursorFolder}'");
				return;
			}
			string hotspotConfigFile = Path.Combine(cursorFolder, HotspotFileName);
			if (!File.Exists(hotspotConfigFile))
			{
				LogError($"Missing hotspots file '{hotspotConfigFile}'");
				return;
			}

			string json = File.ReadAllText(hotspotConfigFile);
			JObject hotspotConfig = JObject.Parse(json);

			LoadCursorOverride(_manager, CursorType.Default, cursorFolder, hotspotConfig);
			LoadCursorOverride(_manager, CursorType.Hover, cursorFolder, hotspotConfig);
			LoadCursorOverride(_manager, CursorType.Pressed, cursorFolder, hotspotConfig);
			LoadCursorOverride(_manager, CursorType.Painting, cursorFolder, hotspotConfig);
		}

		private void LoadCursorOverride(CursorManager target, CursorType cursorType, string cursorDir, JObject hotspots)
		{
			string cursorTypeName = cursorType.ToString();
			string filename = $"{cursorTypeName.ToLower()}.png";
			string filepath = Path.Combine(cursorDir, filename);
			if (!File.Exists(filepath))
			{
				LogError($"Missing file '{filepath}' for {cursorTypeName} cursor");
				return;
			}
			var hotspot = hotspots.GetValue(cursorTypeName, StringComparison.OrdinalIgnoreCase)?.ToObject<Vector2>();
			if (!hotspot.HasValue)
			{
				LogWarning($"Missing hotspot for {cursorTypeName} cursor");
			}
			byte[] bytes = File.ReadAllBytes(filepath);
			Texture2D texture = new Texture2D(1, 1);
			texture.LoadImage(bytes, false);
			target.Cursors[cursorType] = new MousePointer.CursorData { m_Texture = texture, m_Hotspot = hotspot.Value };
		}

		public override void Unload()
		{
			base.Unload();
			if (_manager != null)
			{
				_manager.RestoreFromOriginalMousePointer();
			}
		}

		private void Log(string message)
		{
			Console.WriteLine($"[CustomCursor] {message}");
		}

		private void LogWarning(string message)
		{
			Log($"Warning: {message}");
		}

		private void LogError(string message)
		{
			Log($"Error: {message}");
		}
	}
}