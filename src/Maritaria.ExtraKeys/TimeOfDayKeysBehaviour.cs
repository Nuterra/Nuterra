using System;
using UnityEngine;

namespace Maritaria.ExtraKeys
{
	public class TimeOfDayKeysBehaviour : MonoBehaviour
	{
		public ExtraKeysMod Mod { get; set; }
		public void Update()
		{
			if (Input.GetKeyDown(Mod.KeyConfig.TurnDayKey))
			{
				Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(12, 0, 0);
			}
			if (Input.GetKeyDown(Mod.KeyConfig.TurnNightKey))
			{
				Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(0, 0, 0);
			}
		}
	}
}