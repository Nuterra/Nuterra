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
		struct MemberInfo<T> where T : IMemberDef
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
