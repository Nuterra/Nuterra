using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;
using System.Collections;

namespace Utilities
{
    public class UtilitiesMod : TerraTechMod
    {
        private GameObject _holder;

        public override string Name => "Utilities";

        public override string Description => "Display some debug values about the game (player position, game mode, etc.";

        public override void Load()
        {
            base.Load();
            _holder = new GameObject();
            _holder.AddComponent<PlayerInfo>();
            _holder.AddComponent<CurrentSaveInfo>();
            _holder.AddComponent<TimeInfo>();
            //_holder.AddComponent<ManAssets>();
            _holder.AddComponent<UITest>();
            UnityEngine.Object.DontDestroyOnLoad(_holder);
            Hooks.Visibles.VisibleSpawned += Visibles_VisibleSpawned;
            Hooks.Visibles.VisibleRecycled += Visibles_VisibleRecycled;
        }

        private void Visibles_VisibleRecycled(VisibleRecycledEvent obj)
        {
            try
            {
                AllVisibles[(int)obj.visible.type].Add(obj.visible);
            }
            catch { }
        }

        private void Visibles_VisibleSpawned(VisibleSpawnedEvent obj)
        {
            try
            {
                AllVisibles[(int)obj.visible.type].Remove(obj.visible);
            }
            catch { }
        }

        public static Dictionary<int, List<Visible>> AllVisibles = new Dictionary<int, List<Visible>>
        {
            {
                0,
                new List<Visible>()
            },
            {
                1,
                new List<Visible>()
            },
            {
                2,
                new List<Visible>()
            },
            {
                3,
                new List<Visible>()
            },
            {
                4,
                new List<Visible>()
            },
            {
                5,
                new List<Visible>()
            },
            {
                6,
                new List<Visible>()
            },
        };
    }
}
