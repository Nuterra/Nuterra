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

		private static string m_EnemyAIName = "FTUE";

		private int num;

		public class KeySequence
		{
			public KeySequence(string keySequence, float interval)
			{
				IEnumerable<char> arg_2C_0 = keySequence.ToUpper();
				Func<char, KeyCode> arg_2C_1;
				if ((arg_2C_1 = SylverMod.KeySequence.<>c.<>9__0_0) == null)
				{
					arg_2C_1 = (SylverMod.KeySequence.<>c.<>9__0_0 = new Func<char, KeyCode>(SylverMod.KeySequence.<>c.<>9.<.ctor>b__0_0));
				}
				this.keyCodes = arg_2C_0.Select(arg_2C_1).ToArray<KeyCode>();
				this.currentIndex = 0;
				this.maxInterval = interval;
			}

			public bool Completed()
			{
				if (Time.time > this.lastAcceptTime + this.maxInterval)
				{
					this.currentIndex = 0;
				}
				bool flag = false;
				if (Input.GetKeyDown(this.keyCodes[this.currentIndex]))
				{
					this.currentIndex++;
					this.lastAcceptTime = Time.time;
					flag = true;
					if (this.currentIndex == this.keyCodes.Length)
					{
						this.currentIndex = 0;
						AudioSource component = Singleton.Manager<DebugUtil>.inst.GetComponent<AudioSource>();
						if (component)
						{
							component.Play();
						}
						return true;
					}
				}
				if (Input.anyKeyDown && !flag)
				{
					this.currentIndex = 0;
				}
				return false;
			}

			private KeyCode[] keyCodes;

			private int currentIndex;

			private float maxInterval;

			private float lastAcceptTime;

			[CompilerGenerated]
			[Serializable]
			private sealed class <>c
			{
				static <>c()
				{
					// Note: this type is marked as 'beforefieldinit'.
				}

				public <>c()
				{
				}

				internal KeyCode <.ctor>b__0_0(char c)
				{
					return (KeyCode)((int)Enum.Parse(typeof(KeyCode), c.ToString()));
				}

				public static readonly SylverMod.KeySequence.<>c <>9 = new SylverMod.KeySequence.<>c();

				public static Func<char, KeyCode> <>9__0_0;
			}
		}
	}
}
