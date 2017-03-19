using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;

namespace Nuterra.Build
{
	public class ModificationInfo
	{
		public string TerraTechRoot { get; set; }
		public HashSet<string> AcceptedHashes { get; } = new HashSet<string>();
		public string AssemblyCSharpOutputPath { get; set; }
		public string AccessFilePath { get; set; }
		public string TerraTechData { get; set; }
		public ModuleDefMD AssemblyCSharp { get; set; }
		public string TerraTechManaged => Path.Combine(TerraTechData, "Managed");
		public string CleanAssemblyPath { get; set; }
		public string NuterraData { get; set; }
		public string NuterraAssembly { get; set; }

		public static ModificationInfo Parse(string[] commandLineArgs)
		{
			ModificationInfo info = new ModificationInfo();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				string param = commandLineArgs[i];
				string next = (i < commandLineArgs.Length - 1) ? commandLineArgs[i + 1] : null;
				bool skipNext = false;

				if (param.Equals(CommonSwitches.TerraTechDir, StringComparison.OrdinalIgnoreCase))
				{
					info.TerraTechRoot = next;
					skipNext = true;
				}

				if (param.Equals(CommonSwitches.AssemblyHash, StringComparison.OrdinalIgnoreCase))
				{
					info.AcceptedHashes.Add(next);
					skipNext = true;
				}

				if (param.Equals(CommonSwitches.AssemblyOutput, StringComparison.OrdinalIgnoreCase))
				{
					info.AssemblyCSharpOutputPath = next;
					skipNext = true;
				}

				if (param.Equals(CommonSwitches.AccessFile, StringComparison.OrdinalIgnoreCase))
				{
					info.AccessFilePath = next;
					skipNext = true;
				}

				if (param.Equals(CommonSwitches.NuterraData, StringComparison.OrdinalIgnoreCase))
				{
					info.NuterraData = next;
					skipNext = true;
				}

				if (param.Equals(CommonSwitches.NuterraAssembly, StringComparison.OrdinalIgnoreCase))
				{
					info.NuterraAssembly = next;
					skipNext = true;
				}

				if (skipNext)
				{
					i++;
				}
			}
			info.InitDefaults();

			return info;
		}

		public void InitDefaults()
		{
			if (TerraTechRoot == null)
			{
				TerraTechRoot = Directory.GetCurrentDirectory();
			}

			if (NuterraData == null)
			{
				NuterraData = Path.Combine(TerraTechRoot, "Nuterra_Data");
			}

			if (AcceptedHashes.Count == 0)
			{
				AcceptedHashes.Add(Path.Combine(NuterraData, "build.hash.txt"));
			}

			if (AccessFilePath == null)
			{
				AccessFilePath = Path.Combine(NuterraData, "build.access.txt");
			}

			foreach (string hashSpec in AcceptedHashes.ToArray())
			{
				if (string.IsNullOrWhiteSpace(hashSpec) || hashSpec.StartsWith("#"))
				{
					AcceptedHashes.Remove(hashSpec);
				}
				else if (hashSpec.Contains('.'))
				{
					AcceptedHashes.Remove(hashSpec);
					//*.* treated as file
					if (File.Exists(hashSpec))
					{
						foreach (string hash in File.ReadAllLines(hashSpec))
						{
							string[] parts = hash.Split(new char[] { ' ' }, count: 2);
							AcceptedHashes.Add(parts[0]);
						}
					}
					else
					{
						Error.MissingFile(hashSpec);
					}
				}
			}

			if (AcceptedHashes.Count == 0)
			{
				Error.NoHashSpecified();
			}

			if (NuterraAssembly == null)
			{
				NuterraAssembly = "Nuterra.Internal.dll";
			}
		}
	}
}