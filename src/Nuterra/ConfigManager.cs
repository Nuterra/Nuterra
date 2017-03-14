using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nuterra.Internal;

namespace Nuterra
{
	internal sealed class ConfigManager
	{
		private string _folder;
		public string Folder => Path.Combine(FolderStructure.DataFolder, _folder);
		public string FileExtension => ".json";

		internal ConfigManager(string folder)
		{
			_folder = folder;
		}

		public JObject LoadModConfig(TerraTechMod mod)
		{
			if (mod == null) throw new ArgumentNullException(nameof(mod));
			string path = GetModConfigFilePath(mod);
			if (File.Exists(path))
			{
				string json = File.ReadAllText(path);
				return JObject.Parse(json);
			}
			else
			{
				JObject fallback = mod.CreateDefaultConfiguration();
				File.WriteAllText(path, fallback.ToString(Formatting.Indented));
				return fallback;
			}
		}

		private string GetModConfigFilePath(TerraTechMod mod)
		{
			string configName = mod.Name;
			if (configName == null) throw new ArgumentException("TerraTechMod.Name is null", nameof(mod));
			configName = configName.ToLower();
			string path = Path.Combine(Folder, configName) + FileExtension;
			return path;
		}
	}
}