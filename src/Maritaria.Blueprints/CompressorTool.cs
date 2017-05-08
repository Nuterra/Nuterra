using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maritaria.Blueprints
{
	public sealed class CompressorTool : MonoBehaviour
	{
		private void Update()
		{
			if (!Input.GetKeyDown(KeyCode.N)) return;
			if (!ManPointer.inst.targetObject) return;

			var compressed = ManPointer.inst.targetObject.GetComponent<CompressedTechContainer>();
			if (compressed)
			{
				compressed.UnloadTech();
			}
			else
			{
				var tank = ManPointer.inst.targetTank;
				if (tank && tank.IsFriendly())
				{
					CompressedTechContainer.CompressTech(tank);
				}
			}
		}

		private Tank TraceMouse(out TankBlock block)
		{
			var mousePos = Input.mousePosition;
			RaycastHit ray;
			bool hit = Physics.Raycast(Singleton.camera.ScreenPointToRay(mousePos), out ray, float.MaxValue, Globals.inst.layerTank.mask, QueryTriggerInteraction.Ignore);
			if (!hit)
			{
				block = null;
				return null;
			}
			block = ray.transform.GetComponent<TankBlock>();
			return ray.transform.GetComponent<Tank>();
		}
	}
}