using Nuterra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public override void Load()
		{
			base.Load();

			_behaviourHolder = new GameObject();
			_behaviourHolder.AddComponent<MagnetToggleKeyBehaviour>();
			_behaviourHolder.AddComponent<TimeOfDayKeysBehaviour>();
			_behaviourHolder.AddComponent<FirstPersonKeyBehaviour>();
			_behaviourHolder.AddComponent<ProductionToggleKeyBehaviour>();
			UnityEngine.Object.DontDestroyOnLoad(_behaviourHolder);

			SplashScreenHandler.Init();
		}

	}
}
