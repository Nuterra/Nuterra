using System.Collections.Generic;
using Nuterra.Internal;
using UnityEngine;

namespace Maritaria.ExtraKeys
{
	public sealed class ProductionToggleKey : MonoBehaviour
	{
		private readonly HashSet<BlockTypes> _productionBlocks = new HashSet<BlockTypes> {
			BlockTypes.GSOScrapper_322,
			BlockTypes.GCScrapper_432,
			BlockTypes.VENScrapper_212,
			BlockTypes.HE_Scrapper_323,
			BlockTypes.GSOHeart_343,
		};

		public ExtraKeysMod Mod { get; set; }
		public bool ProductionActive { get; set; }

		private void OnEnable()
		{
			ProductionActive = true;
			Hooks.Modules.Heart.OnUpdate += Heart_OnUpdate;
			Hooks.Modules.ItemPickup.CanPickup += ItemPickup_CanPickup;
		}

		private void OnDisable()
		{
			Hooks.Modules.Heart.OnUpdate -= Heart_OnUpdate;
			Hooks.Modules.ItemPickup.CanPickup -= ItemPickup_CanPickup;
		}

		private void Update()
		{
			if (Input.GetKeyDown(Mod.KeyConfig.ProductionToggleKey))
			{
				ProductionActive = !ProductionActive;
			}
		}

		private void Heart_OnUpdate(HeartUpdateEvent eventInfo)
		{
			//eventInfo.IsOnline &= ProductionActive;
		}

		private void ItemPickup_CanPickup(ItemPickupEvent eventInfo)
		{
			if (_productionBlocks.Contains(eventInfo.Module.block.BlockType))
			{
				eventInfo.CanPickup &= ProductionActive;
			}
		}
	}
}