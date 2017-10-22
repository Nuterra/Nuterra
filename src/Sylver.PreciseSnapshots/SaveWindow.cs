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
    class SaveWindow : MonoBehaviour
    {
        private int ID = Utils.GetWindowID();

        private string path;

        private bool visible = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftAlt) && Singleton.playerTank)
            {
                path = Path.Combine(PreciseSnapshotsMod.PreciseSnapshotsFolder, Singleton.playerTank.name + ".xml");
                if(File.Exists(path))
                {
                    visible = true;
                    return;
                }
                XMLSave.SaveTechAsXML(Singleton.playerTank, PreciseSnapshotsMod.PreciseSnapshotsFolder);
            }
        }

        private void OnGUI()
        {
            if (!visible) return;
            GUI.Window(ID, new Rect((Screen.width - 700f) / 2, (Screen.height - 200f) / 2, 700f, 200f), new GUI.WindowFunction(DoWindow), "");
        }

        private void DoWindow(int id)
        {
            GUILayout.Label(new GUIContent("<color=yellow>WARNING</color>"), new GUIStyle { richText = true, alignment = TextAnchor.MiddleCenter, fontSize = 32 });
            GUILayout.Label("The path \"" + path + "\" already exists.\n Do you want to replace it ?");
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Cancel")) visible = false;
            if(GUILayout.Button("Overwrite"))
            {
                visible = false;
                XMLSave.SaveTechAsXML(Singleton.playerTank, PreciseSnapshotsMod.PreciseSnapshotsFolder);
            }
            GUILayout.EndHorizontal();
        }
    }
}
