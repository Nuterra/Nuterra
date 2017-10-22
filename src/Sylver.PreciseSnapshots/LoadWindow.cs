using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuterra;
using UnityEngine;
using System.IO;

namespace Sylver.PreciseSnapshots
{
    class LoadWindow : MonoBehaviour
    {
        private int ID = Utils.GetWindowID();

        private bool visible = false;

        private Vector2 scrollPos = Vector2.zero;

        public Tank loadedTech;

        private string path = "";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftAlt) && Singleton.playerTank)
            {
                visible = true;
            }
        }

        private void OnGUI()
        {
            if (!visible) return;
            GUI.Window(ID, new Rect((Screen.width - 700f) / 2, (Screen.height - 500f) / 2, 700f, 500f), new GUI.WindowFunction(DoWindow), "Techs");
        }

        private void DoWindow(int id)
        {

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (var tech in Directory.GetFiles(PreciseSnapshotsMod.PreciseSnapshotsFolder))
            {
                if (GUILayout.Button(tech,new GUIStyle(GUI.skin.button) { richText = true, alignment = TextAnchor.MiddleLeft }))
                {
                    path = tech;
                }
            }
            GUILayout.EndScrollView();
            GUILayout.TextField(path);
            if (GUILayout.Button("Load"))
            {
                try
                {
                    Vector3 position = Singleton.playerTank.trans.position;
                    Quaternion rotation = Singleton.playerTank.trans.rotation;

                    loadedTech = XMLLoad.LoadXMLAsTech(path, position, rotation);

                    Tank playerTank = Singleton.playerTank;
                    Singleton.Manager<ManTechs>.inst.SetPlayerTank(null, true);
                    playerTank.RemoveFromGame();


                    Singleton.Manager<ManTechs>.inst.SetPlayerTank(loadedTech, true);
                }
                catch { }
                visible = false;
            }
            if (GUILayout.Button("Cancel")) visible = false;

        }
    }
}
