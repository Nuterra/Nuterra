using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Rewired.UI.ControlMapper;
using Nuterra.Internal;


namespace Sylver.CommandConsole
{
    class CommandHandler : MonoBehaviour
    {

        private void OnGUI()
        {
            if (!visible) return;
            
            
            //GUI.skin.window = new GUIStyle { normal = { background = back, textColor = Color.white }, stretchHeight = true, stretchWidth = true };
            GUI.Window(int.MaxValue, new Rect(Screen.width - 500f, Screen.height - 500f, 500f, 500f), new GUI.WindowFunction(DoWindow), "Console"/*, new GUIStyle { normal = { background = back, textColor = Color.white }/*, stretchHeight = true, stretchWidth = true }*/);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                visible = !visible;
            }
        }

        private void DoWindow(int id)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUILayout.Label(output);
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal();
            expr = GUILayout.TextField(expr);
            GUILayout.EndHorizontal();
            if(GUILayout.Button("Execute"))
            {
                output.text += "\n" + expr;
                Handler(expr);
            }
        }

        private void Handler(string input)
        {
            if (input.Split(' ').Length == 0) return;
            string commandName = input.Split(' ')[0];
            
            if (commandName == "Help")
            {
                int page = 0;
                int i = 0;
                if (input.Split(' ').Length > 1)
                {
                    if (!int.TryParse(input.Split(' ')[1], out page))
                    {
                        TTCommand commandHelp = (TTCommand)Activator.CreateInstance(Commands[input.Split(' ')[1]]);
                        output.text += "\n" + string.Format(info, input.Split(' ')[1] + " : " + commandHelp.Description);

                        foreach (string argName in commandHelp.ArgumentsDescriptions.Keys)
                        {
                            Console.WriteLine(argName.ToString());
                            try
                            {
                                Console.WriteLine(commandHelp.ArgumentsDescriptions[argName]);

                                output.text += "\n" + argName + " : " + commandHelp.ArgumentsDescriptions[argName];
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                            }
                        }
                        Console.WriteLine(output.text);
                    }
                    else
                    {
                        if (page < 0) page = 0;
                        if (page > 0) page -= 1;
                        output.text += "\n" + string.Format(info, "Help - Page " + (page + 1) + "/" + Math.Ceiling((double)Commands.Count / 5));
                        foreach (string commName in Commands.Keys)
                        {
                            if (page * 5 <= i && i < page * 5 + 5)
                            {
                                TTCommand commandHelp = (TTCommand)Activator.CreateInstance(Commands[commName]);
                                output.text += "\n" + commName + " : " + commandHelp.Description;
                            }
                            i++;
                        }
                    }
                }
                else
                {
                    output.text += "\n" + string.Format(info,"Help - Page " + (page + 1) + "/" + Math.Ceiling((double)Commands.Count / 5));
                    foreach(string commName in Commands.Keys)
                    {
                        if(page*5<=i && i<page*5+5)
                        {
                            TTCommand commandHelp = (TTCommand)Activator.CreateInstance(Commands[commName]);
                            output.text += "\n" + commName + " : " + commandHelp.Description;
                        }
                        i++;
                    }
                }

            }
            else if (commandName == "clear")
            {
                output.text = "";
            }
            else if (commandName == "s")
            {
                Tank temp = Singleton.Manager<ManSpawn>.inst.SpawnEmptyTech(Singleton.playerTank.Team, Singleton.playerPos + new Vector3(30, 0, 30), Quaternion.identity, true, false);
                try
                {
                    output.text += "\n"+ temp.blockman.blockTableSize + " " + temp.blockman.blockCentreBounds.ToString();
                    temp.blockman.AddBlock(Singleton.Manager<ManSpawn>.inst.SpawnBlock(BlockTypes.GSOCockpit_111, new Vector3(0, 53, 0), Quaternion.identity), IntVector3.zero);
                    output.text += "\n" + temp.blockman.blockTableSize + " " + temp.blockman.blockCentreBounds.ToString();
                    output.text += "\n" + temp.blockman.GetBlockAtPosition(IntVector3.zero).BlockType.ToString();
                } catch (Exception ex)
                {
                    output.text += "\n" + string.Format(error, ex.Message + "\n" + ex.StackTrace);
                }
            }
            else
            {
                if (!Commands.TryGetValue(commandName, out var a))
                {
                    output.text += "\n" + string.Format(info,"The command \"" + commandName + "\" doesn't exists\nType \"Help\" to get a list of available commands");
                }
                else
                {
                    Dictionary<string, string> args = new Dictionary<string, string>();
                    if (input.Split(' ').Length > 1)
                    {
                        for (var i = 1; i < input.Split(' ').Length; i++)
                        {
                            string[] arg = input.Split(' ')[i].Split(':');
                            args.Add(arg[0], arg[1]);
                        }
                    }

                    TTCommand command = (TTCommand)Activator.CreateInstance(Commands[commandName]);
                    try
                    {
                        string commandOut = command.Call(args);
                        if (commandOut != null) output.text += "\n" + commandOut;
                    }
                    catch (Exception ex)
                    {
                        output.text += "\n" + string.Format(error,"An error occured in the command " + commandName);
                        Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                    }
                }
            }
        }
        private string expr = "";
        private GUIContent output = new GUIContent("");
        private bool visible = false;

        public static Dictionary<string, Type> Commands = new Dictionary<string, Type>();

        private Vector2 scrollPos = Vector2.zero;

        public static Texture2D back = new Texture2D(42, 42);

        public static string info = "<color=yellow>{0}</color>";
        public static string error = "<color=red>{0}</color>";
    }
}
