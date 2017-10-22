using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Sylver.CommandConsole
{
    class TeleportationCommand : TTCommand
    {
        public override void Init()
        {
            try
            {
                CommandHandler.Commands.Add(Name, GetType());
            }
            catch { }
            /*argumentsDescriptions.Add("X", "X position");
            argumentsDescriptions.Add("Y", "Y position");
            argumentsDescriptions.Add("Z", "Z position");
            foreach(string a in ArgumentsDescriptions.Keys)
            {
                Console.WriteLine(a + " " + ArgumentsDescriptions[a]);
            }*/
        }
        public override string Call(Dictionary<string, string> arguments)
        {
            var x = "";
            var y = "";
            var z = "";
            if(!arguments.TryGetValue("X", out x) || !arguments.TryGetValue("Y", out y) || !arguments.TryGetValue("Z", out z))
            {
                return string.Format(CommandHandler.error, "Missing Argument");
            }
            Vector3 vector = new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
            Vector3 b = Singleton.cameraTrans.position - Singleton.playerTank.boundsCentreWorld;
            Vector3 vector2 = Singleton.cameraTrans.rotation * vector;
            vector2 = vector2.SetY(0f).normalized * 100f;
            Vector3 vector3 = Singleton.playerTank.boundsCentreWorld + vector2;
            Singleton.playerTank.visible.Teleport(vector, Singleton.playerTank.trans.rotation, false);
            vector3 = Singleton.Manager<ManWorld>.inst.ProjectToGround(vector3, true) + Vector3.up;
            Singleton.Manager<CameraManager>.inst.ResetCamera(vector + b, Singleton.cameraTrans.rotation);

            return "Player teleported to "+x+" "+y+" "+z;

        }

        public override string Name => "Teleport";
        public override string Description => "Teleports the player to the specified X Y Z values";

        public override Dictionary<string, string> ArgumentsDescriptions => new Dictionary<string, string>
        {
            {
                "X",
                "X position"
            },
            {
                "Y",
                "Y position"
            },
            {
                "Z",
                "Z position"
            }
        };
    }
}
