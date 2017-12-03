using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nuterra.UI
{
    class InputFields
    {
        public GUIStyle Default { get; set; }

        internal InputFields()
        {
            this.Default = new GUIStyle
            {
                normal =
                {
                    background = Elements.LoadImageFromAsset("TextField_BG.png"),
                    textColor = Color.white
                },
                active =
                {
                    background = Elements.LoadImageFromAsset("TextField_BG_Highlight.png"),
                    textColor = Color.white
                },

                clipping = TextClipping.Clip,
                border = new RectOffset(10, 10, 5, 5),
                padding = new RectOffset(10, 10, 4, 4)
            };
        }
    }
}
