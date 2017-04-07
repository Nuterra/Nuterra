using System;
using UnityEngine;

namespace Sylver
{
	public class SylverMod : MonoBehaviour
	{
		public static string FriendlyAIName
		{
			get
			{
				return SylverMod.m_FriendlyAIName;
			}
			set
			{
				SylverMod.m_FriendlyAIName = value;
			}
		}

		public static bool IsRandD => ManGameMode.inst.IsCurrent<ModeMisc>();

		private void Update()
		{
			if (!IsRandD)
			{
				//Only enable the behaviour in R&D mode
				return;
			}

			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Alpha1))
			{
				this.num++;
				if (this.num >= 5)
				{
					this.num = 0;
				}
			}
			if (this.num == 0)
			{
				SylverMod.m_FriendlyAIName = "Gauntlet_01";
			}
			if (this.num == 1)
			{
				SylverMod.m_FriendlyAIName = "Cannon";
			}
			if (this.num == 2)
			{
				SylverMod.m_FriendlyAIName = "FTUE";
			}
			if (this.num == 3)
			{
				SylverMod.m_FriendlyAIName = "LiftOffCab";
			}
			if (this.num == 4)
			{
				SylverMod.m_FriendlyAIName = "VENLiftOffCab";
			}
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
			{
				Singleton.Manager<ManSpawn>.inst.TestSpawnFriendlyTech(SylverMod.m_FriendlyAIName);
			}
			if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Q))
			{
				Singleton.Manager<SylverSpawn>.inst.SpawnSylverEnemy(SylverMod.m_FriendlyAIName);
			}
		}

		private void OnApplicationQuit()
		{
			d.isShuttingDown = true;
		}

		public SylverMod()
		{
		}

		static SylverMod()
		{
		}

		private static string m_FriendlyAIName = "FTUE";

		private int num;
	}
}