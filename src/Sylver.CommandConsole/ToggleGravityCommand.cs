using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sylver.CommandConsole
{
    class ToggleGravityCommand : TTCommand
    {
        public override Dictionary<string, string> ArgumentsDescriptions => new Dictionary<string, string>();
        /*{
            {
                "(optional) Type",
                "Type of object ( Tech | Block | Crate | Chunk | Scenery | Waypoint )"
            }
        };*/

        public override string Name => "ToggleGravity";

        public override string Description => "Toggles the gravity of the specified object type (All by default)";

        public override string Call(Dictionary<string, string> arguments)
        {
            g = !g;
            foreach (Tank cur in Singleton.Manager<ManTechs>.inst.CurrentTechs)
            {
                cur.rbody.useGravity = g;
            }
            return "Gravity set to " + g;
            /*string type = "All";
            
            try
            {
                var Visibles = Utilities.UtilitiesMod.AllVisibles;
                if (arguments.TryGetValue("Type", out type) && Visibles != null)
                {
                    if (type != "All")
                    {
                        ObjectTypes VisibleType = ObjectTypes.Null;
                        switch (type)
                        {
                            case "Block":
                                VisibleType = ObjectTypes.Block;
                                break;

                            case "Tech":
                            case "Vehicle":
                            case "Tank":
                                VisibleType = ObjectTypes.Vehicle;
                                break;

                            case "Crate":
                                VisibleType = ObjectTypes.Crate;
                                break;

                            case "Chunk":
                                VisibleType = ObjectTypes.Chunk;
                                break;

                            case "Scenery":
                                VisibleType = ObjectTypes.Scenery;
                                break;

                            case "Waypoint":
                                VisibleType = ObjectTypes.Waypoint;
                                break;

                            default:
                                return string.Format(CommandHandler.error, "Incorrect argument \"" + type + "\"");
                        }
                        Console.WriteLine(type + " " + VisibleType);
                        if (type == "Tank")
                        {
                            foreach (Tank cur in Singleton.Manager<ManTechs>.inst.CurrentTechs)
                            {
                                cur.rbody.useGravity = g;
                            }
                        }
                        else
                        {
                            foreach (var vis in Visibles[(int)VisibleType])
                            {
                                vis.rbody.useGravity = g;
                            }
                        }
                    }

                }
                else
                {
                    /*var state = Singleton.Manager<ManSaveGame>.inst.CurrentState;
                    foreach (ManSaveGame.StoredTile tile in state.m_StoredTiles.Values)
                    {
                        foreach(var visList in tile.m_StoredVisibles.Values)
                        {
                            foreach (var vis in visList)
                            {

                            }
                        }
                    }
                    try
                    {
                        foreach (var list in Visibles.Values)
                        {
                            foreach (var vis in list)
                            {
                                vis.rbody.useGravity = g;
                            }
                        }
                    }
                    catch
                    {
                        foreach (Tank cur in Singleton.Manager<ManTechs>.inst.CurrentTechs)
                        {
                            cur.rbody.useGravity = g;
                        }
                    }
                }

                //Singleton.playerTank.rbody.useGravity = !Singleton.playerTank.rbody.useGravity;
                return "Gravity set to " + g;
            }
            catch (Exception ex)
            {
                return string.Format(CommandHandler.error, ex.Message + "\n" + ex.StackTrace);
            }*/
        }

        public override void Init()
        {
            CommandHandler.Commands.Add(Name, GetType());
        }

        private static bool g = true;
    }
}
