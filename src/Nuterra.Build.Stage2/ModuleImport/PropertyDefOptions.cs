using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace Nuterra.Build.ModuleImport
{
	internal sealed class PropertyDefOptions
	{
		public PropertyAttributes Attributes;
		public UTF8String Name;
		public PropertySig PropertySig;
		public Constant Constant;
		public List<MethodDef> GetMethods = new List<MethodDef>();
		public List<MethodDef> SetMethods = new List<MethodDef>();
		public List<MethodDef> OtherMethods = new List<MethodDef>();
		public List<CustomAttribute> CustomAttributes = new List<CustomAttribute>();

		public PropertyDefOptions()
		{
		}

		public PropertyDefOptions(PropertyDef prop)
		{
			Attributes = prop.Attributes;
			Name = prop.Name;
			PropertySig = prop.PropertySig;
			Constant = prop.Constant;
			GetMethods.AddRange(prop.GetMethods);
			SetMethods.AddRange(prop.SetMethods);
			OtherMethods.AddRange(prop.OtherMethods);
			CustomAttributes.AddRange(prop.CustomAttributes);
		}

		public PropertyDef CopyTo(PropertyDef prop)
		{
			prop.Attributes = Attributes;
			prop.Name = Name ?? UTF8String.Empty;
			prop.PropertySig = PropertySig;
			prop.Constant = Constant;
			prop.GetMethods.Clear();
			prop.GetMethods.AddRange(GetMethods);
			prop.SetMethods.Clear();
			prop.SetMethods.AddRange(SetMethods);
			prop.OtherMethods.Clear();
			prop.OtherMethods.AddRange(OtherMethods);
			prop.CustomAttributes.Clear();
			prop.CustomAttributes.AddRange(CustomAttributes);
			return prop;
		}

		public PropertyDef CreatePropertyDef(ModuleDef ownerModule) => ownerModule.UpdateRowId(CopyTo(new PropertyDefUser()));

		public static PropertyDefOptions Create(ModuleDef module, UTF8String name, bool isInstance)
		{
			return new PropertyDefOptions
			{
				Attributes = 0,
				Name = name,
				PropertySig = isInstance ?
								PropertySig.CreateInstance(module.CorLibTypes.Int32) :
								PropertySig.CreateStatic(module.CorLibTypes.Int32),
				Constant = null,
			};
		}
	}
}