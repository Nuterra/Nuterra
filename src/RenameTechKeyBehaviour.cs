using System;
using UnityEngine;

namespace Maritaria
{
	public class RenameTechKeyBehaviour : MonoBehaviour
	{
		public void Update()
		{
			if (Input.GetKeyDown((KeyCode)107))
			{
				Tank tank = Singleton.Manager<ManTechs>.inst.IterateTechs().FirstOrDefault(new Func<Tank, bool>(this.IsPlayerControlledTech));
				if (tank != null)
				{
					UIScreenRenameTech.ShowForTank(tank);
				}
			}
		}

		public RenameTechKeyBehaviour()
		{
		}

		public bool IsPlayerControlledTech(Tank tank)
		{
			return tank != null && tank.IsPlayer;
		}
	}
}
