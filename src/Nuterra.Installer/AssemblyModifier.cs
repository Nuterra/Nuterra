using System;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Nuterra.Installer.ModuleImport;

namespace Nuterra.Installer
{
	internal sealed class AssemblyModifier
	{
		private string _managedDir;
		private string _terraTechAssemblyFile;
		private ModuleDefMD _terraTech;

		public AssemblyModifier(string managedDir, string terraTechAssemblyFile)
		{
			_managedDir = managedDir;
			_terraTechAssemblyFile = terraTechAssemblyFile;

			AssemblyResolver resolver = new AssemblyResolver();
			resolver.PreSearchPaths.Add(managedDir);
			_terraTech = ModuleDefMD.Load(terraTechAssemblyFile);
		}

		public void ApplyAccessorMod(string accessFile)
		{
			AccessModifier.Apply(_terraTech, accessFile);
		}

		public void MergeNuterra(string nuterraAssemblyFile)
		{
			var importer = new ModuleImporter(_terraTech.Assembly.ManifestModule, makeEverythingPublic: false);
			var v = File.ReadAllBytes(nuterraAssemblyFile);
			var debugFileResult = new DebugFileResult();
			importer.Import(v, debugFileResult, ModuleImporter.Options.None);
			new AddUpdatedNodesHelper().Finish(_terraTech, importer);
		}

		public void HookNuterra()
		{
			Hooking.Hooker.Apply(_terraTech);
		}

		public void Write(string outputFile)
		{
			ModuleWriterOptions options = new ModuleWriterOptions();
			options.MetaDataOptions.Flags = MetaDataFlags.PreserveRids;

			_terraTech.Write(outputFile, options);
		}
	}
}