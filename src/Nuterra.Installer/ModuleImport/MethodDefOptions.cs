using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.PE;

namespace Nuterra.Installer.ModuleImport
{
	sealed class MethodDefOptions
	{
		public MethodImplAttributes ImplAttributes;
		public MethodAttributes Attributes;
		public MethodSemanticsAttributes SemanticsAttributes;
		public RVA RVA;
		public UTF8String Name;
		public MethodSig MethodSig;
		public ImplMap ImplMap;
		public List<CustomAttribute> CustomAttributes = new List<CustomAttribute>();
		public List<DeclSecurity> DeclSecurities = new List<DeclSecurity>();
		public List<ParamDef> ParamDefs = new List<ParamDef>();
		public List<GenericParam> GenericParameters = new List<GenericParam>();
		public List<MethodOverride> Overrides = new List<MethodOverride>();

		public MethodDefOptions()
		{
		}

		public MethodDefOptions(MethodDef method)
		{
			ImplAttributes = method.ImplAttributes;
			Attributes = method.Attributes;
			SemanticsAttributes = method.SemanticsAttributes;
			RVA = method.RVA;
			Name = method.Name;
			MethodSig = method.MethodSig;
			ImplMap = method.ImplMap;
			CustomAttributes.AddRange(method.CustomAttributes);
			DeclSecurities.AddRange(method.DeclSecurities);
			ParamDefs.AddRange(method.ParamDefs);
			GenericParameters.AddRange(method.GenericParameters);
			Overrides.AddRange(method.Overrides);
		}

		public MethodDef CopyTo(MethodDef method)
		{
			method.ImplAttributes = ImplAttributes;
			method.Attributes = Attributes;
			method.SemanticsAttributes = SemanticsAttributes;
			method.RVA = RVA;
			method.Name = Name ?? UTF8String.Empty;
			method.MethodSig = MethodSig;
			method.ImplMap = ImplMap;
			method.CustomAttributes.Clear();
			method.CustomAttributes.AddRange(CustomAttributes);
			method.DeclSecurities.Clear();
			method.DeclSecurities.AddRange(DeclSecurities);
			method.ParamDefs.Clear();
			method.ParamDefs.AddRange(ParamDefs);
			method.GenericParameters.Clear();
			method.GenericParameters.AddRange(GenericParameters);
			method.Overrides.Clear();
			method.Overrides.AddRange(Overrides.Select(e => e.MethodBody != null ? e : new MethodOverride(method, e.MethodDeclaration)));
			method.Parameters.UpdateParameterTypes();
			return method;
		}

		public MethodDef CreateMethodDef(ModuleDef ownerModule) => ownerModule.UpdateRowId(CopyTo(new MethodDefUser()));

		public static MethodDefOptions Create(UTF8String name, MethodSig methodSig)
		{
			return new MethodDefOptions
			{
				ImplAttributes = MethodImplAttributes.IL | MethodImplAttributes.Managed,
				Attributes = MethodAttributes.Public | MethodAttributes.ReuseSlot | MethodAttributes.HideBySig | (methodSig.HasThis ? 0 : MethodAttributes.Static),
				Name = name,
				MethodSig = methodSig,
				ImplMap = null,
			};
		}
	}
}
