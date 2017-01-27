using System;
using UnityEngine;

namespace Maritaria
{
	public static class Modules
	{
		public static class Drill
		{
			//Hook by replacing method body of ModuleDrill.ControlInput()
			public static void ControlInput(ModuleDrill module, int aim, int fire)
			{
				KeyCode keyCode = MaritariaMod.Instance.Config.DrillKey;
				if ((module.block != null) && (module.block.BlockType == BlockTypes.GCBuzzSaw_312))
				{
					keyCode = MaritariaMod.Instance.Config.SawKey;
				}
				module.m_Spinning = (fire != 0) || Modules.GetPlayerInput(module, keyCode);
			}
		}

		public static class Energy
		{
			//Hook by replacing method body of ModuleEnergy.OnUpdateSupplyEnergy()
			public static void OnUpdateSupplyEnergy(ModuleEnergy module, int dummy)
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

			//Hook by replacing method body of ModuleEnergy.CheckOutputCondictions()
			public static bool CheckOutputConditions(ModuleEnergy module)
			{
				return Modules.Energy.CheckOutputConditions_Anchored(module) && Modules.Energy.CheckOutputConditions_DayTime(module) && Modules.Energy.CheckOutputConditions_ThermalSource(module);
			}

			public static bool CheckOutputConditions_Anchored(ModuleEnergy module)
			{
				if (module.block.BlockType == BlockTypes.GSOGeneratorSolar_141 && module.block.tank && !module.block.tank.beam.IsActive && module.block.tank.rbody.velocity.magnitude < MaritariaMod.Instance.Config.MobileSolarVelocityThreshold)
				{
					return MaritariaMod.Instance.Config.MobileSolarPanels;
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
				if (module.block.BlockType == BlockTypes.GSOGeneratorSolar_141 && MaritariaMod.Instance.Config.MobileSolarPanels)
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

		public static class Hammer
		{
			//Hook by replacing method body of ModuleHammer.ControlInput()
			public static void ControlInput(ModuleHammer module, int aim, int fire)
			{
				module.state.enabled = (fire != 0) || Modules.GetPlayerInput(module, MaritariaMod.Instance.Config.HammerKey);
			}
		}

		public static class Magnet
		{
			public static bool DisabledForPlayerControlledTank;

			//Hook by replacing method body of ModuleItemHolderMagnet.FixedUpdate()
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
			//Hook by replacing method body of ModuleScoop.ControlInput()
			public static void ControlInput(ModuleScoop module, int aim, int fire)
			{
				bool shouldFire = (fire != 0) || Modules.GetPlayerInput(module, MaritariaMod.Instance.Config.ScoopKey);
				if (module.actuator.isPlaying)
				{
					return;
				}
				if (!module.lifted && shouldFire)
				{
					module.actuator.Play(module.lift.name);
					module.lifted = true;
					return;
				}
				if (module.lifted && (!shouldFire || (module.upAndDownMode && shouldFire)))
				{
					module.actuator.Play(module.drop.name);
					module.lifted = false;
				}
			}
		}

		public static class Weapon
		{
			public static void ControlInputManual(ModuleWeapon module, int aim, int fire_input)
			{
				bool fire = (fire_input != 0);
				if (module.block.BlockType == BlockTypes.GCPlasmaCutter_222)
				{
					fire |= Modules.GetPlayerInput(module, MaritariaMod.Instance.Config.PlasmaKey);
				}
				module.AimControl = aim;
				module.FireControl = fire && (Time.timeScale != 0f);
				module.m_TargetPosition = Vector3.zero;
				if (module.FireControl && module.block.tank && module.block.tank.beam.IsActive && !Mode<ModeMain>.inst.TutorialLockBeam)
				{
					module.block.tank.beam.EnableBeam(false, false);
				}
			}
		}

		public static bool GetPlayerInput(Module module, KeyCode keyCode)
		{
			return module.block.tank.PlayerFocused && Input.GetKey(keyCode);
		}
	}
}