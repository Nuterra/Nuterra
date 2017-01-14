using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Nuterra.InstallProcess
{
	internal static class ReflectionExtensions
	{
		internal static string GetCecilFullName(this Type type)
		{
			return type.FullName.Replace('+', '/');
		}

		internal static TypeDefinition GetNuterraType(this AssemblyDefinition asm, Type type)
		{
			return asm.MainModule.GetType(type.GetCecilFullName());
		}

		internal static TypeDefinition GetTerraTechType(this AssemblyDefinition asm, Type type)
		{
			return asm.MainModule.GetType(type.GetCecilFullName());
		}

		internal static void FormatInstructions(this ILProcessor processor)
		{
			var body = processor.Body.Instructions;
			int totalOffset = 0;
			foreach (Instruction instruction in body)
			{
				instruction.Offset = totalOffset;
				totalOffset += instruction.GetSize();
			}

		}
	}
}