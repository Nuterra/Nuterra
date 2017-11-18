using System;
using System.Threading;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Nuterra;
using Nuterra.UI;

namespace Sylver.AdvancedBuilding
{
    class RotateBlocks : MonoBehaviour
    {
        private int ID = Utils.GetWindowID();

        private bool visible = false;

        private TankBlock block;

        private float x=0, y=0, z=0,posX,posY;

        private Rect win;

        private void Start()
        {
           // Singleton.Manager<ManPointer>.inst.MouseEvent += Inst_MouseEvent;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(2))
            {
                posX = Input.mousePosition.x;
                posY = Input.mousePosition.y;
                win = new Rect(posX - 200f, posY, 200f, 200f);
                try
                {
                    block = Singleton.Manager<ManPointer>.inst.targetVisible.block;
                    x = block.trans.localRotation.eulerAngles.x;
                    y = block.trans.localRotation.eulerAngles.y;
                    z = block.trans.localRotation.eulerAngles.z;
                    //Console.WriteLine(block.trans.rotation.eulerAngles);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                    block = null;
                }
                visible = block;
            }
        }

        /*private void Inst_MouseEvent(ManPointer.Event arg1, bool arg2, bool arg3)
        {
            if (arg1 == ManPointer.Event.MMB && arg2 && Singleton.Manager<ManPointer>.inst.targetVisible.block)
            {
                
            }
        }*/

        private void OnGUI()
        {
            if (!visible||!block) return;
            GUI.skin = NuterraGUI.Skin;/*.window = new GUIStyle(GUI.skin.window)
            {
                normal =
            {
                background = NuterraGUI.LoadImage("Border_BG.png"),
                textColor = Color.white
            },
                border = new RectOffset(16, 16, 16, 16),
            };
            GUI.skin.label.margin.top = 5;
            GUI.skin.label.margin.bottom = 5;*/
            try
            {
                win = GUI.Window(ID, win, new GUI.WindowFunction(DoWindow), "Block Rotation");
                block.trans.localRotation = Quaternion.Euler(x, y, z);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void DoWindow(int id)
        {
            GUILayout.Label("X rotation");
            float.TryParse(GUILayout.TextField(x.ToString()),out x);

            GUILayout.Label("Y rotation");
            float.TryParse(GUILayout.TextField(y.ToString()), out y);

            GUILayout.Label("Z rotation");
            float.TryParse(GUILayout.TextField(z.ToString()), out z);

            //GUILayout.Label(block.cachedLocalRotation.ToString());

            if (GUILayout.Button("Close"))
            {
                visible = false;
                block = null;
            }
            GUI.DragWindow();
        }
    }
}
