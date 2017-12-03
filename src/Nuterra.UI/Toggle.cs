using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nuterra.UI
{
    class Toggle
    {
        public GUIStyle Default { get; set; }

        public GUIStyle Check { get; set; }

        internal Toggle()
        {
            this.Default = new GUIStyle
            {
                normal =
                {
                    background = Elements.LoadImageFromAsset("Toggle_OFF.png")
                },
                active =
                {
                    background = Elements.LoadImageFromAsset("Toggle_ON.png")
                },
            };
            Check = new GUIStyle(GUI.skin.toggle)
            {
                normal =
                {
                    background = Elements.LoadImageFromAsset("Check_Unticked.png")
                },
                active =
                {
                    background = Elements.LoadImageFromAsset("Check_Ticked.png")
                },
                border = new RectOffset(2, 2, 2, 2)
            };
        }
    }
}
