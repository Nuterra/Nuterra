using System;
using Nuterra;
using UnityEngine;

namespace Maritaria.WorldBuilder
{
	public sealed class WorldBuilderMod : TerraTechMod
	{
		private GameObject holder;
		public override string Name => nameof(WorldBuilderMod);
		public override string Description => "Allows for spawning of trees, rocks, etc.";

		public override void Load()
		{
			base.Load();
			holder = new GameObject();
			holder.AddComponent<EditorHotkey>().Mod = this;
			WorldEditorCamera.InitSingleton(holder);
			UnityEngine.Object.DontDestroyOnLoad(holder);
		}
	}
}