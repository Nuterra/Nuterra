using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Sylver.CommandConsole
{
    class SetGravityCommand : TTCommand
    {
        public override Dictionary<string, string> ArgumentsDescriptions => new Dictionary<string, string>
        {
            {
                "(optional) X",
                "Gravity on X axis (default : 0)"
            },
            {
                "(optional) Y",
                "Gravity on Y axis (default : -30)"
            },
            {
                "(optional) Z",
                "Gravity on Z axis (default : 0)"
            }
        };

        public override string Name => "SetGravity";

        public override string Description => "Set the game Gravity";

        public override string Call(Dictionary<string, string> arguments)
        {
            

            Vector3 newGrav = Physics.gravity;

            if (arguments.TryGetValue("X", out string argX)) if (float.TryParse(argX, out float floatX)) newGrav.x = floatX; //Quaternion.Euler(floatX, 0, 0).x;
            if (arguments.TryGetValue("Y", out string argY)) if (float.TryParse(argY, out float floatY)) newGrav.y = floatY; //Quaternion.Euler(0, floatY, 0).y;
            if (arguments.TryGetValue("Z", out string argZ)) if (float.TryParse(argZ, out float floatZ)) newGrav.z = floatZ; //Quaternion.Euler(0, 0, floatZ).z;
            Physics.gravity = newGrav;

            return "Gravity set to to " + Physics.gravity.ToString();
        }

        public override void Init()
        {
            CommandHandler.Commands.Add(Name, GetType());
        }
    }
}
