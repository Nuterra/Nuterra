using System;
using UnityEngine;

namespace Maritaria.ExtraKeys
{
	public class SuicideKeyBehaviour : MonoBehaviour
	{
		public ExtraKeysMod Mod { get; set; }
		public void Update()
		{
			Tank player = Singleton.playerTank;
			if (player != null && Input.GetKeyDown(Mod.KeyConfig.SuicideKey))
			{
				KillTech(player);
			}
		}

		public static void KillTech(Tank target)
		{
			if (target == null) throw new ArgumentNullException(nameof(target));
			BlockManager blockManager = target.blockman;
			foreach (ModuleTechController controller in blockManager.IterateBlockComponents<ModuleTechController>())
			{
				controller.block.damage.SelfDestruct(1f);
			}
		}
	}
}