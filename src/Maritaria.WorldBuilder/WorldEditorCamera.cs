using System;
using System.Collections.Generic;
using System.Linq;
using Nuterra.Internal;
using UnityEngine;

namespace Maritaria.WorldBuilder
{
	public sealed class WorldEditorCamera : CameraManager.Camera
	{
		public static WorldEditorCamera inst { get; private set; }

		public static void InitSingleton(GameObject obj)
		{
			inst = obj.AddComponent<WorldEditorCamera>();
			inst.enabled = false;
		}

		private bool _mouseDragging = false;
		private Vector3 _mouseStart;
		private Quaternion _rotationStart;
		private Quaternion _rotation;
		private Vector3 _position;
		private float _movementSpeed = 0.3f;

		private void Awake()
		{
			Hooks.Modules.TankControl.CanControlTank += TankControl_CanControlTank;
			Hooks.Managers.Pointer.OnCameraSpinStart += Pointer_OnCameraSpinStart;
			Hooks.Managers.Pointer.OnCameraSpinEnd += Pointer_OnCameraSpinEnd;
			ManGameMode.inst.PreExitModeEvent.Subscribe(OnGameModeChange);
		}

		private void TankControl_CanControlTank(CanControlTankEvent info)
		{
			if (CameraManager.inst.IsCurrent<WorldEditorCamera>())
			{
				info.CanControl = false;
			}
		}

		private void Pointer_OnCameraSpinStart()
		{
			if (CameraManager.inst.IsCurrent<WorldEditorCamera>())
			{
				_mouseDragging = true;
				_mouseStart = Input.mousePosition;
				_rotationStart = _rotation;
			}
		}

		private void Pointer_OnCameraSpinEnd()
		{
			if (CameraManager.inst.IsCurrent<WorldEditorCamera>())
			{
				_mouseDragging = false;
			}
		}

		private static void OnGameModeChange(Mode obj)
		{
			if (CameraManager.inst.IsCurrent<WorldEditorCamera>())
			{
				CameraManager.inst.Switch<TankCamera>();
			}
		}

		public override void Enable()
		{
			_position = Singleton.cameraTrans.position;
			var angles = Singleton.cameraTrans.eulerAngles;
			_rotation = Quaternion.Euler(angles.x, angles.y, 0f);
		}

		private void Update()
		{
			if (CameraManager.inst.IsCurrent<WorldEditorCamera>())
			{
				Input_LookAround();
				Input_Flying();
				Input_ResetCamera();
				Update_Camera();
			}
		}

		private void Input_LookAround()
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

		private void Input_Flying()
		{
			if (Input.GetKey(KeyCode.W))
			{
				_position += _rotation * (Vector3.forward * _movementSpeed);
			}
			if (Input.GetKey(KeyCode.S))
			{
				_position += _rotation * (Vector3.back * _movementSpeed);
			}
			if (Input.GetKey(KeyCode.A))
			{
				_position += _rotation * (Vector3.left * _movementSpeed);
			}
			if (Input.GetKey(KeyCode.D))
			{
				_position += _rotation * (Vector3.right * _movementSpeed);
			}
		}

		private void Input_ResetCamera()
		{
			if (Input.GetKeyDown(KeyCode.B))
			{
				_position = Singleton.playerPos + (Vector3.up * 5f) + (Vector3.back * 5f);
				_rotation = Quaternion.LookRotation(Vector3.forward + Vector3.down, Vector3.up);
			}
		}

		private void Update_Camera()
		{
			Singleton.cameraTrans.position = _position;
			Singleton.cameraTrans.rotation = _rotation;
		}
	}
}