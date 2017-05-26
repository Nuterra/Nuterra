using System;
using System.Collections.Generic;
using System.IO;
using Nuterra.Editor;
using Nuterra.Internal;
using UnityEngine;

namespace Nuterra.BlockPacks
{
	public sealed class BlockPackMod : TerraTechMod
	{
		private const string ManifestExtension = ".manifest";
		private Dictionary<string, AssetBundle> _bundles = new Dictionary<string, AssetBundle>();

		public override string Name => nameof(BlockPackMod);
		public override string Description => "Loads assetbundles and adds the blocks contained within to the game";

		public override void Load()
		{
			base.Load();

			string packsFolder = Path.Combine(FolderStructure.DataFolder, "BlockPacks");

			if (Directory.Exists(packsFolder))
			{
				LoadPacks(packsFolder);
			}
			else
			{
				Directory.CreateDirectory(packsFolder);
			}
		}

		private void LoadPacks(string packsFolder)
		{
			foreach (string manifestFile in Directory.GetFiles(packsFolder, $"*" + ManifestExtension))
			{
				LoadSinglePack(manifestFile);
			}
		}

		private void LoadSinglePack(string manifestFile)
		{
			string bundlePath = manifestFile.Substring(0, manifestFile.Length - ManifestExtension.Length);
			var bundle = AssetBundle.LoadFromFile(bundlePath);
			if (bundle == null)
			{
				return;
			}
			_bundles.Add(bundlePath, bundle);

			foreach (string assetPath in bundle.GetAllAssetNames())
			{
				// Assets/Blocks/*.prefab
				if (!assetPath.StartsWith("assets/blocks/", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				if (!assetPath.EndsWith(".prefab"))
				{
					continue;
				}
				LoadBlock(bundle, assetPath);
			}
		}

		private static Dictionary<string, GameObject> _loadedblocks = new Dictionary<string, GameObject>();

		private void LoadBlock(AssetBundle bundle, string assetPath)
		{
			var prefab = bundle.LoadAsset<GameObject>(assetPath);
			if (prefab.GetComponent<CustomBlockPrefab>() != null)
			{
				_loadedblocks.Add(assetPath, prefab);
				var prefabInfo = prefab.GetComponent<CustomBlockPrefab>();
				new BlockPrefabBuilder()
					.SetBlockID(prefabInfo.BlockID)
					.SetName(prefabInfo.Name)
					.SetDescription(prefabInfo.Description)
					.SetPrice(prefabInfo.Price)
					.SetFaction(FactionSubTypes.GSO)
					.SetCategory(BlockCategories.Accessories)
					.SetSize(new Vector3I(prefabInfo.Dimensions), BlockPrefabBuilder.AttachmentPoints.Bottom)
					.SetModel(prefab)
					.SetIcon(prefabInfo.DisplaySprite)
					.Register();
			}
		}
	}
}