using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;
using System.IO;

namespace Sylver.PreciseSnapshots
{
    public class PreciseSnapshotsMod : TerraTechMod
    {
        public static readonly string PreciseSnapshotsFolder = Path.Combine(FolderStructure.RootFolder, "PreciseSnapshots");

        private GameObject _holder;

        public override string Name => "Precise Snapshots";

        public override string Description => "";

        public override void Load()
        {
            base.Load();
            _holder = new GameObject();
            _holder.AddComponent<SaveWindow>();
            _holder.AddComponent<LoadWindow>();
            UnityEngine.Object.DontDestroyOnLoad(_holder);

            if(!Directory.Exists(PreciseSnapshotsFolder))
            {
                Directory.CreateDirectory(PreciseSnapshotsFolder);
            }

            
        }
    }
}
