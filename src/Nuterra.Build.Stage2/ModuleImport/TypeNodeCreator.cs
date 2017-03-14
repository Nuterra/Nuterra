using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace Nuterra.Build.ModuleImport
{
	internal class TypeNodeCreator
	{
		private List<TypeDef> newTypes;
		private ModuleDef module;

		public TypeNodeCreator(ModuleDef module, List<TypeDef> newTypes)
		{
			this.module = module;
			this.newTypes = newTypes;
		}

		internal void Add()
		{
			foreach (TypeDef newType in newTypes)
			{
				module.Types.Add(newType);
			}
		}
	}
}