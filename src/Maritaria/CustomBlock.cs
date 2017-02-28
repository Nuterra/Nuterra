using System;
using UnityEngine;

namespace Maritaria
{
	public interface CustomBlock
	{
		int BlockID { get; }
		string Name { get; }
		string Description { get; }
		int Price { get; }
		FactionSubTypes Faction { get; }
		BlockCategories Category { get; }
		GameObject Prefab { get; }
		Sprite DisplaySprite { get; }
	}
}