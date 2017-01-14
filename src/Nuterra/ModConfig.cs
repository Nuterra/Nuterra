using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Nuterra
{
	public static class ModConfig
	{
		public static JObject Data { get; }

		static ModConfig()
		{
			string configData = File.ReadAllText(Path.Combine(Nuterra.DataFolder, "nuterra.json"));
			Console.WriteLine($"configData: {configData}");
			Data = new JObject(configData);
		}
	}
}