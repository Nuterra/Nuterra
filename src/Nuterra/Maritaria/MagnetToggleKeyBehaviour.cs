using System;
using UnityEngine;

namespace Maritaria
{
	public class MagnetToggleKeyBehaviour : MonoBehaviour
	{
		private GUIContent _content = new GUIContent();

		private GUIStyle _style = new GUIStyle();

		public static readonly string DisplayFormat = "<color=white><b>Magnets: </b>{0}</color>";

		public static readonly string OfflineStatus = "<color=#ff2828>OFFLINE</color>";

		public static readonly string OnlineStatus = "<color=#28ff28>ONLINE</color>";

		public void Update()
		{
			if (Input.GetKeyDown(MaritariaMod.Instance.Config.MagnetToggleKey))
			{
				Modules.Magnet.DisabledForPlayerControlledTank = !Modules.Magnet.DisabledForPlayerControlledTank;
			}
		}

		private void Start()
		{
			_style.richText = true;
			_style.alignment = TextAnchor.MiddleCenter;
		}

		public void OnGUI()
		{
			if (!Singleton.playerTank || (Singleton.playerTank.blockman.IterateBlockComponents<ModuleItemHolderMagnet>().FirstOrDefault() == null) || ManPauseGame.inst.IsPaused)
			{
				//No tank, magnet blocks or game is paused
				return;
			}
			if (Modules.Magnet.DisabledForPlayerControlledTank)
			{
				_content.text=(string.Format(MagnetToggleKeyBehaviour.DisplayFormat, MagnetToggleKeyBehaviour.OfflineStatus));
			}
			else
			{
				_content.text=(string.Format(MagnetToggleKeyBehaviour.DisplayFormat, MagnetToggleKeyBehaviour.OnlineStatus));
			}
			_style.fontSize=(Screen.height / 60);
			Vector2 vector=new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.95f);
			Vector2 vector2 = _style.CalcSize(_content);
			vector.y -= vector2.y * 0.5f;
			vector.x -= vector2.x * 0.5f;
			GUI.Label(new Rect(vector, vector2), _content, _style);
		}
	}
}
