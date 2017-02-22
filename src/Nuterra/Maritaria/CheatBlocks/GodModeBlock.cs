using System;
using UnityEngine;

namespace Maritaria.CheatBlocks
{
	public sealed class GodModeBlock : CustomBlock
	{
		public static readonly int BlockID = 9003;

		public int CustomBlock.BlockID => ID;
		public string Name => "Godmode Block";
		public string Description => "Protects all attached blocks from all incomming damage";
		public BlockCategories Category => BlockCategories.Accessories;
		public FactionSubTypes Faction => FactionSubTypes.EXP;
		public Sprite DisplaySprite => null;

		public GodModeBlock()
		{
		}

	}
}