using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Nuterra;
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
		}
	}
}