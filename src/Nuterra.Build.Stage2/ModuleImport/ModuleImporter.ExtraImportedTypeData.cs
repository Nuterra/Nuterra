using System;
using dnlib.DotNet;

namespace Nuterra.Installer.ModuleImport
{
	partial class ModuleImporter
	{
		private struct ExtraImportedTypeData
		{
			/// <summary>
			/// New type in temporary module created by the compiler
			/// </summary>
			public TypeDef CompiledType { get; }

			public ExtraImportedTypeData(TypeDef compiledType)
			{
				CompiledType = compiledType;
			}
		}
	}
}