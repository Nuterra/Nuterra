using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Nuterra.Installer.ModuleImport;

namespace Nuterra.Installer
{
	public static class Merger
	{

		public static void Main()
		{
			string managedDir = @"MergeTest";
			string terraTechAssembly = Path.Combine(managedDir, "TerraTest.dll");
			string nuterraAssembly = Path.Combine(managedDir, "NuterraTest.dll");
			string outputFile = Path.Combine(managedDir, "MergedTest.dll");
			MergeNuterra(managedDir, terraTechAssembly, nuterraAssembly, outputFile);
		}

		public static void MergeNuterra(string managedDir, string terraTechAssemblyFile, string nuterraAssemblyFile, string outputFile)
		{
			AssemblyResolver resolver = new AssemblyResolver();
			resolver.PreSearchPaths.Add(managedDir);
			ModuleContext context = new ModuleContext();
			ModuleDefMD terraTech = ModuleDefMD.Load(terraTechAssemblyFile, context);

			var importer = new ModuleImporter(terraTech.Assembly.ManifestModule, makeEverythingPublic: false);
			var v = File.ReadAllBytes(nuterraAssemblyFile);
			var debugFileResult = new DebugFileResult();
			importer.Import(v, debugFileResult, ModuleImporter.Options.None);
			new AddUpdatedNodesHelper().Finish(terraTech, importer);

			ModuleWriterOptions options = new ModuleWriterOptions();
			options.MetaDataOptions.Flags = MetaDataFlags.PreserveRids;


			terraTech.Write(outputFile, options);

		}
	}
}
