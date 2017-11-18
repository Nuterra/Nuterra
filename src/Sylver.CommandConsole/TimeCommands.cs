using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;

namespace Sylver.CommandConsole
{
    class TimeSetCommand : TTCommand
    {
        public override Dictionary<string, string> ArgumentsDescriptions => new Dictionary<string, string>
        {
            {
                "Moment",
                "Moment of the day ( Day | Night | Noon | Midnight )"
            },
            {
                "Hour",
                "Hour of the day"
            }
        };

        public override string Name => "TimeSet";

        public override string Description => "Set the time of day";

        public override string Call(Dictionary<string, string> arguments)
        {
            if (arguments.TryGetValue("Moment", out string moment))
            {
                switch (moment)
                {
                    case "Day":
                        Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(5, 30, 0);
                        break;

                    case "Night":
                        Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(18, 30, 0);
                        break;

                    case "Noon":
                        Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(12, 0, 0);
                        break;

                    case "Midnight":
                        Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(0, 0, 0);
                        break;

                    default:
                        return string.Format(CommandHandler.info, "Incorrect moment");
                }
            }
            else if (arguments.TryGetValue("Hour", out string sHour))
            {
                if (int.TryParse(sHour, out int hour)) Singleton.Manager<ManTimeOfDay>.inst.SetTimeOfDay(hour, 0, 0);
            }
            else
            {
                return string.Format(CommandHandler.info, "Missing Argument Moment/Hour");
            }

            return "Time set to " + Singleton.Manager<ManTimeOfDay>.inst.TimeOfDay;
        }

        public override void Init()
        {
            CommandHandler.Commands.Add(Name, GetType());
        }
    }

    class TimeStopCommand : TTCommand
    {
        public override Dictionary<string, string> ArgumentsDescriptions => new Dictionary<string, string>();

        public override string Name => "TimeStop";

        public override string Description => "Toggle the time";

        public override string Call(Dictionary<string, string> arguments)
        {
            Singleton.Manager<ManTimeOfDay>.inst.TogglePause();
            return "Time progression set to " + Singleton.Manager<ManTimeOfDay>.inst.m_Sky.Components.Time.ProgressTime;
        }

        public override void Init()
        {
            CommandHandler.Commands.Add(Name, GetType());
        }
    }

    class SetDayLengthCommand : TTCommand
    {
        public override Dictionary<string, string> ArgumentsDescriptions => new Dictionary<string, string>
        {
            {
                "Length",
                "Day length in minutes"
            }
        };

        public override string Name => "SetDayLength";

        public override string Description => "Set day length in minutes";

        public override string Call(Dictionary<string, string> arguments)
        {
            if (arguments.TryGetValue("Length", out string sLength))
            {
                if (float.TryParse(sLength, out float length)) Singleton.Manager<ManTimeOfDay>.inst.m_Sky.Components.Time.DayLengthInMinutes = length;
            }
            else
            {
                return string.Format(CommandHandler.info, "Missing Argument Length");
            }

            return "Day length set to " + Singleton.Manager<ManTimeOfDay>.inst.m_Sky.Components.Time.DayLengthInMinutes + " minutes";
        }

        public override void Init()
        {
            CommandHandler.Commands.Add(Name, GetType());
        }
    }
}
