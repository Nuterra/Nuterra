using System;
using System.IO;
using dnlib.DotNet.Writer;

namespace Nuterra.Build
{
	public sealed class SaveAssemblyCSharp : ModificationStep
	{
		protected override void Perform(ModificationInfo info)
		{
			string output = info.AssemblyCSharpOutputPath ?? Path.Combine(info.TerraTechManaged, "Assembly-CSharp.dll");
			ModuleWriterOptions writerOptions = new ModuleWriterOptions();
			//writerOptions.MetaDataOptions.Flags = MetaDataFlags.PreserveFieldRids | MetaDataFlags.PreservePropertyRids;
			info.AssemblyCSharp.Write(output, writerOptions);
		}
	}
}