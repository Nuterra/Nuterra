using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;

namespace Sylver.Piston
{
    public class PistonBlockMod : TerraTechMod
    {
        private GameObject _holder;

        public override string Name => "Piston Block";

        public override string Description => "";

        public override void Load()
        {
            base.Load();
            _holder = new GameObject();
            _holder.AddComponent<ModulePiston>();
            UnityEngine.Object.DontDestroyOnLoad(_holder);

            new BlockPrefabBuilder(BlockTypes.GSOBlock_111)
                .SetBlockID(10000)
                .SetName("EXP Piston")
                .SetDescription("Test")
                .SetFaction(FactionSubTypes.EXP)
                .SetCategory(BlockCategories.Power)
                .AddComponent<ModulePiston>()
                .Register();
        }
    }
}
