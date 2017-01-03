using System;

namespace Sylver
{
	public class SylverSpawn
	{
		public void SpawnSylverEnemy(string presetName)
		{
			Console.Write("Spawn demand detected");
			TankPreset presetFromName = Singleton.Manager<ManSpawn>.inst.GetPresetFromName(presetName);
			if (presetFromName != null)
			{
				Singleton.Manager<ManSpawn>.inst.TestSpawnTank(presetFromName.m_TechData, ManSpawn.NewEnemyTeam, false);
			}
		}

		public SylverSpawn()
		{
		}
	}
}
