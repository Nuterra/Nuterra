using System;
using UnityEngine;

namespace Maritaria
{
	public static class SaveGameFlagger
	{
		//Hook at beginning of method, passing this
		public static void ManSaveGame_State_ctor(ManSaveGame.State state)
		{
			state.m_OverlayData = "This game is modded";
		}
		//Hook at beginning of method, passing this.m_State
		public static void ManSaveGame_SaveData_OnDeserialized(ManSaveGame.State state)
		{
			if (state != null)
			{
				ManSaveGame_State_ctor(state);
			}
		}
	}
}