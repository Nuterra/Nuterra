using System;
using Nuterra.Internal;
using UnityEngine;

namespace Maritaria.ExtraKeys
{
	public sealed class ProductionToggleKey : MonoBehaviour
	{
		public ExtraKeysMod Mod { get; set; }
		public bool ProductionActive { get; set; }

		private void OnEnable()
		{
			Hooks.Modules.Heart.OnUpdate += Heart_OnUpdate;
		}

		private void Update()
		{
			if (Input.GetKeyDown(Mod.KeyConfig.ProductionToggleKey))
			{
				ProductionActive = !ProductionActive;
			}
		}

		private void OnDisable()
		{
			Hooks.Modules.Heart.OnUpdate -= Heart_OnUpdate;
		}

		private void Heart_OnUpdate(HeartUpdateEvent eventInfo)
		{
			eventInfo.IsOnline &= ProductionActive;
		}
	}
}