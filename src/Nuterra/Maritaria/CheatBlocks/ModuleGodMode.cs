using System;

namespace Maritaria.CheatBlocks
{
	public sealed class ModuleGodMode : Module
	{
		private ComponentPool.PoolDelegates OnPool()
		{
			block.AttachEvent += OnAttach;
			block.DetachEvent += OnDetach;
			return new ComponentPool.PoolDelegates(OnSpawn, OnRecycle);
		}

		private void OnSpawn()
		{
		}

		private void OnRecycle()
		{
		}

		private void OnAttach()
		{
			foreach (TankBlock block in block.tank.blockman.IterateBlocks())
			{
				block.visible.damageable.SetInvulnerable(true, true);
			}
		}

		private void OnDetach()
		{
			bool retainGodmode = false;
			foreach (TankBlock block in block.tank.blockman.IterateBlocks())
			{
				var otherBlock = block.GetComponent<ModuleGodMode>();
				if (otherBlock != this)
				{
					retainGodmode = true;
					break;
				}
			}
			if (!retainGodmode)
			{
				block.visible.damageable.SetInvulnerable(false, true);
			}
		}
	}
}