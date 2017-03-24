using System;

namespace Maritaria.ExtraBlocks
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
				EnableBlockGodMode(dmg);
			}
			block.tank.AttachEvent.Subscribe(OnNewBlockAttached);
			block.tank.DetachEvent.Subscribe(OnBlockRemoved);
		}

		private void OnDetach()
		{
			if (!HasAnotherGodModule(block.tank))
			{
				block.tank.SetInvulnerable(false, true);
				block.tank.AttachEvent.Unsubscribe(OnNewBlockAttached);
				block.tank.DetachEvent.Unsubscribe(OnNewBlockAttached);
			}
		}

		private static void EnableBlockGodMode(Damageable dmg)
		{
			dmg.SetInvulnerable(true, true);
			dmg.InitHealth(Damageable.kRefillHealth);
		}

		private void OnNewBlockAttached(TankBlock otherblock, Tank tank)
		{
			if (tank != block.tank)
			{
				throw new InvalidOperationException("This shouldnt happen");
			}
			EnableBlockGodMode(otherblock.GetComponent<Damageable>());
		}

		private void OnBlockRemoved(TankBlock otherblock, Tank tank)
		{
			if (!HasAnotherGodModule(tank))
			{
				otherblock.GetComponent<Damageable>().SetInvulnerable(false, false);
			}
		}

		private bool HasAnotherGodModule(Tank tank)
		{
			foreach (TankBlock otherBlock in tank.blockman.IterateBlocks())
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