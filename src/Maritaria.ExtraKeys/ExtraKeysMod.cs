using System;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;

namespace Maritaria.ExtraKeys
{
	[Mod]
	public sealed class ExtraKeysMod : TerraTechMod
	{
		public override string Name => nameof(ExtraKeysMod);
		public override string Description => "Adds new keybindings to some things";

		public override void Load()
		{
			base.Load();
			Hooks.Modules.Drill.CanFire += Drill_CanFire;
			Hooks.Modules.Hammer.CanFire += Hammer_CanFire;
			Hooks.Modules.Scoop.CanFire += Scoop_CanFire;
			Hooks.Modules.Weapon.CanFire += Weapon_CanFire;
		}

		private void Drill_CanFire(CanFireEvent<ModuleDrill> eventInfo)
		{
			eventInfo.Fire |= Input.GetKeyDown(KeyCode.Alpha1);
			if (eventInfo.Module.block.tank == Singleton.playerTank)
			{
			}
		}

		private void Hammer_CanFire(CanFireEvent<ModuleHammer> eventInfo)
		{
			if (eventInfo.Module.block.tank == Singleton.playerTank)
			{
				eventInfo.Fire |= Input.GetKeyDown(KeyCode.Alpha2);
			}
		}

		private void Scoop_CanFire(CanFireEvent<ModuleScoop> eventInfo)
		{
			if (eventInfo.Module.block.tank == Singleton.playerTank)
			{
				eventInfo.Fire |= Input.GetKeyDown(KeyCode.Alpha3);
			}
		}

		private void Weapon_CanFire(CanFireEvent<ModuleWeapon> eventInfo)
		{
			if (eventInfo.Module.block.BlockType == BlockTypes.GCPlasmaCutter_222)
			{
				eventInfo.Fire |= Input.GetKeyDown(KeyCode.Alpha3);
			}
		}
	}
}