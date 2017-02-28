using System;

namespace Maritaria.CheatBlocks
{
	public sealed class ModuleGodMode : Module
	{
		private void OnPool()
		{
			block.AttachEvent += OnAttach;
			block.DetachEvent += OnDetach;
		}

		private void OnSpawn()
		{
		}

		private void OnRecycle()
		{
		}

		private void OnAttach()
		{
			foreach (Damageable dmg in block.tank.blockman.IterateBlockComponents<Damageable>())
			{
				dmg.SetInvulnerable(true, true);
				dmg.InitHealth(Damageable.kRefillHealth);
			}
		}

		private void OnDetach()
		{
			if (!HasAnotherGodModule())
			{
				block.tank.SetInvulnerable(false, true);
			}
		}

		private bool HasAnotherGodModule()
		{
			foreach (TankBlock otherBlock in block.tank.blockman.IterateBlocks())
			{
				var otherGodModule = otherBlock.GetComponent<ModuleGodMode>();
				if ((otherGodModule != null) && (this != otherGodModule))
				{
					return true;
				}
			}
			return false;
		}
	}
}