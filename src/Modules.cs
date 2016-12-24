using System;
using UnityEngine;

namespace Maritaria
{
	public static class Modules
	{
		public static class Drill
		{
			public static void Input(ModuleDrill module, int aim, bool fire)
			{
				KeyCode keyCode = Mod.Config.DrillKey;
				if ((module.block != null) && (module.block.BlockType == BlockTypes.GCBuzzSaw_312))
				{
					keyCode = Mod.Config.SawKey;
				}
				module.m_Spinning = (fire || Modules.GetPlayerInput(module, keyCode));
			}
		}
		public static class Magnet
		{
			public static bool DisabledForPlayerControlledTank;
			public static void FixedUpdate(ModuleItemHolderMagnet module)
			{
				if (module.block.tank && module.block.tank.IsPlayer && DisabledForPlayerControlledTank)
				{
					if (!module.m_Holder.IsEmpty)
					{
						module.UnglueAllObjects(false);
						module.m_Holder.DropAll();
					}
					module.m_PickupDelayTimeout = Time.time + module.m_Pickup.PrePickupPeriod;
					return;
				}
				module.UpdateItemMovement();
				module.m_Holder.PickupContentionPeriod = Mathf.Max(module.m_PickupDelayTimeout - Time.time, 0.001f);
				module.m_SettleThresholdSqr = module.m_SettlingSpeedThreshold * module.m_SettlingSpeedThreshold;
			}

		}
		public static class Scoop
		{
			public static void Scoop_Input(ModuleScoop module, int aim, bool fire)
			{
				fire = (fire || Modules.GetPlayerInput(module, Mod.Config.ScoopKey));
				if (module.actuator.isPlaying)
				{
					return;
				}
				if (!module.lifted & fire)
				{
					module.actuator.Play(module.lift.name);
					module.lifted = true;
					return;
				}
				if (module.lifted && (!fire || (module.upAndDownMode & fire)))
				{
					module.actuator.Play(module.drop.name);
					module.lifted = false;
				}
			}
		}
		public static class Hammer
		{
			public static void Hammer_Input(ModuleHammer module, int aim, bool fire)
			{
				module.state.enabled=(fire || Modules.GetPlayerInput(module, Mod.Config.HammerKey));
			}
		}
		public static class Weapon
		{
			public static void Input(ModuleWeapon module, int aim, bool fire)
			{
				if (module.block.BlockType == BlockTypes.GCPlasmaCutter_222)
				{
					fire = (fire || Modules.GetPlayerInput(module, Mod.Config.PlasmaKey));
				}
				module.AimControl = aim;
				module.FireControl = (fire && Time.timeScale != 0f);
				module.m_TargetPosition = Vector3.zero;
				if (module.FireControl && module.block.tank && module.block.tank.beam.IsActive && !Mode<ModeMain>.inst.TutorialLockBeam)
				{
					module.block.tank.beam.EnableBeam(false, false);
				}
			}
		}	
		public static class Energy
		{
			public static void OnUpdateSupplyEnergy(ModuleEnergy module)
			{
				module.IsGenerating = (module.m_OutputPerSecond != 0f && module.CheckOutputConditions());
				if (module.IsGenerating)
				{
					float num = Modules.Energy.GetOutputMultiplier(module);
					module.Supply(module.m_OutputEnergyType, module.m_OutputPerSecond * num * Time.deltaTime);
				}
				if (module.m_AnimatorController != null)
				{
					module.m_AnimatorController.Set(module.m_GeneratingEnergyBool, module.IsGenerating);
				}
			}

			public static bool CheckOutputConditions(ModuleEnergy module)
			{
				return Modules.Energy.CheckOutputConditions_Anchored(module) && Modules.Energy.CheckOutputConditions_DayTime(module) && Modules.Energy.CheckOutputConditions_ThermalSource(module);
			}

			public static bool CheckOutputConditions_Anchored(ModuleEnergy module)
			{
				if (module.block.BlockType == BlockTypes.GSOGeneratorSolar_141 && module.block.tank && !module.block.tank.beam.IsActive && module.block.tank.rbody.velocity.magnitude < Mod.Config.MobileSolarVelocityThreshold)
				{
					return Mod.Config.MobileSolarPanels;
				}
				return (module.m_OutputConditions & ModuleEnergy.OutputConditionFlags.Anchored) == (ModuleEnergy.OutputConditionFlags)0 || (module.block.tank && module.block.tank.IsAnchored);
			}

			public static bool CheckOutputConditions_DayTime(ModuleEnergy module)
			{
				return (module.m_OutputConditions & ModuleEnergy.OutputConditionFlags.DayTime) == (ModuleEnergy.OutputConditionFlags)0 || !Singleton.Manager<ManTimeOfDay>.inst.NightTime;
			}

			public static bool CheckOutputConditions_ThermalSource(ModuleEnergy module)
			{
				return (module.m_OutputConditions & ModuleEnergy.OutputConditionFlags.Thermal) == (ModuleEnergy.OutputConditionFlags)0 || module.m_ThermalSourceInRange;
			}

			public static float GetOutputMultiplier(ModuleEnergy module)
			{
				float result = 1f;
				if (module.block.BlockType == BlockTypes.GSOGeneratorSolar_141 && Mod.Config.MobileSolarPanels)
				{
					result = 0.1f;
				}
				if ((module.m_OutputConditions & ModuleEnergy.OutputConditionFlags.Thermal) != (ModuleEnergy.OutputConditionFlags)0)
				{
					result = module.m_ThermalSourceInRange.PowerMultiplier;
				}
				return result;
			}
		}
		public static bool GetPlayerInput(Module module, KeyCode keyCode)
		{
			return module.block.tank.PlayerFocused && Input.GetKey(keyCode);
		}
	}
}
