using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Nuterra
{
	public static class Extensions
	{
		public static IEnumerable<TerrainObject> GetVanillaTerrainPrefabs(this ManSpawn inst)
		{
			return inst.m_TerrainObjectTable.m_GUIDToPrefabLookup.Values;
		}

		public static bool IsScenery(this GameObject obj)
		{
			return obj.layer == Globals.inst.layerScenery;
		}
	}
}
