using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Nuterra;
using Nuterra.Internal;
using System.IO;

namespace Nuterra.UI
{
    public class NuterraGUI : Singleton.Manager<NuterraGUI>
    {

        public NuterraGUI()
        {

        }

        public static GUISkin Skin
        {
            get
            {
                //Console.WriteLine("Skin called");
                Init();
                return _skin;
            }
            set { _skin = value; }
        }

        private void OnGUI()
        {
            if(!Elements.IsInitialized)
            {
                Elements.BuildElements();
            }
        }

        private static void Init()
        {
            if (!Elements.IsInitialized)
            {
                Console.WriteLine("Initializing the Skin");
                Elements.BuildElements();
            }
        }

        private static void Start()
        {
            //inst.gameObject.SetActive(true);
        }

        internal static void BuildSkin()
        {
            NuterraGUI.Skin = GUI.skin;
            Skin.window = Elements.Windows.Default;
            Skin.button = Elements.Buttons.HUDDefault;
            Skin.toggle = Elements.Toggle.Default;

            Skin.verticalScrollbar = Elements.ScrollView.Vertical;
            Skin.verticalScrollbarThumb = Elements.ScrollView.ThumbVertical;
            Skin.horizontalScrollbar = Elements.ScrollView.Horizontal;
            Skin.horizontalScrollbarThumb = Elements.ScrollView.ThumbHorizontal;

            Skin.textField = (Skin.textArea = Elements.InputFields.Default);

            Skin.label.fontSize = 12;
            Skin.label.margin = new RectOffset { top = 0, bottom = 0 };
            Skin.label.padding = new RectOffset { top = 0, bottom = 0 };

            // Barbaric way to get the Exo-font but needs very specific modifications
            // Need to find another way
            /*try
            {
                Console.WriteLine(Skin.font.name);
                Console.WriteLine(UICoords.Exo.name);

                Skin.font = UICoords.Exo;

                string fontnames = "";
                foreach (string name in Skin.font.fontNames)
                {
                    fontnames += name + " ";
                }
                Console.WriteLine("\n" + fontnames + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }*/
               
        }

        private static GUISkin _skin;
    }
}
