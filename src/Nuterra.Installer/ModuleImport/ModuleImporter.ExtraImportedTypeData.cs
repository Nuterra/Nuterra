using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;

namespace Nuterra.Installer.ModuleImport
{
	partial class ModuleImporter
	{

		struct ExtraImportedTypeData
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
