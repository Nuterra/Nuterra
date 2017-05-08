using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maritaria.Blueprints
{
	public sealed class CompressedTechContainer : Module
	{
		private TechData _savedTech = new TechData();

		private void OnPool()
		{
			block.serializeEvent.Subscribe(new Action<bool, TankPreset.BlockSpec>(this.OnSerialize));
		}

		private void OnSerialize(bool saving, TankPreset.BlockSpec spec)
		{
			if (saving)
			{
				var serialData = new ContainerData
				{
					SavedTech = _savedTech,
				};
				serialData.Store(spec.saveState);
			}
			else
			{
				var data = SerialData<ContainerData>.Retrieve(spec.saveState);
				_savedTech = data.SavedTech;
			}
		}

		private sealed class ContainerData : SerialData<ContainerData>
		{
			public TechData SavedTech { get; set; }
		}

		private void OnSpawn()
		{
		}

		private void OnRecycle()
		{
		}

		public static TankBlock CompressTech(Tank player)
		{
			var block = ManSpawn.inst.SpawnBlock(BlueprintsMod.CompressedTechBlockID, player.trans.position, player.trans.rotation);
			var container = block.GetComponent<CompressedTechContainer>();
			container.StoreTech(player);
			return block;
		}

		public void StoreTech(Tank player)
		{
			_savedTech.SaveTech(player, saveRuntimeState: true, saveMetaData: false);
			player.RemoveFromGame();
		}

		public void UnloadTech()
		{
			SpawnTech();
			SelfDestruct();
		}

		private void SpawnTech()
		{
			if (_savedTech != null)
			{
				ManSpawn.inst.SpawnTank(new ManSpawn.TankSpawnParams
				{
					blockIDs = null,
					forceSpawn = true,
					grounded = true,
					placement = ManSpawn.TankSpawnParams.Placement.BoundsCentredAtPosition,
					position = Singleton.Manager<ManWorld>.inst.ProjectToGround(transform.position, true),
					rotation = Quaternion.Euler(0f, Singleton.cameraTrans.rotation.eulerAngles.y, 0f),
					teamID = ManSpawn.PlayerTeam,
					techData = _savedTech,
				}, addToObjectManager: true);
			}
		}

		private void SelfDestruct()
		{
			block.RemoveFromGame();
		}
	}
}