using System;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Nuterra.Installer
{
	internal static class Hooker
	{
		public static void Apply(AssemblyDef assembly)
		{
			//Booting Nuterra
			Redirect(assembly, typeof(ManUI), typeof(Nuterra), new RedirectSettings(nameof(Nuterra.Start)) { PassArguments = false });

			//Module overrides
			Redirect(assembly, typeof(ModuleDrill), typeof(Maritaria.Modules.Drill), new RedirectSettings(nameof(Maritaria.Modules.Drill.ControlInput)) { ReplaceBody = true });
			Redirect(assembly, typeof(ModuleEnergy), typeof(Maritaria.Modules.Energy), new RedirectSettings(nameof(Maritaria.Modules.Energy.OnUpdateSupplyEnergy)) { ReplaceBody = true });
			Redirect(assembly, typeof(ModuleEnergy), typeof(Maritaria.Modules.Energy), new RedirectSettings(nameof(Maritaria.Modules.Energy.CheckOutputConditions)) { ReplaceBody = true });
			Redirect(assembly, typeof(ModuleHammer), typeof(Maritaria.Modules.Hammer), new RedirectSettings(nameof(Maritaria.Modules.Hammer.ControlInput)) { ReplaceBody = true });
			Redirect(assembly, typeof(ModuleItemHolderMagnet), typeof(Maritaria.Modules.Magnet), new RedirectSettings(nameof(Maritaria.Modules.Magnet.FixedUpdate)) { ReplaceBody = true });
			Redirect(assembly, typeof(ModuleScoop), typeof(Maritaria.Modules.Scoop), new RedirectSettings(nameof(Maritaria.Modules.Scoop.ControlInput)) { ReplaceBody = true });
			Redirect(assembly, typeof(ModuleWeapon), typeof(Maritaria.Modules.Weapon), new RedirectSettings(nameof(Maritaria.Modules.Weapon.ControlInputManual)) { ReplaceBody = true });

			//Creating custom blocks
			Redirect(assembly, typeof(ManSpawn), typeof(Maritaria.BlockLoader.Hooks_ManSpawn), new RedirectSettings(nameof(Maritaria.BlockLoader.Hooks_ManSpawn.Start)) { PassArguments = false, AppendToEnd = true });
			Redirect(assembly, typeof(BlockUnlockTable), typeof(Maritaria.BlockLoader.Hooks_BlockUnlockTable), new RedirectSettings(nameof(Maritaria.BlockLoader.Hooks_BlockUnlockTable.Init)));
			Redirect(assembly, typeof(ManLicenses), typeof(Maritaria.BlockLoader.Hooks_ManLicenses), new RedirectSettings(nameof(Maritaria.BlockLoader.Hooks_ManLicenses.SetupLicenses)));

			Redirect(assembly, typeof(ManStats.IntStatList), typeof(Maritaria.BlockLoader.Hooks_IntStatList), new RedirectSettings(nameof(Maritaria.BlockLoader.Hooks_IntStatList.OnSerializing)) { ReplaceBody = true });

			Hook_StringLookup_GetString(assembly);
			Hook_SpriteFetcher_GetSprite(assembly);
			Hook_BugReportFlagger(assembly);
			Hook_TankCamera_FixedUpdate(assembly);

			Redirect(assembly, typeof(ModuleItemPickup), typeof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleItemPickup), new RedirectSettings(nameof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleItemPickup.OnSpawn)) { });
			Redirect(assembly, typeof(ModuleItemPickup), typeof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleItemPickup), new RedirectSettings(nameof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleItemPickup.OnAttach)) { InsertionStart = 3 });//First 3 instructions are to set IsEnabled, which the hook overrides later
			Hook_ModuleHeart_IsOnline(assembly);

			Redirect(assembly, typeof(ManSaveGame.State), typeof(Maritaria.SaveGameFlagger), new RedirectSettings(".ctor") { TargetMethod = nameof(Maritaria.SaveGameFlagger.ManSaveGame_State_ctor) });
			Redirect(assembly, typeof(ManSaveGame.SaveData), typeof(Maritaria.SaveGameFlagger), new RedirectSettings("OnDeserialized") { TargetMethod = nameof(Maritaria.SaveGameFlagger.ManSaveGame_SaveData_OnDeserialized) });
		}

		private static void Redirect(AssemblyDef assembly, Type sourceType, Type targetType, RedirectSettings settings)
		{
			TypeDef cecilSource = assembly.GetTerraTechType(sourceType);
			MethodDef sourceMethod = cecilSource.Methods.Single(m => m.Name == settings.SourceMethod);
			TypeDef cecilTarget = assembly.GetNuterraType(targetType);
			MethodDef targetMethod = cecilTarget.Methods.Single(m => m.Name == settings.TargetMethod);

			var body = sourceMethod.Body.Instructions;
			if (settings.ReplaceBody)
			{
				body.Clear();
				body.Add(OpCodes.Ret.ToInstruction());
			}
			int insertedInstructionCounter = settings.InsertionStart;//TODO: Use insertbefore instead
			if (settings.AppendToEnd)
			{
				if (body.Count > 0)
				{
					insertedInstructionCounter = body.Count - 1;//Last IL before ret instruction
				}
			}
			if (settings.PassArguments)
			{
				int paramOffset = 0;
				if (sourceMethod.HasThis)
				{
					body.Insert(insertedInstructionCounter++, GetLoadArgOpCode(0));
					paramOffset = 1;
				}
				for (int i = 0; i < sourceMethod.Parameters.Count; i++)
				{
					body.Insert(insertedInstructionCounter++, GetLoadArgOpCode(processor, paramOffset + i));
				}
			}
			body.Insert(insertedInstructionCounter++, processor.Create(OpCodes.Call, targetMethod));
		}

		private static Instruction GetLoadArgOpCode(int i)
		{
			switch (i)
			{
				case 0: return OpCodes.Ldarg_0.ToInstruction();
				case 1: return OpCodes.Ldarg_1.ToInstruction();
				case 2: return OpCodes.Ldarg_2.ToInstruction();
				case 3: return OpCodes.Ldarg_3.ToInstruction();
				default: return OpCodes.Ldarg.ToInstruction(i);
			}
		}

		private static void Hook_StringLookup_GetString(AssemblyDef assembly)
		{
			TypeDef cecilSource = assembly.GetTerraTechType(typeof(StringLookup));
			MethodDef sourceMethod = cecilSource.Methods.Single(m => m.Name == "GetString");
			TypeDef cecilTarget = assembly.GetNuterraType(typeof(Maritaria.BlockLoader.Hooks_StringLookup));
			MethodDef targetMethod = cecilTarget.Methods.Single(m => m.Name == nameof(Maritaria.BlockLoader.Hooks_StringLookup.GetString));

			var body = sourceMethod.Body.Instructions;
			var processor = sourceMethod.Body.GetILProcessor();

			var originalMethodStart = processor.Body.Instructions.First();
			int index = 0;
			body.Insert(index++, processor.Create(OpCodes.Ldarg_0));
			body.Insert(index++, processor.Create(OpCodes.Ldarg_2));
			body.Insert(index++, processor.Create(OpCodes.Call, targetMethod));
			body.Insert(index++, processor.Create(OpCodes.Stloc_0));
			body.Insert(index++, processor.Create(OpCodes.Ldloc_0));
			body.Insert(index++, processor.Create(OpCodes.Brfalse_S, originalMethodStart));
			body.Insert(index++, processor.Create(OpCodes.Ldloc_0));
			body.Insert(index++, processor.Create(OpCodes.Ret));

			/*
				0	0000	ldarg.0
				1	0001	ldarg.2
				2	0002	call		string Maritaria.BlockLoader::StringLookup_GetString(int32, valuetype LocalisationEnums/StringBanks)
				3	0007	stloc.0
				4	0008	ldloc.0
				5	0009	brfalse.s	{ original method start instruction }
				6	000B	ldloc.0
				7	000C	ret
				... remaining method code ...
			 */
		}

		private static void Hook_SpriteFetcher_GetSprite(AssemblyDef assembly)
		{
			TypeDef cecilSource = assembly.GetTerraTechType(typeof(SpriteFetcher));
			MethodDef sourceMethod = cecilSource.Methods.Single(m => m.FullName == "UnityEngine.Sprite SpriteFetcher::GetSprite(ObjectTypes,System.Int32)");
			TypeDef cecilTarget = assembly.GetNuterraType(typeof(Maritaria.BlockLoader.Hooks_SpriteFetcher));
			MethodDef targetMethod = cecilTarget.Methods.Single(m => m.Name == nameof(Maritaria.BlockLoader.Hooks_SpriteFetcher.GetSprite));

			AssemblyDef unityEngine = assembly.ManifestModule.GetAssemblyRef(new UTF8String("UnityEngine"));
			TypeDef unityEngine_Object = unityEngine.MainModule.GetType("UnityEngine.Object");
			MethodReference equalityCheck = unityEngine_Object.Methods.Single(m => m.FullName == "System.Boolean UnityEngine.Object::op_Inequality(UnityEngine.Object,UnityEngine.Object)");

			var body = sourceMethod.Body.Instructions;
			var processor = sourceMethod.Body.GetILProcessor();

			var equalityCheckImported = assembly.MainModule.Import(equalityCheck);

			var originalMethodStart = processor.Body.Instructions.First();
			int index = 0;
			body.Insert(index++, processor.Create(OpCodes.Ldarg_1));
			body.Insert(index++, processor.Create(OpCodes.Ldarg_2));
			body.Insert(index++, processor.Create(OpCodes.Call, targetMethod));
			body.Insert(index++, processor.Create(OpCodes.Stloc_0));
			body.Insert(index++, processor.Create(OpCodes.Ldloc_0));
			body.Insert(index++, processor.Create(OpCodes.Ldnull));
			body.Insert(index++, processor.Create(OpCodes.Call, equalityCheckImported));
			body.Insert(index++, processor.Create(OpCodes.Brfalse_S, originalMethodStart));
			body.Insert(index++, processor.Create(OpCodes.Ldloc_0));
			body.Insert(index++, processor.Create(OpCodes.Ret));
			/*
				0	0000	ldarg.1
				1	0001	ldarg.2
				2	0002	call		class [UnityEngine]UnityEngine.Sprite Maritaria.BlockLoader::SpriteFetcher_GetSprite(valuetype ObjectTypes, int32)
				3	0007	stloc.0
				4	0008	ldloc.0
				5	0009	ldnull
				6	000A	call		bool [UnityEngine]UnityEngine.Object::op_Inequality(class [UnityEngine]UnityEngine.Object, class [UnityEngine]UnityEngine.Object)
				7	000F	brfalse.s	{ original method start instruction }
				8	0011	ldloc.0
				9	0012	ret
				... remaining method code ...
			 */
		}

		private static void Hook_BugReportFlagger(AssemblyDef assembly)
		{
			TypeDef bugReportFlagger = assembly.GetNuterraType(typeof(Maritaria.BugReportFlagger));
			MethodDef markReportForm = bugReportFlagger.Methods.Single(m => m.Name == nameof(Maritaria.BugReportFlagger.MarkReportForm));
			MethodDef markUserMessage = bugReportFlagger.Methods.Single(m => m.Name == nameof(Maritaria.BugReportFlagger.MarkUserMessage));

			TypeDef bugReporter = assembly.GetTerraTechType(typeof(UIScreenBugReport));
			TypeDef postIterator = bugReporter.NestedTypes.Single(t => t.FullName == "UIScreenBugReport/<PostIt>c__Iterator78");
			MethodDef moveNext = postIterator.Methods.Single(m => m.Name == "MoveNext");
			FieldDefinition bodyField = bugReporter.Fields.Single(f => f.Name == "m_Body");

			var body = moveNext.Body.Instructions;
			var processor = moveNext.Body.GetILProcessor();

			Instruction wwwFormInit = body.First(i => i.OpCode == OpCodes.Newobj);
			Instruction storeWwwForm = wwwFormInit.Next;
			FieldDefinition wwwFormField = (FieldDefinition)storeWwwForm.Operand;
			int index = body.IndexOf(storeWwwForm) + 1;
			//TODO: work away index u sing InsertAfter
			body.Insert(index++, processor.Create(OpCodes.Ldarg_0));
			body.Insert(index++, processor.Create(OpCodes.Ldfld, wwwFormField));
			body.Insert(index++, processor.Create(OpCodes.Call, markReportForm));

			Instruction loadBodyField = body.First(i => i.OpCode == OpCodes.Ldfld && i.Operand == bodyField);
			Instruction readBodyText = loadBodyField.Next;
			processor.InsertAfter(readBodyText, processor.Create(OpCodes.Call, markUserMessage));
		}

		public static void Hook_TankCamera_FixedUpdate(AssemblyDef assembly)
		{
			const string methodName = nameof(Maritaria.FirstPersonKeyBehaviour.Hooks_TankCamera.FixedUpdate);
			TypeDef cecilSource = assembly.GetTerraTechType(typeof(TankCamera));
			MethodDef sourceMethod = cecilSource.Methods.Single(m => m.Name == methodName);
			TypeDef cecilTarget = assembly.GetNuterraType(typeof(Maritaria.FirstPersonKeyBehaviour.Hooks_TankCamera));
			MethodDef targetMethod = cecilTarget.Methods.Single(m => m.Name == methodName);

			var body = sourceMethod.Body.Instructions;
			var processor = sourceMethod.Body.GetILProcessor();

			Instruction injectBeforeInstruction = body[472];

			//Insert call to something that returns a bool
			Instruction callTargetInstruction = processor.Create(OpCodes.Call, targetMethod);
			processor.InsertBefore(injectBeforeInstruction, callTargetInstruction);

			//Skip next instruction if bool is true (jumping to 'injectBeforeInstruction')
			Instruction branchInstruction = processor.Create(OpCodes.Brtrue_S, injectBeforeInstruction);
			processor.InsertBefore(injectBeforeInstruction, branchInstruction);

			//Return method, except if the previous call returns true
			Instruction returnInstruction = processor.Create(OpCodes.Ret);
			processor.InsertBefore(injectBeforeInstruction, returnInstruction);

			/*
				... instruction #467 ...
				call		Maritaria.FirstPersonKeyBehaviour.Hooks_TankCamera::FixedUpdate()
				brtrue.s	{ original next instruction }
				ret
				... remaining code ...

				Make sure to replace jumps to #467 to the first injected opcode
			 */
		}

		private static void Hook_ModuleHeart_IsOnline(AssemblyDef assembly)
		{
			const string methodName = nameof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleHeart.get_IsOnline);
			TypeDef cecilSource = assembly.GetTerraTechType(typeof(ModuleHeart));
			MethodDef sourceMethod = cecilSource.Methods.Single(m => m.Name == methodName);
			TypeDef cecilTarget = assembly.GetNuterraType(typeof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleHeart));
			MethodDef targetMethod = cecilTarget.Methods.Single(m => m.Name == methodName);

			var body = sourceMethod.Body.Instructions;
			var processor = sourceMethod.Body.GetILProcessor();

			Instruction returnFalseStartInstruction = body[15];

			processor.Replace(body[13], processor.Create(OpCodes.Ble_Un_S, returnFalseStartInstruction));
			processor.Replace(body[14], processor.Create(OpCodes.Ldarg_0));
			processor.InsertBefore(returnFalseStartInstruction, processor.Create(OpCodes.Call, targetMethod));
			processor.InsertBefore(returnFalseStartInstruction, processor.Create(OpCodes.Ret));

			/*
			Before:
			13	0035	cgt
			14	0037	br.s	16 (003A) ret
			15	0039	ldc.i4.0
			16	003A	ret
			After:
			13	002F	ble.un.s	17 (0038) ldc.i4.0
			14	0031	ldnull
			15	0032	call	bool Maritaria.ProductionToggleKeyBehaviour::ModuleHeart_get_IsOnline(class ModuleHeart)
			16	0037	ret
			17	0038	ldc.i4.0
			18	0039	ret
			*/
		}
	}
}