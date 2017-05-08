using System;
using System.Collections.Generic;
using System.IO;
using Nuterra.Internal;
using UnityEngine;

namespace Nuterra
{
	public static class AssetBundleImport
	{
		public static readonly string AssetFilename = "mod-nuterra";
		public static readonly AssetBundle NuterraAssetBundle;
		private static Dictionary<string, object> CachedAssets = new Dictionary<string, object>();

		static AssetBundleImport()
		{
			NuterraAssetBundle = AssetBundle.LoadFromFile(Path.Combine(FolderStructure.AssetsFolder, AssetFilename));
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
			object result;
			if (!CachedAssets.TryGetValue(path, out result))
			{
				result = NuterraAssetBundle.LoadAsset<T>(path);
				CachedAssets.Add(path, result);
			}
			return (T)result;
		}
	}
}