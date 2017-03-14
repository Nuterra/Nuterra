using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;

namespace Nuterra.Build.ModuleImport
{
	internal sealed class AddUpdatedNodesHelper
	{
		public void Finish(ModuleDef module, ModuleImporter importer)
		{
			var dict = new Dictionary<string, List<TypeDef>>(StringComparer.Ordinal);
			foreach (var type in importer.NewNonNestedTypes)
			{
				module.Types.Add(type.TargetType);
			}

			var newAssemblyDeclSecurities = importer.NewAssemblyDeclSecurities;
			var newAssemblyCustomAttributes = importer.NewAssemblyCustomAttributes;
			var newModuleCustomAttributes = importer.NewModuleCustomAttributes;

			DeclSecurity[] origAssemblyDeclSecurities = null;
			CustomAttribute[] origAssemblyCustomAttributes = null;
			CustomAttribute[] origModuleCustomAttributes = null;

			if (newAssemblyDeclSecurities != null)
				origAssemblyDeclSecurities = module.Assembly.DeclSecurities.ToArray();
			if (newAssemblyCustomAttributes != null)
				origAssemblyCustomAttributes = module.Assembly.CustomAttributes.ToArray();
			if (newModuleCustomAttributes != null)
				origModuleCustomAttributes = module.Assembly.CustomAttributes.ToArray();

			if (origAssemblyDeclSecurities != null && newAssemblyDeclSecurities != null)
			{
				module.Assembly.DeclSecurities.Clear();
				foreach (var ds in newAssemblyDeclSecurities)
					module.Assembly.DeclSecurities.Add(ds);
			}
			if (origAssemblyCustomAttributes != null && newAssemblyCustomAttributes != null)
			{
				module.Assembly.CustomAttributes.Clear();
				foreach (var ca in newAssemblyCustomAttributes)
					module.Assembly.CustomAttributes.Add(ca);
			}
			if (origModuleCustomAttributes != null && newModuleCustomAttributes != null)
			{
				module.CustomAttributes.Clear();
				foreach (var ca in newModuleCustomAttributes)
					module.CustomAttributes.Add(ca);
			}
			if (importer.NewResources.Length != 0)
			{
				foreach (Resource res in importer.NewResources)
				{
					module.Resources.Add(res);
				}
			}
		}
	}
}