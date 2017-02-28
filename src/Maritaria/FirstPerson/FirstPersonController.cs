using System;
using UnityEngine;

namespace Maritaria.FirstPerson
{
	public class FirstPersonController : Singleton.Manager<FirstPersonController>
	{
		private FirstPersonCamera _camera;
		public bool FirstPersonEnabled { get; set; } = false;

		private void Start()
		{
			_camera = gameObject.AddComponent<FirstPersonCamera>();
			_camera.enabled = false;
		}

		private void Update()
		{
			if (Input.GetKeyDown(MaritariaMod.Instance.Config.FirstPersonKey))
			{
				FirstPersonEnabled = !FirstPersonEnabled;
				if (FirstPersonEnabled)
				{
					ActivateFirstPerson();
				}
				else
				{
					RevertCamera();
				}
			}
			UpdateCamera();
		}

		private void UpdateCamera()
		{
			if (FirstPersonEnabled)
			{
				Console.WriteLine(1);
				Tank player = Singleton.playerTank;
				Console.WriteLine(2);
				if (player == null || !player || _camera == null)
				{
					Console.WriteLine(3);
					RevertCamera();
					return;
				}
				Console.WriteLine(4);
				_camera.Tank = player;
				var module = player.blockman.IterateBlockComponents<ModuleFirstPerson>().FirstOrDefault();
				if (module == null || !module)
				{
					RevertCamera();
					return;
				}
				_camera.Module = module;
			}
		}

		private void ActivateFirstPerson()
		{
			CameraManager.inst.Switch(_camera);
		}

		private void RevertCamera()
		{
			FirstPersonEnabled = false;
			CameraManager.inst.Switch<TankCamera>();
		}

		public static class Hooks_TankControl
		{
			public static bool PlayerInput(TankControl tank)
			{
				bool isAllowedCamera = CameraManager.inst.IsCurrent<TankCamera>() || CameraManager.inst.IsCurrent<FirstPersonCamera>();
				bool isLockedDebugCamera = CameraManager.inst.IsCurrent<DebugCamera>() && CameraManager.inst.GetDebugCamera().IsLocked;
				return (isAllowedCamera || isLockedDebugCamera);
			}
		}

		public static class Hooks_ManPointer
		{
			public static void StartCameraSpin(ManPointer inst)
			{
				if (CameraManager.inst.IsCurrent<FirstPersonCamera>())
				{
					FirstPersonCamera.inst.BeginSpinControl();
				}
			}

			public static void StopCameraSpin(ManPointer inst)
			{
				if (CameraManager.inst.IsCurrent<FirstPersonCamera>())
				{
					FirstPersonCamera.inst.EndSpinControl();
				}
			}
		}
	}
}