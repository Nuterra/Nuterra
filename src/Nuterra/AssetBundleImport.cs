using System;
using System.IO;
using UnityEngine;

namespace Nuterra
{
	public static class AssetBundleImport
	{
		public static readonly string AssetFilename = "nuterra-mod";
		public static readonly AssetBundle NuterraAssetBundle;

		static AssetBundleImport()
		{
			NuterraAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Nuterra.DataFolder, AssetFilename));
			if (NuterraAssetBundle == null)
			{
				Debug.Log($"Failed to load {AssetFilename} AssetBundle, errors are coming");
			}
		}

		public static T Load<T>(string path) where T : UnityEngine.Object
		{
			if (NuterraAssetBundle == null)
			{
				throw new InvalidOperationException("AssetBundle is not loaded");
			}
			return NuterraAssetBundle.LoadAsset<T>(path);
		}
	}
}