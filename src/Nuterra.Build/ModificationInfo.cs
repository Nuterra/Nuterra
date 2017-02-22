using System;
using System.IO;
using dnlib.DotNet;

namespace Nuterra.Build
{
	public class ModificationInfo
	{
		public string TerraTechRoot { get; set; }
		public string ExpectedHash { get; set; }
		public string AssemblyCSharpOutputPath { get; set; }
		public string AccessFilePath { get; set; }
		public string TerraTechData { get; set; }
		public ModuleDefMD AssemblyCSharp { get; set; }
		public string TerraTechManaged => Path.Combine(TerraTechData, "Managed");
		public string CleanAssemblyPath { get; set; }
	}
}