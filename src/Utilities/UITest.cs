using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Nuterra.Internal;

namespace Utilities
{
    class UITest : MonoBehaviour
    {
        private void Start()
        {
            nuterraGreen.LoadImage(File.ReadAllBytes(Path.Combine(FolderStructure.AssetsFolder, "NuterraGreen.png")), false);
            var btrans = button.transform as RectTransform;
            btrans.position = Vector3.zero;
            btrans.anchorMin = new Vector2(0.5f, 0.5f);
            btrans.anchorMax = new Vector2(0.5f, 0.5f);
            /*Sprite a = new Sprite();
            a.texture = new Texture2D();
                a.texture.LoadImage(File.ReadAllBytes(Path.Combine(FolderStructure.AssetsFolder, "Border_BG.png")));
            button.image.sprite = a;*/
            button.gameObject.transform.parent = gameObject.transform;

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                gameObject.SetActive(!gameObject.activeSelf);

            }
        }

        public bool CanDraw()
        {
            if (_splashScreen == null) return false;
            if (_splashScreen.m_MyCanvas == null) return false;
            if (!_splashScreen.m_MyCanvas.gameObject.activeSelf) return false;
            if (_splashScreen.m_SplashScreenIndex != 0) return false;
            return true;
        }

        private void OnGUI()
        {
            if(CanDraw())
            {
                GUI.DrawTexture(new Rect(Screen.width/2-315/2,Screen.height*0.8f,315,50), nuterraGreen);
            }
        }

        public ManSplashScreen _splashScreen = Singleton.Manager<ManSplashScreen>.inst;

        Button button = ManUI.inst.GetScreen(ManUI.ScreenType.RenameTech).m_ExitButton;
        Texture2D nuterraGreen = new Texture2D(315, 49);
    }
}
