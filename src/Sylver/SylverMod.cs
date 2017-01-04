using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

		private void Update()
		{
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
				SylverMod.m_EnemyAIName = "Gauntlet_01";
			}
			if (this.num == 1)
			{
				SylverMod.m_FriendlyAIName = "Cannon";
				SylverMod.m_EnemyAIName = "Cannon";
			}
			if (this.num == 2)
			{
				SylverMod.m_FriendlyAIName = "FTUE";
				SylverMod.m_EnemyAIName = "FTUE";
			}
			if (this.num == 3)
			{
				SylverMod.m_FriendlyAIName = "LiftOffCab";
				SylverMod.m_EnemyAIName = "LiftOffCab";
			}
			if (this.num == 4)
			{
				SylverMod.m_FriendlyAIName = "VENLiftOffCab";
				SylverMod.m_EnemyAIName = "VENLiftOffCab";
			}
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
			{
				Singleton.Manager<ManSpawn>.inst.TestSpawnFriendlyTech(SylverMod.m_FriendlyAIName);
			}
			if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Q))
			{
				Singleton.Manager<SylverSpawn>.inst.SpawnSylverEnemy(SylverMod.m_EnemyAIName);
			}
			if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.E))
			{
				Singleton.Manager<ManPop>.inst.DebugForceSpawn();
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

		public static string EnemyAIName
		{
			get
			{
				return SylverMod.m_EnemyAIName;
			}
			set
			{
				SylverMod.m_EnemyAIName = value;
			}
		}

		private static string m_FriendlyAIName = "FTUE";

		private DebugUtil.KeySequence m_DebugAISpawn;

		private static string m_EnemyAIName = "FTUEAI";

		private int num;

		

		}
	}
}

