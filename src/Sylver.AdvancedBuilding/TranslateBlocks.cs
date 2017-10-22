using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Sylver.AdvancedBuilding
{
    class TranslateBlocks : MonoBehaviour
    {
        private bool visible = false;

        private TankBlock block;

        private float x = 0, y = 0, z = 0, posX, posY;

        private Rect win;

        private void Start()
        {
            // Singleton.Manager<ManPointer>.inst.MouseEvent += Inst_MouseEvent;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {
                posX = Input.mousePosition.x;
                posY = Input.mousePosition.y;
                win = new Rect(posX, posY, 200f, 200f);
                try
                {
                    block = Singleton.Manager<ManPointer>.inst.targetVisible.block;
                    x = block.trans.position.x;
                    y = block.trans.position.y;
                    z = block.trans.position.z;
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
            if (!visible || !block) return;
            GUI.skin.label.margin.top = 5;
            GUI.skin.label.margin.bottom = 5;
            try
            {
                win = GUI.Window(int.MinValue+1, win, new GUI.WindowFunction(DoWindow), "Block position");
                block.trans.position = new Vector3(x, y, z);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void DoWindow(int id)
        {
            GUILayout.Label("X position");
            float.TryParse(GUILayout.TextField(x.ToString()), out x);

            GUILayout.Label("Y position");
            float.TryParse(GUILayout.TextField(y.ToString()), out y);

            GUILayout.Label("Z position");
            float.TryParse(GUILayout.TextField(z.ToString()), out z);

            //GUILayout.Label(((IntVector3)block.trans.localPosition).ToString());

            if (GUILayout.Button("Close"))
            {
                visible = false;
                block = null;
            }
            GUI.DragWindow();
        }
    }
}
