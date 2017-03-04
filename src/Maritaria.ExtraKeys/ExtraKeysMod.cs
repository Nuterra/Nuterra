using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;

namespace Maritaria.ExtraKeys
{
	[Mod]
	public sealed class ExtraKeysMod : TerraTechMod
	{
		private GameObject _obj;
		public override string Name => "ExtraKeys";
		public override string Description => "Adds new keybindings to some things";

		public KeyConfig KeyConfig { get; private set; } = new KeyConfig
		{
			DrillKey = KeyCode.Alpha1,
			HammerKey = KeyCode.Alpha2,
			MagnetToggleKey = KeyCode.Alpha3,
			ScoopKey = KeyCode.Alpha4,
			PlasmaKey = KeyCode.Alpha5,
			SawKey = KeyCode.Alpha6,

			TurnNightKey = KeyCode.Alpha0,
			TurnDayKey = KeyCode.Alpha9,
			SuicideKey = KeyCode.K,
			ProductionToggleKey = KeyCode.O,
		};

		public override void Load()
		{
			base.Load();
			KeyConfig = Config.ToObject<KeyConfig>();
			Hooks.Modules.Drill.CanFire += Drill_CanFire;
			Hooks.Modules.Hammer.CanFire += Hammer_CanFire;
			Hooks.Modules.Scoop.CanFire += Scoop_CanFire;
			Hooks.Modules.Weapon.CanFire += Weapon_CanFire;
			_obj = new GameObject();
			_obj.AddComponent<SuicideKey>().Mod = this;
			_obj.AddComponent<TimeOfDayKeys>().Mod = this;
			_obj.AddComponent<ProductionToggleKey>().Mod = this;
			UnityEngine.Object.DontDestroyOnLoad(_obj);
		}

		public override void Unload()
		{
			base.Unload();
			Hooks.Modules.Drill.CanFire -= Drill_CanFire;
			Hooks.Modules.Hammer.CanFire -= Hammer_CanFire;
			Hooks.Modules.Scoop.CanFire -= Scoop_CanFire;
			Hooks.Modules.Weapon.CanFire -= Weapon_CanFire;
			UnityEngine.Object.Destroy(_obj);
		}

		public override JObject CreateDefaultConfiguration()
		{
			return JObject.FromObject(KeyConfig);
		}

		private void Drill_CanFire(CanFireEvent<ModuleDrill> eventInfo)
		{
			eventInfo.Fire |= Input.GetKey(KeyConfig.DrillKey);
		}

		private void Hammer_CanFire(CanFireEvent<ModuleHammer> eventInfo)
		{
			eventInfo.Fire |= Input.GetKey(KeyConfig.HammerKey);
		}

		private void Scoop_CanFire(CanFireEvent<ModuleScoop> eventInfo)
		{
			eventInfo.Fire |= Input.GetKey(KeyConfig.ScoopKey);
		}

		private void Weapon_CanFire(CanFireEvent<ModuleWeapon> eventInfo)
		{
			if (eventInfo.Module.block.BlockType == BlockTypes.GCPlasmaCutter_222)
			{
				eventInfo.Fire |= Input.GetKey(KeyConfig.PlasmaKey);
			}
		}
	}
}