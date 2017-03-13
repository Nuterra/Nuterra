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
			BlockLoader.Register(new CockpitBlock());
			BlockLoader.Register(new LargeCockpitBlock());
		}

		public override JObject CreateDefaultConfiguration()
		{
			return JObject.FromObject(CockpitConfig);
		}
	}
}