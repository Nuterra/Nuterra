using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;

namespace Maritaria.Blueprints
{
	public sealed class BlueprintsMod : TerraTechMod
	{
		public static readonly BlockTypes CompressedTechBlockID = (BlockTypes)7000;

		private GameObject _holder;

		public override string Name => nameof(BlueprintsMod);
		public override string Description => "";

		public override void Load()
		{
			base.Load();

			var bundle = AssetBundle.LoadFromFile(Path.Combine(FolderStructure.AssetsFolder, "mod-blueprints"));
			var model = bundle.LoadAsset<GameObject>(@"Assets/Blocks/CompressedTech/PackageBlock.prefab");

			new BlockPrefabBuilder()
				.SetBlockID((int)CompressedTechBlockID)
				.SetCategory(BlockCategories.Null)
				.SetMass(10f)
				.SetModel(model)
				.SetSize(new Vector3I(1, 1, 1), BlockPrefabBuilder.AttachmentPoints.All)
				.AddComponent<CompressedTechContainer>()
				.Register();

			_holder = new GameObject();
			UnityEngine.Object.DontDestroyOnLoad(_holder);
			_holder.AddComponent<CompressorTool>();
		}
	}
}