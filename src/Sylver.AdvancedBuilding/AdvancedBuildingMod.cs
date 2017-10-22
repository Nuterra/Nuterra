using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;

namespace Sylver.AdvancedBuilding
{
    public class AdvancedBuildingMod : TerraTechMod
    {
        private GameObject _holder;

        public override string Name => "Advanced Building Mod";

        public override string Description => "";

        public override void Load()
        {
            base.Load();
            _holder = new GameObject();
            _holder.AddComponent<RotateBlocks>();
            _holder.AddComponent<TranslateBlocks>();
            _holder.AddComponent<ScaleBlocks>();
            UnityEngine.Object.DontDestroyOnLoad(_holder);  
        }

    }
}
