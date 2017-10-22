using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Sylver.AdvancedBuilding
{
    class ScaleBlocks : MonoBehaviour
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
                win = new Rect(posX + 200f, posY, 200f, 200f);
                try
                {
                    block = Singleton.Manager<ManPointer>.inst.targetVisible.block;
                    x = block.trans.localScale.x;
                    y = block.trans.localScale.y;
                    z = block.trans.localScale.z;
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
                win = GUI.Window(int.MinValue + 2, win, new GUI.WindowFunction(DoWindow), "Block scale");
                block.trans.localScale = new Vector3(x, y, z);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void DoWindow(int id)
        {
            GUILayout.Label("X scale");
            float.TryParse(GUILayout.TextField(x.ToString()), out x);

            GUILayout.Label("Y scale");
            float.TryParse(GUILayout.TextField(y.ToString()), out y);

            GUILayout.Label("Z scale");
            float.TryParse(GUILayout.TextField(z.ToString()), out z);
            if (GUILayout.Button("Close"))
            {
                visible = false;
                block = null;
            }
            GUI.DragWindow();
        }
    }
}
