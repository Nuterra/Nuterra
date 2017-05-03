using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuterra
{
	public static class Extensions
	{
		public static IEnumerable<TerrainObject> GetVanillaTerrainPrefabs(this ManSpawn inst)
		{
			return inst.m_TerrainObjectTable.m_GUIDToPrefabLookup.Values;
		}
	}
}
