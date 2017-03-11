using System;
using System.Reflection;
using Nuterra.Internal;
using UnityEngine;

namespace Maritaria.ExtraKeys
{
	public sealed class MagnetToggleKey : MonoBehaviour
	{
		public ExtraKeysMod Mod { get; set; }

		public bool MagnetsEnabled { get; set; }

		private void OnEnable()
		{
			MagnetsEnabled = true;
			Hooks.Modules.ItemPickup.CanPickup += ItemPickup_CanPickup;
		}

		private void OnDisable()
		{
			Hooks.Modules.ItemPickup.CanPickup -= ItemPickup_CanPickup;
		}

		private void ItemPickup_CanPickup(ItemPickupEvent obj)
		{
			if (!Singleton.playerTank) return;
			if (obj.Module.block.tank != Singleton.playerTank) return;

			var magnet = obj.Module.block.GetComponent<ModuleItemHolderMagnet>();
			if (magnet == null) return;

			obj.CanPickup &= MagnetsEnabled;
		}

		private void Update()
		{
			if (Input.GetKeyDown(Mod.KeyConfig.MagnetToggleKey))
			{
				MagnetsEnabled = !MagnetsEnabled;
			}
			if (!MagnetsEnabled && Singleton.playerTank)
			{
#warning Use of reflection
				var method = typeof(ModuleItemHolderMagnet).GetMethod("UnglueAllObjects", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				var holderField = typeof(ModuleItemHolderMagnet).GetField("m_Holder", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (ModuleItemHolderMagnet magnet in Singleton.playerTank.blockman.IterateBlockComponents<ModuleItemHolderMagnet>())
				{
					method.Invoke(magnet, new object[] { false });
					var holder = (ModuleItemHolder)holderField.GetValue(magnet);
					holder.DropAll();
				}
			}
			if (Input.GetKeyDown(KeyCode.H))
			{
				Tank tank = Singleton.playerTank;
				if (!tank) return;

				float closestDistance = float.MaxValue;
				Tank closestTank = null;
				foreach (Tank other in ManTechs.inst.IterateTechs())
				{
					if (other.IsAnchored) continue;
					if (other == tank) continue;
					float distance = Vector3.Distance(tank.gameObject.transform.position, other.gameObject.transform.position);
					if (distance < closestDistance)
					{
						distance = closestDistance;
						closestTank = other;
					}
				}
				if (closestTank)
				{
					Console.WriteLine("Found a close one");
					Vector3 offset = Vector3.up * ((tank.blockBounds.extents.z + closestTank.blockBounds.extents.z + 2) / 2);
					var joint = tank.gameObject.AddComponent<FixedJoint>();
					closestTank.gameObject.transform.position = tank.gameObject.transform.position + offset;
					closestTank.gameObject.transform.rotation = tank.gameObject.transform.rotation;
					joint.connectedAnchor = closestTank.boundsCentreWorld;
					joint.anchor = offset;
					joint.axis = Vector3.up;
					joint.connectedBody = closestTank.rbody;
				}
				else
				{
					Console.WriteLine("noone near");
				}
			}
			if (Input.GetKeyDown(KeyCode.J))
			{
				Tank tank = Singleton.playerTank;
				if (!tank) return;
				tank.gameObject.transform.position += Vector3.up * 5;
				var existingJoint = tank.gameObject.GetComponent<FixedJoint>();
				if (existingJoint != null)
				{
					GameObject.Destroy(existingJoint);
					return;
				}
			}
		}
	}
}