using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Nuterra;

namespace Utilities
{
    class PlayerInfo : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1)) visible = !visible;
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                WikiInfos.GetBlocksInfos();
                WikiInfos.GetResourcesInfos();
            }
        }

        private void OnGUI()
        {
            if (!visible || !Singleton.playerTank) return;

            try
            {
                GUI.Window(ID, new Rect(0, 0, 300f, 300f), new GUI.WindowFunction(DoWindow), "Player Tech Infos");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void DoWindow(int id)
        {
            GUILayout.Label("Name : " + Singleton.playerTank.name);
            GUILayout.Label("Position : " + Singleton.playerTank.trans.position.ToString());
            GUILayout.Label("Rotation : " + Singleton.playerTank.trans.rotation.eulerAngles.ToString());
            GUILayout.Label("Velocity : " + Singleton.playerTank.rbody.velocity.ToString());
            GUILayout.Label("Angular Velocity : " + Singleton.playerTank.rbody.angularVelocity.ToString());
            GUILayout.Label("Acceleration : " + Singleton.playerTank.acceleration.ToString());
            GUILayout.Label("Block Count : " + Singleton.playerTank.blockman.blockCount.ToString());
            GUILayout.Label("Size : " + Singleton.playerTank.visible.Radius.ToString());
            GUI.DragWindow();
        }

        public static bool visible = false;

        private int ID = Utils.GetWindowID();
    }
}
