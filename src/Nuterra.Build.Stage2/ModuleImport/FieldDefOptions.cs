using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.PE;

namespace Nuterra.Installer.ModuleImport
{
	internal sealed class FieldDefOptions
	{
		public FieldAttributes Attributes;
		public UTF8String Name;
		public FieldSig FieldSig;
		public uint? FieldOffset;
		public MarshalType MarshalType;
		public RVA RVA;
		public byte[] InitialValue;
		public ImplMap ImplMap;
		public Constant Constant;
		public List<CustomAttribute> CustomAttributes = new List<CustomAttribute>();

		public FieldDefOptions()
		{
		}

		public FieldDefOptions(FieldDef field)
		{
			Attributes = field.Attributes;
			Name = field.Name;
			FieldSig = field.FieldSig;
			FieldOffset = field.FieldOffset;
			MarshalType = field.MarshalType;
			RVA = field.RVA;
			InitialValue = field.InitialValue;
			ImplMap = field.ImplMap;
			Constant = field.Constant;
			CustomAttributes.AddRange(field.CustomAttributes);
		}

		public FieldDef CopyTo(FieldDef field)
		{
			field.Attributes = Attributes;
			field.Name = Name ?? UTF8String.Empty;
			field.FieldSig = FieldSig;
			field.FieldOffset = FieldOffset;
			field.MarshalType = MarshalType;
			field.RVA = RVA;
			field.InitialValue = InitialValue;
			field.ImplMap = ImplMap;
			field.Constant = Constant;
			field.CustomAttributes.Clear();
			field.CustomAttributes.AddRange(CustomAttributes);
			return field;
		}

		public FieldDef CreateFieldDef(ModuleDef ownerModule) => ownerModule.UpdateRowId(CopyTo(new FieldDefUser()));

		public static FieldDefOptions Create(UTF8String name, FieldSig fieldSig)
		{
			return new FieldDefOptions
			{
				Attributes = FieldAttributes.Public,
				Name = name,
				FieldSig = fieldSig,
				FieldOffset = null,
				MarshalType = null,
				RVA = 0,
				InitialValue = null,
				ImplMap = null,
				Constant = null,
			};
		}
	}
}