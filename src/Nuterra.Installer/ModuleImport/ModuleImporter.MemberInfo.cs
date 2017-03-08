using System;
using dnlib.DotNet;

namespace Nuterra.Installer.ModuleImport
{
	partial class ModuleImporter
	{
		private struct MemberInfo<T> where T : IMemberDef
		{
			public T TargetMember { get; }
			public T EditedMember { get; }

			public MemberInfo(T targetMember, T editedMember)
			{
				TargetMember = targetMember;
				EditedMember = editedMember;
			}
		}
	}
}