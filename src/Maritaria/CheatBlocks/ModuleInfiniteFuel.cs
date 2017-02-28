using System;
using UnityEngine;
namespace Maritaria.CheatBlocks
{
	public sealed class ModuleInfiniteFuel : ModuleFuelTank
	{
		public ModuleInfiniteFuel()
		{
			base.m_MaterialsToUpdate = new Material[0];
		}

		public override float Capacity => 1000;
		public override float RefillRate => Capacity * 10;
	}
}