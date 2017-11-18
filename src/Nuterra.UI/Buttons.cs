using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Nuterra.UI
{
    public class Buttons
    {
        public GUIStyle HUDDefault { get; set; }

        public GUIStyle HUDRed { get; set; }

        public GUIStyle Close { get; set; }

        public GUIStyle Plain { get; set; }

        public GUIStyle PlainGrey { get; set; }

        internal Buttons()
        {
            HUDDefault = new GUIStyle(GUI.skin.button)
            {
                normal =
                {
                    background = Elements.LoadImage("HUD_Button_BG.png"),
                    textColor = Color.white
                },
                hover =
                {
                    background = Elements.LoadImage("HUD_Button_Highlight.png"),
                    textColor = Color.white
                },
                active =
                {
                    background = Elements.LoadImage("HUD_Button_Selected.png"),
                    textColor = Color.white
                },
                border = new RectOffset(16, 16, 16, 16),
                padding = new RectOffset(8, 8, 8, 8),
                alignment = TextAnchor.MiddleCenter,
                
            };

            HUDRed = new GUIStyle(HUDDefault)
            {
                normal =
                {
                    background = Elements.LoadImage("HUD_Button_GREY_BG.png"),
                    textColor = Color.white
                },
                hover =
                {
                    background = Elements.LoadImage("HUD_Button_RED_BG.png"),
                    textColor = Color.white
                },
                active =
                {
                    background = Elements.LoadImage("HUD_Button_RED_BG.png"),
                    textColor = Color.white
                },
            };

            Close = new GUIStyle
            {
                normal =
                {
                    background = Elements.LoadImage("Button_Close.png")
                },
                active =
                {
                    background = Elements.LoadImage("Button_Close.png")
                },
                alignment = TextAnchor.UpperRight,
                margin = new RectOffset { right = 0, top=0 }
            };

            Plain = new GUIStyle(HUDDefault)
            {
                normal =
                {
                    background = Elements.LoadImage("Button_BG.png")
                },
                hover =
                {
                    background = Elements.LoadImage("Button_BG.png")
                },
                active =
                {
                    background = Elements.LoadImage("Button_Highlight_BG.png")
                }
            };

            PlainGrey = new GUIStyle(Plain)
            {
                normal =
                {
                    background = Elements.LoadImage("Button_GREY_BG.png")
                },
                hover =
                {
                    background = Elements.LoadImage("Button_GREY_BG.png")
                },
            };
        }
    }
}
