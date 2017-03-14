using System;

namespace Sylver
{
	public class SylverSpawn : Singleton.Manager<SylverSpawn>
	{
		public void SpawnSylverEnemy(string presetName)
		{
			TankPreset presetFromName = this.GetPresetFromName(presetName);
			if (presetFromName != null)
			{
				Singleton.Manager<ManSpawn>.inst.TestSpawnTank(presetFromName.m_TechData, ManSpawn.NewEnemyTeam, false);
				return;
			}
			Console.WriteLine("ManSpawn.TestSpawnFriendlyTech - no preset found with name " + presetName);
		}

		public SylverSpawn()
		{
		}

		private TankPreset GetPresetFromName(string presetName)
		{
			TankPreset result = null;
			Singleton.Manager<ManSpawn>.inst.TankPresets.TryGetValue(presetName.ToLower(), out result);
			return result;
		}
	}
}