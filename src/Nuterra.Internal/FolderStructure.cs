using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Nuterra.Internal
{
	public static class FolderStructure
	{
		public static readonly string RootFolder = Path.Combine(Application.dataPath, "..");
		public static readonly string DataFolder = Path.Combine(RootFolder, "Nuterra_Data");
		public static readonly string ModsFolder = Path.Combine(DataFolder, "Mods");
		public static readonly string ConfigFolder = Path.Combine(DataFolder, "Config");
		public static readonly string AssetsFolder = Path.Combine(DataFolder, "Assets");
	}
}
