using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Sylver.CommandConsole
{
    class DisarmCommand : TTCommand
    {
        public override Dictionary<string, string> ArgumentsDescriptions => new Dictionary<string, string>();

        public override string Name => "Disarm";

        public override string Description => "Disarm a tech by making his weapons explode";

        public override string Call(Dictionary<string, string> arguments)
        {
            if (!Singleton.playerTank) return string.Format(CommandHandler.info, "Specified Tech not found");
                
            foreach(TankBlock block in Singleton.playerTank.blockman.IterateBlocks())
            {
                if (Singleton.Manager<ManSpawn>.inst.m_BlockPrefabs[(int)block.BlockType].GetComponent("ModuleWeapon") && !Singleton.playerTank.blockman.IsRootBlock(block))
                    block.visible.damageable.Damage(new ManDamage.DamageInfo(Single.MaxValue, ManDamage.DamageType.Impact, null, null, default(Vector3), default(Vector3)));
            }

            return "Tech disarmed";
        }

        public override void Init()
        {
            CommandHandler.Commands.Add(Name, GetType());
        }
    }
}
