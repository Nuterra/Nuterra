using System;
using System.IO;
using Nuterra.Build;
using Nuterra.Installer.ModuleImport;

namespace Nuterra.Installer
{
	public sealed class AbsorbNuterra : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			var importer = new ModuleImporter(info.AssemblyCSharp.Assembly.ManifestModule, makeEverythingPublic: false);
			var assemblyBytes = File.ReadAllBytes(info.NuterraAssembly);
			var debugFileResult = new DebugFileResult();
			importer.Import(assemblyBytes, debugFileResult, ModuleImporter.Options.None);
			new AddUpdatedNodesHelper().Finish(info.AssemblyCSharp, importer);
		}
	}
}