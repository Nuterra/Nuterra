using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Nuterra.Internal
{
	public static class Hooks
	{
		public static class Modules
		{
			public static class Drill
			{
				//Hook by replacing method body of ModuleDrill.ControlInput()
				internal static void ControlInput(ModuleDrill module, int aim, int fire)
				{
					bool shouldFireDefault = (fire != 0);
					var shouldFireEvent = new CanFireEvent<ModuleDrill>(module, shouldFireDefault);
					CanFire?.Invoke(shouldFireEvent);

					bool performBehaviour = true;
					if (shouldFireEvent.Fire)
					{
						var fireEvent = new FireEvent<ModuleDrill>(module);
						OnFire?.Invoke(fireEvent);
						performBehaviour = fireEvent.PerformVanillaBehaviour;
					}
					if (performBehaviour)
					{
						module.m_Spinning = shouldFireEvent.Fire;
					}
				}

				public static event Action<CanFireEvent<ModuleDrill>> CanFire;

				public static event Action<FireEvent<ModuleDrill>> OnFire;
			}

			public static class Energy
			{
				//Hook by replacing method body of ModuleEnergy.OnUpdateSupplyEnergy()
				internal static void OnUpdateSupplyEnergy(ModuleEnergy module, int dummy)
				{
					module.IsGenerating = (module.m_OutputPerSecond != 0f && module.CheckOutputConditions());

					EnergyUpdateEvent update = new EnergyUpdateEvent(module);
					update.GeneratingEnergy = module.IsGenerating;
					update.EnergyOutput = module.m_OutputPerSecond * Time.deltaTime;

					OnUpdateEnergy?.Invoke(update);

					if (module.IsGenerating)
					{
						module.Supply(module.m_OutputEnergyType, update.EnergyOutput);
					}
					if (module.m_AnimatorController != null)
					{
						module.m_AnimatorController.Set(module.m_GeneratingEnergyBool, module.IsGenerating);
					}
				}

				public static event Action<EnergyUpdateEvent> OnUpdateEnergy;
			}

			public static class Hammer
			{
				//Hook by replacing method body of ModuleHammer.ControlInput()
				internal static void ControlInput(ModuleHammer module, int aim, int fire)
				{
					bool shouldFireDefault = (fire != 0);
					var shouldFireEvent = new CanFireEvent<ModuleHammer>(module, shouldFireDefault);
					CanFire?.Invoke(shouldFireEvent);

					bool performBehaviour = true;
					if (shouldFireEvent.Fire)
					{
						var fireEvent = new FireEvent<ModuleHammer>(module);
						OnFire?.Invoke(fireEvent);
						performBehaviour = fireEvent.PerformVanillaBehaviour;
#warning Remove duplicate versions of this
					}
					if (performBehaviour)
					{
						module.state.enabled = shouldFireEvent.Fire;
					}
				}

				public static event Action<CanFireEvent<ModuleHammer>> CanFire;

				public static event Action<FireEvent<ModuleHammer>> OnFire;
			}

			public static class Scoop
			{
				//Hook by replacing method body of ModuleScoop.ControlInput()
				internal static void ControlInput(ModuleScoop module, int aim, int fire)
				{
					bool shouldFireDefault = (fire != 0);
					var shouldFireEvent = new CanFireEvent<ModuleScoop>(module, shouldFireDefault);
					CanFire?.Invoke(shouldFireEvent);

					bool performBehaviour = true;
					if (shouldFireEvent.Fire)
					{
						var fireEvent = new FireEvent<ModuleScoop>(module);
						OnFire?.Invoke(fireEvent);
						performBehaviour = fireEvent.PerformVanillaBehaviour;
#warning Remove duplicate versions of this
					}
					if (performBehaviour)
					{
						if (module.actuator.isPlaying)
						{
							return;
						}
						if (!module.lifted && shouldFireEvent.Fire)
						{
							module.actuator.Play(module.lift.name);
							module.lifted = true;
							return;
						}
						if (module.lifted && (!shouldFireEvent.Fire || (module.upAndDownMode && shouldFireEvent.Fire)))
						{
							module.actuator.Play(module.drop.name);
							module.lifted = false;
						}
					}
				}

				public static event Action<CanFireEvent<ModuleScoop>> CanFire;

				public static event Action<FireEvent<ModuleScoop>> OnFire;
			}

			public static class Weapon
			{
				//Hook by replacing method body of ModuleScoop.ControlInputManual()
				internal static void ControlInputManual(ModuleWeapon module, int aim, int fire)
				{
					bool shouldFireDefault = (fire != 0) && (Time.timeScale != 0f);
					var shouldFireEvent = new CanFireEvent<ModuleWeapon>(module, shouldFireDefault);
					CanFire?.Invoke(shouldFireEvent);

					bool performBehaviour = true;
					if (shouldFireEvent.Fire)
					{
						var fireEvent = new FireEvent<ModuleWeapon>(module);
						OnFire?.Invoke(fireEvent);
						performBehaviour = fireEvent.PerformVanillaBehaviour;
#warning Remove duplicate versions of this
					}
					if (performBehaviour)
					{
						module.AimControl = aim;
						module.FireControl = shouldFireEvent.Fire;
						module.m_TargetPosition = Vector3.zero;
						if (module.FireControl && module.block.tank && module.block.tank.beam.IsActive && !Mode<ModeMain>.inst.TutorialLockBeam)
						{
							module.block.tank.beam.EnableBeam(false, false);
						}
					}
				}

				public static event Action<CanFireEvent<ModuleWeapon>> CanFire;

				public static event Action<FireEvent<ModuleWeapon>> OnFire;
			}
		}

		public static class Managers
		{
			public static class Licenses
			{
				//Hook to be called at the end of Licenses.Init
				internal static void Init()
				{
					OnInitializing?.Invoke();
				}

				public static event Action OnInitializing;
			}

			public static class Stats
			{
				public static class IntStatList
				{
					//Hook by replacing method body of ManStats.IntStatList.OnSerializing()
					//Fixes blocks not loading because the name of the block is serialized into a number and can't be resolved by Enum.GetName()
					public static void OnSerializing(ManStats.IntStatList list, StreamingContext context)
					{
						list.m_StatPerTypeSerialized = new Dictionary<string, int>(list.m_StatPerType.Count);
						foreach (KeyValuePair<int, int> current in list.m_StatPerType)
						{
							string value = Enum.GetName(list.m_EnumType, current.Key) ?? current.Key.ToString();
							list.m_StatPerTypeSerialized.Add(value, current.Value);
						}
					}
				}
			}

			public static class Pointer
			{
				//Hook to be called at the end of ManPointer.StartCameraSpin
				internal static void StartCameraSpin(ManPointer inst)
				{
					OnCameraSpinEnd?.Invoke();
				}

				public static event Action OnCameraSpinStart;

				//Hook to be called at the end of ManPointer.StopCameraSpin
				internal static void StopCameraSpin(ManPointer inst)
				{
					OnCameraSpinEnd?.Invoke();
				}

				public static event Action OnCameraSpinEnd;
			}
		}

		public static class Game
		{
		}
	}

	public sealed class EnergyUpdateEvent
	{
		public ModuleEnergy Module { get; }
		public bool GeneratingEnergy { get; set; }
		public float EnergyOutput { get; set; }

		public EnergyUpdateEvent(ModuleEnergy module)
		{
			if (module == null) throw new ArgumentNullException(nameof(module));
			Module = module;
			GeneratingEnergy = module.IsGenerating;
			EnergyOutput = module.m_OutputPerSecond;
		}
	}

	public sealed class CanFireEvent<T> where T : Module
	{
		public T Module { get; }
		public bool Fire { get; set; }

		public CanFireEvent(T module, bool fireDefault)
		{
			if (module == null) throw new ArgumentNullException(nameof(module));
			Module = module;
			Fire = fireDefault;
		}
	}

	public sealed class FireEvent<T> where T : Module
	{
		public T Module { get; }
		public bool PerformVanillaBehaviour { get; set; } = false;

		public FireEvent(T module)
		{
			if (module == null) throw new ArgumentNullException(nameof(module));
			Module = module;
		}
	}
}