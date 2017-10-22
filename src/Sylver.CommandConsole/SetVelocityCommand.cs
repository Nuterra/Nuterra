using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Sylver.CommandConsole
{
    class SetVelocityCommand : TTCommand
    {
        public override Dictionary<string, string> ArgumentsDescriptions => new Dictionary<string, string>
        {
            {
                "(optional) X",
                "Velocity on the X axis"
            },
            {
                "(optional) Y",
                "Velocity on the Y axis"
            },
            {
                "(optional) Z",
                "Velocity on the Z axis"
            },
        };

        public override string Name => "SetVelocity";

        public override string Description => "Set the velocity of a tech";

        public override string Call(Dictionary<string, string> arguments)
        {
            if (!Singleton.playerTank) return string.Format(CommandHandler.info, "Specified Tech not found");

            Vector3 newVel = Singleton.playerTank.rbody.velocity;

            if (arguments.TryGetValue("X", out string argX)) if (int.TryParse(argX, out int intX)) newVel.x = intX;
            if (arguments.TryGetValue("Y", out string argY)) if (int.TryParse(argY, out int intY)) newVel.y = intY;
            if (arguments.TryGetValue("Z", out string argZ)) if (int.TryParse(argZ, out int intZ)) newVel.z = intZ;
            Singleton.playerTank.rbody.velocity = newVel;

            return "Tech velocity set to " + newVel.ToString();
        }

        public override void Init()
        {
            CommandHandler.Commands.Add(Name, GetType());
        }
    }
}
