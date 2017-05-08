using System;
using UnityEngine;

namespace Maritaria.Cockpit
{
	internal sealed class FirstPersonCamera : CameraManager.Camera
	{
		public static FirstPersonCamera inst { get; private set; }

		private Vector3 _mouseStart = Vector3.zero;
		private bool _mouseDragging = false;
		private Quaternion _rotation;
		private Quaternion _rotationStart;

		public Tank Tank { get; set; }
		public ModuleFirstPerson Module { get; set; }

		private void Awake()
		{
			inst = this;
			ManGameMode.inst.PreExitModeEvent.Subscribe(OnGameModeChange);
		}

		private static void OnGameModeChange(Mode obj)
		{
			if (CameraManager.inst.IsCurrent<FirstPersonCamera>())
			{
				CameraManager.inst.Switch<TankCamera>();
			}
		}

		public override void Enable()
		{
			_rotation = Quaternion.identity;
		}

		private void Update()
		{
			if (!Tank) return;
			if (!Module) return;
			if (!Module.FirstPersonAnchor) return;

			UpdateLocalRotation();
			UpdateCamera();
		}

		private void UpdateLocalRotation()
		{
			if (_mouseDragging)
			{
				Vector3 mouseDelta = Input.mousePosition - _mouseStart;

				mouseDelta = mouseDelta / Screen.width;
				float changeAroundY = mouseDelta.x * TankCamera.inst.spinSensitivity * 400f * Globals.inst.camSpinSensHorizontal;
				float changeAroundX = mouseDelta.y * TankCamera.inst.spinSensitivity * 400f * Globals.inst.camSpinSensVertical;

				changeAroundY += _rotationStart.eulerAngles.y;
				changeAroundX += _rotationStart.eulerAngles.x;

				if (changeAroundX > 180f)
				{
					changeAroundX -= 360f;
				}

				float before = changeAroundX;
				changeAroundX = Mathf.Clamp(changeAroundX, -80, 80);
				Quaternion newRotation = Quaternion.Euler(changeAroundX, changeAroundY, 0);
				_rotation = newRotation;
			}
		}

		private void UpdateCamera()
		{
			Singleton.cameraTrans.position = Module.FirstPersonAnchor.transform.position;
			Singleton.cameraTrans.rotation = Module.FirstPersonAnchor.transform.rotation * _rotation;
		}

		internal void BeginSpinControl()
		{
			_mouseDragging = true;
			_mouseStart = Input.mousePosition;
			_rotationStart = _rotation;
		}

		internal void EndSpinControl()
		{
			_mouseDragging = false;
		}
	}
}