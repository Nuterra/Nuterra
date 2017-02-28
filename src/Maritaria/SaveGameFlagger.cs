using System;
using UnityEngine;

namespace Maritaria
{
	public static class SaveGameFlagger
	{
		//Hook at beginning of method, passing this
		public static void ManSaveGame_State_ctor(ManSaveGame.State state)
		{
			state.m_OverlayData = "Save loaded by modded game";
		}
		//Hook at beginning of method, passing m_State
		public static void ManSaveGame_SaveData_OnDeserialized(ManSaveGame.SaveData saveData, bool loadInfoOnly)
		{
			if (saveData.m_State != null)
			{
				ManSaveGame_State_ctor(saveData.m_State);
			}
		}
	}
}