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
						if (!module.m_Lifted && shouldFireEvent.Fire)
						{
							module.actuator.Play(module.m_LiftAnim.name);
							module.m_Lifted = true;
							return;
						}
						if (module.m_Lifted && (!shouldFireEvent.Fire || (module.m_UpAndDownMode && shouldFireEvent.Fire)))
						{
							module.actuator.Play(module.m_DropAnim.name);
							module.m_Lifted = false;
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

			public static class TankControl
			{
				internal static bool PlayerInput(global::TankControl tank)
				{
					bool isAllowedCamera = CameraManager.inst.IsCurrent<TankCamera>();
					bool isLockedDebugCamera = CameraManager.inst.IsCurrent<DebugCamera>() && CameraManager.inst.GetDebugCamera().IsLocked;
					bool defaultResult = (isAllowedCamera || isLockedDebugCamera);

					var eventResult = new CanControlTankEvent(tank, defaultResult);
					CanControlTank?.Invoke(eventResult);
					return eventResult.CanControl;
				}

				public static event Action<CanControlTankEvent> CanControlTank;
			}

			public static class Heart
			{
				internal static void Update(ModuleHeart module)
				{
					bool onlineOriginal = module.IsOnline;
					var updateEvent = new HeartUpdateEvent(module, onlineOriginal);

					OnUpdate?.Invoke(updateEvent);

					if (!updateEvent.IsOnline && onlineOriginal)
					{
						module.DropAllItems(false);
						module.m_ReadyAfterTime = Time.time + 0.0001f;
					}
				}

				public static event Action<HeartUpdateEvent> OnUpdate;
			}

			public static class ItemPickup
			{
				internal static bool CanAcceptItem(ModuleItemPickup module, bool defaultResult, Visible item, ModuleItemHolder.Stack fromStack, ModuleItemHolder.Stack toStack, ModuleItemHolder.PassType passType)
				{
					var canPickupEvent = new ItemPickupEvent(module, defaultResult, item, fromStack, toStack, passType);
					CanPickup?.Invoke(canPickupEvent);
					return canPickupEvent.CanPickup;
				}

				public static event Action<ItemPickupEvent> CanPickup;

				internal static bool CanReleaseItem(ModuleItemPickup module, bool defaultResult, Visible item, ModuleItemHolder.Stack fromStack, ModuleItemHolder.Stack toStack, ModuleItemHolder.PassType passType)
				{
					var canPickupEvent = new ItemPickupEvent(module, defaultResult, item, fromStack, toStack, passType);
					CanRelease?.Invoke(canPickupEvent);
					return canPickupEvent.CanPickup;
				}

				public static event Action<ItemPickupEvent> CanRelease;
			}
		}

		public static class Managers
		{
			public static class Licenses
			{
				//Hook to be called at the end of Licenses.Init
				internal static void Init(ManLicenses inst, int unused)
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
					internal static void OnSerializing(ManStats.IntStatList list, StreamingContext context)
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
					OnCameraSpinStart?.Invoke();
				}

				public static event Action OnCameraSpinStart;

				//Hook to be called at the end of ManPointer.StopCameraSpin
				internal static void StopCameraSpin(ManPointer inst)
				{
					OnCameraSpinEnd?.Invoke();
				}

				public static event Action OnCameraSpinEnd;
			}

			public static class SaveGame
			{
				internal static bool SaveSaveData(ManSaveGame.SaveData saveData, string filePath)
				{
					FlagSave(saveData);
					var saveEvent = new SaveGameEvent(saveData, filePath);
					OnSave?.Invoke(saveEvent);
					return saveEvent.CancelSave;
				}

				internal static void FlagSave(ManSaveGame.SaveData saveData)
				{
#warning Flag saves here
					//Console.WriteLine($"Flagging save file (current={saveData.State.m_OverlayData ?? "null"})");
					//saveData.State.m_OverlayData = "Save loaded by modded game";
				}

				public static event Action<SaveGameEvent> OnSave;
			}

			public static class SplashScreen
			{
				public static bool Initialized { get; set; }

				public static event Action<ManSplashScreen> Initializing;

				internal static void Awake(ManSplashScreen manager)
				{
					Initialized = true;
					Initializing?.Invoke(manager);
				}
			}

			public static class Screenshot
			{
				internal static void EncodeCompressedPreset(byte[] compressedSerialisedPreset, int serialisedPresetSize, Texture2D texture)
				{
					var info = new ScreenshotEvent(texture);
					BeforeEncodeScreenshot?.Invoke(info);
				}

				public static event Action<ScreenshotEvent> BeforeEncodeScreenshot;
			}
		}

		public static class BugReports
		{
			internal static void MarkReportForm(WWWForm form)
			{
				PreBugReportSubmit?.Invoke(form);
			}

			public static event Action<WWWForm> PreBugReportSubmit;

			internal static string MarkUserMessage(string userMessage)
			{
				var messageEvent = new BugReportMessageEvent(userMessage);
				AlterUserMessage?.Invoke(messageEvent);
				return messageEvent.UserMessage;
			}

			public static event Action<BugReportMessageEvent> AlterUserMessage;
		}

		public static class ResourceLookup
		{
			//Hook at start of method, override result if not null (http://prntscr.com/dqv0zy)
			internal static string GetString(int itemType, LocalisationEnums.StringBanks itemEnum)
			{
				var lookupEvent = new StringLookupEvent(itemEnum, itemType);
				OnStringLookup?.Invoke(lookupEvent);
				return lookupEvent.Result;
			}

			public static event Action<StringLookupEvent> OnStringLookup;

			//Hook at start of method, override result if not null (http://prntscr.com/dqvhrh)
			internal static Sprite GetSprite(ObjectTypes objectType, int itemType)
			{
				var lookupEvent = new SpriteLookupEvent(objectType, itemType);
				OnSpriteLookup?.Invoke(lookupEvent);
				return lookupEvent.Result;
			}

			public static event Action<SpriteLookupEvent> OnSpriteLookup;
		}
	}

	public sealed class ScreenshotEvent
	{
		/// <summary>
		/// The image the tech will be encoded onto.
		/// </summary>
		public Texture2D Texture { get; }

		public ScreenshotEvent(Texture2D image)
		{
			if (image == null) throw new ArgumentNullException(nameof(image));
			Texture = image;
		}
	}

	public sealed class ItemPickupEvent
	{
		public ModuleItemPickup Module { get; }
		public bool CanPickup { get; set; }
		public Visible Item { get; }
		public ModuleItemHolder.Stack Source { get; }
		public ModuleItemHolder.Stack Target { get; }
		public ModuleItemHolder.PassType PassType { get; }

		public ItemPickupEvent(ModuleItemPickup module, bool canPickup, Visible item, ModuleItemHolder.Stack fromStack, ModuleItemHolder.Stack toStack, ModuleItemHolder.PassType passType)
		{
			if (module == null) throw new ArgumentNullException(nameof(item));
			Module = module;
			CanPickup = canPickup;
			Item = item;
			Source = fromStack;
			Target = toStack;
			PassType = passType;
		}
	}

	public sealed class HeartUpdateEvent
	{
		public ModuleHeart Module { get; }
		public bool IsOnline { get; set; }

		public HeartUpdateEvent(ModuleHeart module, bool isOnlineOriginal)
		{
			if (module == null) throw new ArgumentNullException(nameof(module));
			Module = module;
			IsOnline = isOnlineOriginal;
		}
	}

	public sealed class CanControlTankEvent
	{
		public TankControl ControlComponent { get; }
		public bool CanControl { get; set; }

		public CanControlTankEvent(TankControl control, bool canControl)
		{
			if (control == null) throw new ArgumentNullException(nameof(control));
			ControlComponent = control;
			CanControl = canControl;
		}
	}

	public sealed class SpriteLookupEvent
	{
		public ObjectTypes ObjectType { get; }
		public int ItemType { get; }

		public Sprite Result { get; set; }

		public SpriteLookupEvent(ObjectTypes objectType, int itemType)
		{
			ObjectType = objectType;
			ItemType = itemType;
		}
	}

	public sealed class StringLookupEvent
	{
		public LocalisationEnums.StringBanks StringBank { get; }
		public int EnumValue { get; }
		public string Result { get; set; }

		public StringLookupEvent(LocalisationEnums.StringBanks type, int value)
		{
			StringBank = type;
			EnumValue = value;
		}
	}

	public sealed class SaveGameEvent
	{
		public ManSaveGame.SaveData Data { get; }
		public string FilePath { get; set; }
		public bool CancelSave { get; set; } = false;

		public SaveGameEvent(ManSaveGame.SaveData data, string filePath)
		{
			Data = data;
			FilePath = filePath;
		}
	}

	public sealed class BugReportMessageEvent
	{
		public string UserMessage { get; set; }

		public BugReportMessageEvent(string userMessage)
		{
			UserMessage = userMessage;
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
		public bool PerformVanillaBehaviour { get; set; } = true;

		public FireEvent(T module)
		{
			if (module == null) throw new ArgumentNullException(nameof(module));
			Module = module;
		}
	}
}