using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Utilities
{
    class ManAssets : Singleton.Manager<ManAssets>
    {
        private void Start()
        {
            StartCoroutine(UnpackAssets());
            foreach(string key in assets.Keys)
            {
                Console.WriteLine(key);
            }
        }

        private IEnumerator UnpackAssets()
        {
            string path = Application.dataPath + "/sharedassets0.assets";
            int versionUniqueId = SKU.CalcUniqueChangelistVersionIntRepresentation();

            using (WWW www = WWW.LoadFromCacheOrDownload("file://" + path, versionUniqueId))
            {
                yield return www;
                if (!www.error.NullOrEmpty())
                {
                    throw new Exception("WWW download had an error:" + www.error);
                }
                AssetBundle bundle = www.assetBundle;
                string[] assetNames = bundle.GetAllAssetNames();
                for (int j = 0; j < assetNames.Length; j++)
                {
                    AssetBundleRequest request = bundle.LoadAssetAsync(assetNames[j], typeof(UnityEngine.Object));
                    yield return request;
                    assets.Add(request.asset.name, request.asset);
                }
                bundle.Unload(false);
            }

        }
        public Dictionary<string, UnityEngine.Object> assets = new Dictionary<string, UnityEngine.Object>();
    }
}
