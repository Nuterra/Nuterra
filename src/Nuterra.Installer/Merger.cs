using System;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Nuterra.Installer.ModuleImport;

namespace Nuterra.Installer
{
	public static class Merger
	{
		public static void Main()
		{
			string managedDir = @"D:\Program Files (x86)\Steam\steamapps\common\TerraTech Beta\TerraTechWin64_Data\Managed";
			string terraTech = Path.Combine(managedDir, "Assembly-CSharp-backup-0.dll");
			string nuterra = "Nuterra.dll";
			string output = Path.Combine(managedDir, "Assembly-CSharp.dll");
			string accessFile = "installer.access.txt";
			MergeNuterra(managedDir, terraTech, nuterra, accessFile, output);//TODO: Run tests and then figure out if hooking works or not
		}

		public static void MergeNuterra(string managedDir, string terraTechAssemblyFile, string nuterraAssemblyFile, string accessFile, string outputFile)
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

			AccessModifier.Apply(terraTech, accessFile);
			Hooking.Hooker.Apply(terraTech);

			terraTech.Write(outputFile, options);
		}
	}
}