using System;
using UnityEngine;

namespace Maritaria.Cockpit
{
	public class FirstPersonController : Singleton.Manager<FirstPersonController>
	{
		private FirstPersonCamera _camera;
		public bool FirstPersonEnabled { get; set; } = false;

		public CockpitMod Mod { get; set; }

		private void Start()
		{
			_camera = gameObject.AddComponent<FirstPersonCamera>();
			_camera.enabled = false;
		}

		private void Update()
		{
			if (Input.GetKeyDown(Mod.CockpitConfig.FirstPersonKey))
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
				Tank player = Singleton.playerTank;
				if (player == null || !player || _camera == null)
				{
					RevertCamera();
					return;
				}
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
	}
}