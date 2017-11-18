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
    class BlocksInfo : MonoBehaviour
    {
        private int ID = Utils.GetWindowID();

        private bool visible = false;

        private TankBlock block;

        private float posX, posY;

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
                win = new Rect(posX - 400f, posY, 200f, 200f);
                try
                {
                    block = Singleton.Manager<ManPointer>.inst.targetVisible.block;
                }
                catch (Exception ex)
                {
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
            GUI.skin.label.margin.bottom = 0;*/
            try
            {
                win = GUI.Window(ID, win, new GUI.WindowFunction(DoWindow), "Block Infos");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void DoWindow(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Type");
            GUILayout.Label(block.BlockType.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Category");
            GUILayout.Label(block.BlockCategory.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rarity");
            GUILayout.Label(block.BlockRarity.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Centre of Mass");
            GUILayout.Label(block.CentreOfMass.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Mass");
            GUILayout.Label(block.CurrentMass.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Health");
            GUILayout.Label(block.visible.damageable.Health.ToString());
            GUILayout.EndHorizontal();


            if (GUILayout.Button("Close"))
            {
                visible = false;
                block = null;
            }
            GUI.DragWindow();
        }
    }
}
