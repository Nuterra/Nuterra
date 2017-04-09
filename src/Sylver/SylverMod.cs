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
				if (this.num >= techNames.Lenght)
				{
					this.num = 0;
				
				}
				SylverMod.m_FriendlyAIName = techNames[num];
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
			int num = 0;
			this.techNames = this.GetTankNames();
			string[] array = this.techNames;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				this.techNames[num] = text.Split(new char[]
				{
					'/'
				}).Last<string>();
				num++;
			}
		}

		public string[] GetTankNames()
		{
			return Singleton.Manager<ManSpawn>.inst.tankPresetList.files.ToArray();
		}
		public string[] techNames;

		private static string m_FriendlyAIName;

		private int num;
	}
}
