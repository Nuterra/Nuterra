using System;
using UnityEngine;

namespace Gameslynx
{
	public class SuicideKeyBehaviour : MonoBehaviour
	{
		public Update()
		{
			Tank player = Singleton.playerTank;
			if (player != null && Input.GetKeyDown(Maritaria.Mod.Config.SuicideKey))
			{
				KillTech(player);
			}
		}
		
		public static void KillTech(Tech target)
		{
			if (target == null) throw new ArgumentNullException(nameof(target));
			BlockManager blockManager = player.blockman;
			foreach(ModuleTechController controller in blockManager.IterateBlockComponents<ModuleTechController>())
			{
				controller.block.damage.SelfDestruct(1f);
			}
		}
	}
}