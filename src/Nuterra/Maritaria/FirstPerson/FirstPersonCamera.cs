using System;
using UnityEngine;

namespace Maritaria.FirstPerson
{
	internal sealed class FirstPersonCamera : CameraManager.Camera
	{

		public static FirstPersonCamera inst { get; private set; }

		private void Awake()
		{
			inst = this;
		}

		public Tank Tank { get; set; }
		public ModuleFirstPerson Module { get; set; }

		public Quaternion Rotation { get; set; }

		public override void Enable()
		{
			Rotation = Quaternion.identity;
		}

		private void Update()
		{
			if (!Tank) return;
			if (!Module) return;

			UpdateLocalRotation();
			UpdateCamera();
		}

		private void UpdateLocalRotation()
		{

		}

		private void UpdateCamera()
		{
			Singleton.cameraTrans.position = Module.transform.position;
			Singleton.cameraTrans.rotation = Module.transform.rotation * Rotation;
		}

		internal void BeginSpinControl()
		{
			throw new NotImplementedException();
		}

		internal void EndSpinControl()
		{
			throw new NotImplementedException();
		}
	}
}