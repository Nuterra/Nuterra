using System;
using Newtonsoft.Json.Linq;
using Nuterra;

using System;

using UnityEngine;

namespace Maritaria
{
	[Mod]
	public sealed class MaritariaMod : TerraTechMod
	{
		public override string Name => nameof(MaritariaMod);
		public override string Description => "The first mod for TerraTech";
		public override Version Version => new Version(1, 0, 0);

		private GameObject _behaviourHolder;

		public MaritariaConfig Config { get; } = new MaritariaConfig();

		internal static MaritariaMod Instance { get; private set; }

		public override void Load()
		{
			Instance = this;

			base.Load();

			_behaviourHolder = new GameObject();
			_behaviourHolder.AddComponent<MagnetToggleKeyBehaviour>();
			_behaviourHolder.AddComponent<TimeOfDayKeysBehaviour>();
			_behaviourHolder.AddComponent<FirstPersonKeyBehaviour>();
			_behaviourHolder.AddComponent<ProductionToggleKeyBehaviour>();
			_behaviourHolder.AddComponent<Gameslynx.SuicideKeyBehaviour>();
			UnityEngine.Object.DontDestroyOnLoad(_behaviourHolder);

			Config.Load(ModConfig.Data.GetValue(Name, StringComparison.OrdinalIgnoreCase) as JObject);

			BlockLoader.Register(new CockpitBlock());

			SplashScreenHandler.Init();
		}
	}
}