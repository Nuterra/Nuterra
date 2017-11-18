using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Nuterra;

namespace Utilities
{
    class CurrentSaveInfo : MonoBehaviour
    {
        private bool visible = false;

        private int ID = Utils.GetWindowID();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad2)) visible = !visible;
        }

        private void OnGUI()
        {
            if (!visible || Singleton.Manager<ManSaveGame>.inst.m_SaveData==null) return;

            try
            { 
            GUI.Window(ID, new Rect(300f, 0, 300f, 300f), new GUI.WindowFunction(DoWindow), "Current Save Infos");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void DoWindow(int id)
        {
            var save = Singleton.Manager<ManSaveGame>.inst.m_SaveData;
            GUILayout.Label("Name : " + save.SaveInfo.m_SaveName);
            GUILayout.Label("Money : " + save.SaveInfo.m_Money);
            GUILayout.Label("Played Time : " + save.SaveInfo.m_TimePlayed.ToString());
            GUILayout.Label("Seed : " + save.SaveInfo.m_WorldSeed);
            GUILayout.Label("Last Screenshot");

            GUIStyle centered = new GUIStyle(GUI.skin.GetStyle("Label"));
            centered.alignment = TextAnchor.UpperCenter;
            GUILayout.Label(save.SaveInfo.LastScreenshot, centered, GUILayout.Width(100f),GUILayout.Height(100f));
            GUI.DragWindow(new Rect(0,0,300f,20f));
        }
    }
}
