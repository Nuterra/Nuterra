using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maritaria
{
	public class ProductionToggleKeyBehaviour : Singleton.Manager<ProductionToggleKeyBehaviour>
	{
		private static readonly HashSet<BlockTypes> _scrappers = new HashSet<BlockTypes> { BlockTypes.GSOScrapper_322, BlockTypes.GCScrapper_432, BlockTypes.VENScrapper_212, BlockTypes.HE_Scrapper_323 };
		public bool ProductionActive = true;

		public void Update()
		{
			if (Input.GetKeyDown(UnityGraph.Config.ProductionToggleKey))
			{
				ProductionActive = !ProductionActive;
				foreach (Tank tank in Singleton.Manager<ManTechs>.inst.IterateTechsWhere(IsTankOnPlayerTeam))
				{
					DisableHeart(tank);
					ApplyProductionToggleToScrappers(tank);
				}
			}
		}

		private bool IsTankOnPlayerTeam(Tank tank)
		{
			return tank.Team == ManSpawn.PlayerTeam;
		}

		private void DisableHeart(Tank tank)
		{
			foreach (ModuleHeart heart in tank.blockman.IterateBlockComponents<ModuleHeart>())
			{
				heart.DropAllItems();//Make internal
				heart.gameObject.EnsureComponent<ProductionStoppedSign>();
			}
		}

		private void ApplyProductionToggleToScrappers(Tank tank)
		{
			foreach (ModuleItemPickup pickup in tank.blockman.IterateBlockComponents<ModuleItemPickup>())
			{
				ModuleItemPickup_ApplyProductionToggle(pickup);
			}
		}

		public static class Hooks_ModuleItemPickup
		{
			//Hook before method body
			public static void OnSpawn(ModuleItemPickup pickup)
			{
				Singleton.Manager<ProductionToggleKeyBehaviour>.inst.ModuleItemPickup_ApplyProductionToggle(pickup);
			}

			//Hook by replacing assignment of IsEnabled
			public static void OnAttach(ModuleItemPickup pickup)
			{
				Singleton.Manager<ProductionToggleKeyBehaviour>.inst.ModuleItemPickup_ApplyProductionToggle(pickup);
			}
		}

		public static class Hooks_ModuleHeart
		{
			//Hook by &&-ing result of this function
			public static bool get_IsOnline(ModuleHeart heart)
			{
				return Singleton.Manager<ProductionToggleKeyBehaviour>.inst.ProductionActive;
			}
		}

		private void ModuleItemPickup_ApplyProductionToggle(ModuleItemPickup pickup)
		{
			if (pickup.block.tank && IsTankOnPlayerTeam(pickup.block.tank) && _scrappers.Contains(pickup.block.BlockType))
			{
				pickup.IsEnabled = ProductionActive;
				if (!ProductionActive)
				{
					pickup.m_Holder.DropAll();//Make internal
				}
				pickup.gameObject.EnsureComponent<ProductionStoppedSign>();
			}
		}
	}
}