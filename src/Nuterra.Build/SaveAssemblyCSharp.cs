using System;
using dnlib.DotNet.Writer;

namespace Nuterra.Build
{
	public sealed class SaveAssemblyCSharp : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			ModuleWriterOptions writerOptions = new ModuleWriterOptions();
			writerOptions.MetaDataOptions.Flags = MetaDataFlags.PreserveRids;
			info.AssemblyCSharp.Write(info.AssemblyCSharpOutputPath, writerOptions);
		}
	}
}