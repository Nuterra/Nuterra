using System;
using UnityEngine;

namespace Maritaria
{
	public class TimeOfDayKeysBehaviour : MonoBehaviour
	{
		public void Update()
		{
			if (Input.GetKeyDown(Mod.Config.TurnDayKey))
			{
				Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(12, 0, 0);
			}
			if (Input.GetKeyDown(Mod.Config.TurnNightKey))
			{
				Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(0, 0, 0);
			}
		}
	}
}