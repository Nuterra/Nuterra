using System;
using UnityEngine;

namespace Maritaria
{
	public class TimeOfDayKeysBehaviour : MonoBehaviour
	{
		public void Update()
		{
			if (Input.GetKeyDown(UnityGraph.Config.TurnDayKey))
			{
				Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(12, 0, 0);
			}
			if (Input.GetKeyDown(UnityGraph.Config.TurnNightKey))
			{
				Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(0, 0, 0);
			}
		}
	}
}