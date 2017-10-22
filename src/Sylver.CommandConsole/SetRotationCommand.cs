using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Sylver.CommandConsole
{
    class SetRotationCommand : TTCommand
    {
        public override Dictionary<string, string> ArgumentsDescriptions => new Dictionary<string, string>
        {
            {
                "(optional) X",
                "Rotation on X axis (in degrees)"
            },
            {
                "(optional) Y",
                "Rotation on Y axis (in degrees)"
            },
            {
                "(optional) Z",
                "Rotation on Z axis (in degrees)"
            }
        };

        public override string Name => "SetRotation";

        public override string Description => "Set the rotation of a tech";

        public override string Call(Dictionary<string, string> arguments)
        {
            if (!Singleton.playerTank) return string.Format(CommandHandler.info, "Specified Tech not found");

            Vector3 newRot = Singleton.playerTank.trans.rotation.eulerAngles;

            if (arguments.TryGetValue("X", out string argX)) if (int.TryParse(argX, out int intX)) newRot.x = intX; //Quaternion.Euler(intX, 0, 0).x;
            if (arguments.TryGetValue("Y", out string argY)) if (int.TryParse(argY, out int intY)) newRot.y = intY; //Quaternion.Euler(0, intY, 0).y;
            if (arguments.TryGetValue("Z", out string argZ)) if (int.TryParse(argZ, out int intZ)) newRot.z = intZ; //Quaternion.Euler(0, 0, intZ).z;
            Singleton.playerTank.trans.rotation = Quaternion.Euler(newRot);

            return "Tech rotated to " + Singleton.playerTank.trans.rotation.eulerAngles.ToString();
        }

        public override void Init()
        {
            CommandHandler.Commands.Add(Name, GetType());
        }
    }
}
