using System;
using System.Reflection;
using UnityEngine;

namespace Maritaria.CheatBlocks
{
	public sealed class ModuleInfiniteFuel : ModuleFuelTank
	{
		public ModuleInfiniteFuel()
		{
			typeof(ModuleFuelTank).GetField("m_MaterialsToUpdate", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, new Material[0]);
		}

		public override float Capacity => 1000;
		public override float RefillRate => Capacity * 10;
	}
}