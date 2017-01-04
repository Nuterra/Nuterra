using System;
using UnityEngine;

namespace Maritaria
{
	public class SplashScreenHandler : MonoBehaviour
	{
		
		public ManSplashScreen _splashScreen = Singleton.Manager<ManSplashScreen>.inst;
		
		public GUIContent _content = new GUIContent();

		public GUIStyle _style = new GUIStyle();

		public string _contentFormat = "<color=#eeeeee><size=20>Mod by <color=#{0}>Maritaria</color> and <color=#{1}>Sylver</color></size></color>";

		public SplashScreenHandler()
		{
			_style.richText = true;
			_style.alignment = TextAnchor.MiddleCenter;
		}
		
		public static void Init()
		{
			Singleton.Manager<ManSplashScreen>.inst.gameObject.AddComponent<SplashScreenHandler>();
		}
		
		public void OnGUI()
		{
			if (CanDraw())
			{
				string colorMaritaria = GetFormattedColor(GetRainbowColor(0f));
				string colorSylver = GetFormattedColor(GetRainbowColor(1f));
				_content.text = string.Format(_contentFormat, colorMaritaria, colorSylver);
				Vector2 vector = new Vector2(Screen.width * 0.5f, Screen.height * 0.8f);
				Vector2 vector2 = _style.CalcSize(_content);
				vector.x -= vector2.x * 0.5f;
				GUI.Label(new Rect(vector, vector2), _content, _style);
			}
		}
		
		public bool CanDraw()
		{
			if (_splashScreen == null) return false;
			if (_splashScreen.m_MyCanvas == null) return false;
			if (!_splashScreen.m_MyCanvas.gameObject.active) return false;
			if (_splashScreen.m_SplashScreenIndex != 0) return false;
			return true;
		}
		
		public Canvas FindCanvas()
		{
			return _splashScreen.CanvasTrans.GetComponent<Canvas>();
		}

		public static Color GetRainbowColor(float offset)
		{
			return Color.HSVToRGB(Mathf.Sin(Time.time - offset) * 0.5f + 0.5f, 1f, 1f);
		}

		public static string GetFormattedColor(Color color)
		{
			byte b = (byte)(color.r * 255f);
			byte b2 = (byte)(color.g * 255f);
			byte b3 = (byte)(color.b * 255f);
			return string.Format("{0:X2}{1:X2}{2:X2}", b, b2, b3);
		}
	}
}
