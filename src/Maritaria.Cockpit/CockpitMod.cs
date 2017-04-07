using System;
using Newtonsoft.Json.Linq;
using Nuterra;
using UnityEngine;

namespace Maritaria.Cockpit
{
	public sealed class CockpitMod : TerraTechMod
	{
		public override string Name => nameof(CockpitMod);
		public override string Description => "Adds various cockpit blocks to the game to experience the game in first-person";

		public CockpitConfig CockpitConfig { get; private set; } = new CockpitConfig
		{
			FirstPersonKey = KeyCode.F,
		};

		public override void Load()
		{
			base.Load();
			CockpitConfig = Config.ToObject<CockpitConfig>();

			new BlockPrefabBuilder()
				.SetBlockID(9000)
				.SetName("GSO Cockpit")
				.SetDescription("Pop in here and have a first-person look at the world from this block")
				.SetPrice(300)
				.SetFaction(FactionSubTypes.GSO)
				.SetCategory(BlockCategories.Accessories)
				.SetSize(new Vector3I(1, 1, 1))
				.SetModel(@"Assets/Blocks/Cockpit/CockpitBlock.prefab")
				.SetIcon(@"Assets/Blocks/Cockpit/block_icon.png")
				.AddComponent<ModuleFirstPerson>()
				.Register();
			new BlockPrefabBuilder()
				.SetBlockID(9005)
				.SetName("GSO Observatory")
				.SetDescription("Mount this gigantic hamsterball to your tech to be right in the action!")
				.SetPrice(500)
				.SetFaction(FactionSubTypes.GSO)
				.SetCategory(BlockCategories.Accessories)
				.SetSize(new Vector3I(2, 2, 2))
				.SetModel(@"Assets/Blocks/Cockpit/Cockpit.Large.prefab")
				.SetIcon(@"Assets/Blocks/Cockpit/Cockpit.Large.Icon.png")
				.AddComponent<ModuleFirstPerson>()
				.Register();
		}

		public override JObject CreateDefaultConfiguration()
		{
			return JObject.FromObject(CockpitConfig);
		}
	}
}