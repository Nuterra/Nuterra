using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Nuterra.Internal;

namespace Nuterra.UI
{
    class Elements
    {
        public static bool IsInitialized { get; private set; } = false;

        public static Windows Windows { get; private set; }

        public static Buttons Buttons { get; private set; }

        public static Toggle Toggle { get; private set; }

        public static ScrollView ScrollView { get; private set; }

        public static InputFields InputFields { get; private set; }

        public static readonly AssetBundle NuterraUIAssetBundle;

        static Elements()
        {
            NuterraUIAssetBundle = AssetBundle.LoadFromFile(Path.Combine(FolderStructure.AssetsFolder, "mod-nuterra-ui"));

            if (NuterraUIAssetBundle == null)
            {
                Debug.Log("Failed to load mod-nuterra-ui AssetBundle, errors are coming");
            }
        }

        public static void BuildElements()
        {          
            IsInitialized = true;
            Windows = new Windows();
            Buttons = new Buttons();
            Toggle = new Toggle();
            ScrollView = new ScrollView();
            InputFields = new InputFields();
            NuterraGUI.BuildSkin();
        }

        public static Texture2D LoadImageFromAsset(string name)
        {
            return NuterraUIAssetBundle.LoadAsset<Texture2D>(@"Assets/UI/Elements/"+name);
        }

        public static Texture2D LoadImageFromFile(string name)
        {
            if (loadedTextures.ContainsKey(name))
            {
                Texture2D texture2D;
                loadedTextures.TryGetValue(name, out texture2D);
                if (texture2D != null)
                {
                    return texture2D;
                }
            }
            Texture2D result;
            try
            {
                Texture2D texture2D2 = new Texture2D(0, 0);
                texture2D2.LoadImage(File.ReadAllBytes(Path.Combine(FolderStructure.AssetsFolder + "\\UI", name)));
                loadedTextures.Add(name, texture2D2);
                result = texture2D2;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Failed to load: " + name);
                Debug.LogException(ex);
                result = null;
            }
            return result;
        }
        private static readonly Dictionary<string, Texture2D> loadedTextures = new Dictionary<string, Texture2D>();
    }
}
