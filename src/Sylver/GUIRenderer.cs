using System;
using UnityEngine;

namespace Sylver
{
	public class GUIRenderer : MonoBehaviour
	{
		public GUIRenderer()
		{
			this._style = new GUIStyle();
			this._style.richText = true;
			this._style.alignment = TextAnchor.MiddleCenter;
			this._content = new GUIContent();
		}

		public void OnGUI()
		{
			if (!Singleton.playerTank)
			{
				return;
			}
			if (!Singleton.Manager<DebugUtil>.inst.hideGUI)
			{
			this._content.text = string.Format(GUIRenderer.DisplayFormat, SylverMod.FriendlyAIName);
			this._style.fontSize = Screen.height / 60;
			Vector2 position = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.925f);
			Vector2 vector = this._style.CalcSize(this._content);
			position.y -= vector.y * 0.5f;
			position.x -= vector.x * 0.5f;
			GUI.Label(new Rect(position, vector), this._content, this._style);
			}
		}

		static GUIRenderer()
		{
		}

		private GUIContent _content;

		private GUIStyle _style;

		public static readonly string DisplayFormat = "{0}<color=white><b> will be spawned</b></color>";
	}
}
