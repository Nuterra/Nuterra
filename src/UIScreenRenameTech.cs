using System;
using UnityEngine;

namespace Maritaria
{
	public class UIScreenRenameTech : UIScreen
	{
		private string m_UserInput;

		public Tank m_Target;

		public static void ShowForTank(Tank target)
		{
			UIScreenRenameTech uIScreenRenameTech = new UIScreenRenameTech();
			uIScreenRenameTech.m_Target = target;
			uIScreenRenameTech.m_UserInput = target.name;
			Console.WriteLine("Renaming tech: " + target.name);
			Singleton.Manager<ManUI>.inst.PushScreen(uIScreenRenameTech, ManUI.PauseType.None);
			UIScreenRenameTech.Screen_equals_null_because_missing_gameobject();
		}

		public UIScreenRenameTech()
		{
			this.m_UserInput = "MyTechName";
		}

		public void OnGUI()
		{
			SplashScreenHandler._content.text=(string.Format(SplashScreenHandler._contentFormat, SplashScreenHandler.GetFormattedColor(SplashScreenHandler.GetRainbowColor())));
			Vector2 vector = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.8f);
			Vector2 vector2=new Vector2(300f, 60f);
			vector.x -= vector2.x / 2f;
			vector.y -= vector2.y / 2f;
			this.m_UserInput = GUI.TextField(new Rect(vector, vector2), this.m_UserInput, 20);
			vector.y += vector2.y + 10f;
			if (GUI.Button(new Rect(vector, vector2), "Apply"))
			{
				this.ApplyName();
			}
		}

		public void ApplyName()
		{
			this.m_Target.name=(this.m_UserInput);
		}

		public static void Screen_equals_null_because_missing_gameobject()
		{
		}
	}
}
