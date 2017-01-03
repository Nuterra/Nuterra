using System;
using UnityEngine;

namespace Maritaria
{
	public class FirstPersonKeyBehaviour : Singleton.Manager<FirstPersonKeyBehaviour>
	{
		public bool FirstPersonEnabled { get; set; } = false;
		public TankBlock ActiveCameraBlock;
		
		public void Update()
		{
			if (Input.GetKeyDown(Mod.Config.FirstPersonKey))
			{
				FirstPersonEnabled = !FirstPersonEnabled;
			}
		}

		//Hook this function in the middle, before the last if statement that applies the angles to the camera singleton
		//Returns true if the default behaviour should be executed, otherwise false
		//http://prntscr.com/dqkleo
		public bool TankCamera_FixedUpdate()
		{
			this.SetActiveBlockVisibility(true);
			Tank tank = Singleton.playerTank;
			if (tank == null)
			{
				return true;
			}
			if (tank.beam && tank.beam.IsActive)
			{
				this.FirstPersonEnabled = false;
			}
			if (!this.FirstPersonEnabled)
			{
				return true;
			}
			this.ActiveCameraBlock = null;
			if (this.ActiveCameraBlock == null)
			{
				bool found = false;
				BlockManager.BlockIterator<TankBlock>.Enumerator enumerator = tank.blockman.IterateBlocks().GetEnumerator();
				while (enumerator.MoveNext())
				{
					TankBlock block = enumerator.Current;
					if (block.BlockType == (BlockTypes)SmileyBlock.BlockID)
					{
						this.ActiveCameraBlock = block;
						found = true;
						break;
					}
				}
				if (!found)
				{
					return true;
				}
			}
			Singleton.cameraTrans.position = this.ActiveCameraBlock.tank.transform.TransformPoint(this.ActiveCameraBlock.cachedLocalPosition + new Vector3(0f, 0f, 0f));
			Singleton.cameraTrans.rotation = this.ActiveCameraBlock.tank.transform.rotation;
			this.SetActiveBlockVisibility(false);
			return false;
		}
		
		private void SetActiveBlockVisibility(bool visible)
		{
			if (this.ActiveCameraBlock)
			{
				MeshRenderer[] renderers = this.ActiveCameraBlock.GetComponentsInChildren<MeshRenderer>();
				for (int i = 0; i < renderers.Length;i++)
				{
					renderers[i].enabled = visible;
				}
			}
		}
	}
}
