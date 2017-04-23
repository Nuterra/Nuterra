using System;
using UnityEngine;

namespace Nuterra
{
	public sealed class CustomBlock
	{
		public int BlockID { get; internal set; }
		public string Name { get; internal set; }
		public string Description { get; internal set; }
		public int Price { get; internal set; }
		public FactionSubTypes Faction { get; internal set; } = FactionSubTypes.EXP;
		public BlockCategories Category { get; internal set; } = BlockCategories.Standard;
		public GameObject Prefab { get; internal set; }
		public Sprite DisplaySprite { get; internal set; }

	}
}