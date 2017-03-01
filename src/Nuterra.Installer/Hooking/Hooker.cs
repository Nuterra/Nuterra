using System;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nuterra.Internal;

namespace Nuterra.Installer.Hooking
{
	internal static class Hooker
	{
		public static void Apply(ModuleDefMD module)
		{
			//Booting Nuterra
			Redirect(module, "ManUI", typeof(Bootstrapper), new RedirectSettings(nameof(Bootstrapper.Startup)) { PassArguments = false });

			//Modded game flagging
			Hook_BugReportFlagger(module);

			//Manager events
			Redirect(module, "ManLicenses", typeof(Hooks.Managers.Licenses), new RedirectSettings(nameof(Hooks.Managers.Licenses.Init)));
			Redirect(module, "ManPointer", typeof(Hooks.Managers.Pointer), new RedirectSettings(nameof(Hooks.Managers.Pointer.StartCameraSpin)) { AppendToEnd = true });
			Redirect(module, "ManPointer", typeof(Hooks.Managers.Pointer), new RedirectSettings(nameof(Hooks.Managers.Pointer.StopCameraSpin)) { AppendToEnd = true });

			//Module events
			Redirect(module, "ModuleDrill", typeof(Hooks.Modules.Drill), new RedirectSettings(nameof(Hooks.Modules.Drill.ControlInput)) { ReplaceBody = true });
			Redirect(module, "ModuleEnergy", typeof(Hooks.Modules.Energy), new RedirectSettings(nameof(Hooks.Modules.Energy.OnUpdateSupplyEnergy)) { ReplaceBody = true });
			Redirect(module, "ModuleHammer", typeof(Hooks.Modules.Hammer), new RedirectSettings(nameof(Hooks.Modules.Hammer.ControlInput)) { ReplaceBody = true });
			Redirect(module, "ModuleScoop", typeof(Hooks.Modules.Scoop), new RedirectSettings(nameof(Hooks.Modules.Scoop.ControlInput)) { ReplaceBody = true });
			Redirect(module, "ModuleWeapon", typeof(Hooks.Modules.Weapon), new RedirectSettings(nameof(Hooks.Modules.Weapon.ControlInputManual)) { ReplaceBody = true });

			//Custom block support
			Redirect(module, "ManStats+IntStatList", typeof(Hooks.Managers.Stats.IntStatList), new RedirectSettings(nameof(Hooks.Managers.Stats.IntStatList.OnSerializing)) { ReplaceBody = true });
			Hook_StringLookup_GetString(module);
			Hook_SpriteFetcher_GetSprite(module);

			//Misc
			Redirect(module, "ModuleItemPickup", typeof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleItemPickup), new RedirectSettings(nameof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleItemPickup.OnSpawn)) { });
			Redirect(module, "ModuleItemPickup", typeof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleItemPickup), new RedirectSettings(nameof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleItemPickup.OnAttach)) { InsertionStart = 3 });//First 3 instructions are to set IsEnabled, which the hook overrides later
			Redirect(module, "ModuleHeart", typeof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleHeart), new RedirectSettings(nameof(Maritaria.ProductionToggleKeyBehaviour.Hooks_ModuleHeart.get_CanPowerUp)) { ReplaceBody = true });

			Redirect(module, "ManSaveGame+State", typeof(Maritaria.SaveGameFlagger), new RedirectSettings(".ctor") { TargetMethod = nameof(Maritaria.SaveGameFlagger.ManSaveGame_State_ctor) });
			Redirect(module, "ManSaveGame+SaveData", typeof(Maritaria.SaveGameFlagger), new RedirectSettings("OnDeserialized") { TargetMethod = nameof(Maritaria.SaveGameFlagger.ManSaveGame_SaveData_OnDeserialized) });

			Redirect(module, "Mode", typeof(Sylver.SylverMod.Hooks_Mode), new RedirectSettings(nameof(Sylver.SylverMod.Hooks_Mode.EnterMode)));

			Hook_TankControl_PlayerInput(module);
		}

		private static void Redirect(ModuleDefMD module, string sourceType, Type targetType, RedirectSettings settings)
		{
			TypeDef cecilSource = module.Find(sourceType, isReflectionName: true);
			MethodDef sourceMethod = cecilSource.Methods.Single(m => m.Name == settings.SourceMethod);
			TypeDef cecilTarget = module.GetNuterraType(targetType);
			MethodDef targetMethod = cecilTarget.Methods.Single(m => m.Name == settings.TargetMethod);

			var body = sourceMethod.Body.Instructions;

			if (settings.ReplaceBody)
			{
				sourceMethod.Body.Variables.Clear();
				sourceMethod.Body.ExceptionHandlers.Clear();
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
			int insertionStart = insertedInstructionCounter;
			if (settings.PassArguments)
			{
				for (int i = 0; i < sourceMethod.Parameters.Count; i++)
				{
					body.Insert(insertedInstructionCounter++, GetLoadArgOpCode(i));
				}
			}
			body.Insert(insertedInstructionCounter++, OpCodes.Call.ToInstruction(targetMethod));

			if (settings.AppendToEnd)
			{
				Instruction ret = body.Last();
				foreach (Instruction instr in body)
				{
					if (instr.Operand is Instruction)
					{
						Instruction jumpTarget = instr.Operand as Instruction;
						if (jumpTarget == ret)
						{
							instr.Operand = body[insertionStart];
						}
					}
				}
			}
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

		private static void Hook_StringLookup_GetString(ModuleDefMD module)
		{
			TypeDef cecilSource = module.Find("StringLookup", isReflectionName: true);
			MethodDef sourceMethod = cecilSource.Methods.Single(m => m.Name == "GetString");
			TypeDef cecilTarget = module.GetNuterraType(typeof(Maritaria.BlockLoader.Hooks_StringLookup));
			MethodDef targetMethod = cecilTarget.Methods.Single(m => m.Name == nameof(Maritaria.BlockLoader.Hooks_StringLookup.GetString));

			var body = sourceMethod.Body.Instructions;

			var originalMethodStart = body.First();
			int index = 0;
			body.Insert(index++, OpCodes.Ldarg_0.ToInstruction());
			body.Insert(index++, OpCodes.Ldarg_2.ToInstruction());
			body.Insert(index++, OpCodes.Call.ToInstruction(targetMethod));
			body.Insert(index++, OpCodes.Stloc_0.ToInstruction());
			body.Insert(index++, OpCodes.Ldloc_0.ToInstruction());
			body.Insert(index++, OpCodes.Brfalse_S.ToInstruction(originalMethodStart));
			body.Insert(index++, OpCodes.Ldloc_0.ToInstruction());
			body.Insert(index++, OpCodes.Ret.ToInstruction());

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

		private static void Hook_SpriteFetcher_GetSprite(ModuleDefMD module)
		{
			TypeDef cecilSource = module.Find("SpriteFetcher", isReflectionName: true);
			MethodDef sourceMethod = cecilSource.Methods.Single(m => m.FullName == "UnityEngine.Sprite SpriteFetcher::GetSprite(ObjectTypes,System.Int32)");
			TypeDef cecilTarget = module.GetNuterraType(typeof(Maritaria.BlockLoader.Hooks_SpriteFetcher));
			MethodDef targetMethod = cecilTarget.Methods.Single(m => m.Name == nameof(Maritaria.BlockLoader.Hooks_SpriteFetcher.GetSprite));

			AssemblyRef unityEngine = module.GetAssemblyRef(new UTF8String("UnityEngine"));
			TypeRefUser unityEngine_Object = new TypeRefUser(module, new UTF8String("UnityEngine"), new UTF8String("Object"), unityEngine);
			TypeSig objectSig = unityEngine_Object.ToTypeSig();
			MethodSig op_Equality = MethodSig.CreateStatic(module.CorLibTypes.Boolean, objectSig, objectSig);
			MemberRefUser op_EqualityMethod = new MemberRefUser(module, new UTF8String("op_Inequality"), op_Equality, unityEngine_Object);

			var body = sourceMethod.Body.Instructions;
			var originalMethodStart = body.First();
			int index = 0;
			sourceMethod.Body.MaxStack = 6;
			body.Insert(index++, new Instruction(OpCodes.Ldarg_1));
			body.Insert(index++, new Instruction(OpCodes.Ldarg_2));
			body.Insert(index++, new Instruction(OpCodes.Call, targetMethod));
			body.Insert(index++, new Instruction(OpCodes.Stloc_0));
			body.Insert(index++, new Instruction(OpCodes.Ldloc_0));
			body.Insert(index++, new Instruction(OpCodes.Ldnull));
			body.Insert(index++, new Instruction(OpCodes.Call, op_EqualityMethod));
			body.Insert(index++, new Instruction(OpCodes.Brfalse_S, originalMethodStart));
			body.Insert(index++, new Instruction(OpCodes.Ldloc_0));
			body.Insert(index++, new Instruction(OpCodes.Ret));

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

		private static void Hook_BugReportFlagger(ModuleDefMD module)
		{
			TypeDef bugReportFlagger = module.GetNuterraType(typeof(Maritaria.BugReportFlagger));
			MethodDef markReportForm = bugReportFlagger.Methods.Single(m => m.Name == nameof(Maritaria.BugReportFlagger.MarkReportForm));
			MethodDef markUserMessage = bugReportFlagger.Methods.Single(m => m.Name == nameof(Maritaria.BugReportFlagger.MarkUserMessage));

			TypeDef bugReporter = module.Find("UIScreenBugReport", isReflectionName: true);
			TypeDef postIterator = bugReporter.NestedTypes.Single(t => t.FullName == "UIScreenBugReport/<PostIt>c__Iterator78");
			MethodDef moveNext = postIterator.Methods.Single(m => m.Name == "MoveNext");

			FieldDef bodyField = bugReporter.Fields.Single(f => f.Name == "m_Body");

			var body = moveNext.Body.Instructions;

			Instruction formInit = body.First(i => i.OpCode == OpCodes.Newobj);
			int index = body.IndexOf(formInit);
			Instruction storeForm = body[index + 1];
			FieldDef formField = (FieldDef)storeForm.Operand;
			index += 2;
			body.Insert(index++, new Instruction(OpCodes.Ldarg_0));
			body.Insert(index++, new Instruction(OpCodes.Ldfld, formField));
			body.Insert(index++, new Instruction(OpCodes.Call, markReportForm));

			Instruction loadBodyField = body.First(i => i.OpCode == OpCodes.Ldfld && i.Operand == bodyField);
			index = body.IndexOf(loadBodyField);
			Instruction readBodyText = body[index + 1];
			body.Insert(index + 2, new Instruction(OpCodes.Call, markUserMessage));
		}

		public static void Hook_TankControl_PlayerInput(ModuleDefMD module)
		{
			const string methodName = nameof(Maritaria.FirstPerson.FirstPersonController.Hooks_TankControl.PlayerInput);
			TypeDef cecilSource = module.Find("TankControl", isReflectionName: true);
			MethodDef sourceMethod = cecilSource.Methods.Single(m => m.Name == methodName);
			TypeDef cecilTarget = module.GetNuterraType(typeof(Maritaria.FirstPerson.FirstPersonController.Hooks_TankControl));
			MethodDef targetMethod = cecilTarget.Methods.Single(m => m.Name == methodName);

			var body = sourceMethod.Body.Instructions;

			for (int i = 0; i < 9; i++)
			{
				body.RemoveAt(0);
			}

			body.Insert(0, new Instruction(OpCodes.Ldarg_0));
			body.Insert(1, new Instruction(OpCodes.Call, targetMethod));

			/*
				0	0000	ldsfld	!0 class Singleton/Manager`1<class CameraManager>::inst
				1	0005	callvirt	instance bool CameraManager::IsCurrent<class TankCamera>()
				2	000A	brtrue	10 (0032) ldarg.0
				3	000F	ldsfld	!0 class Singleton/Manager`1<class CameraManager>::inst
				4	0014	callvirt	instance bool CameraManager::IsCurrent<class DebugCamera>()
				5	0019	brfalse	50 (00B6) ret
				6	001E	ldsfld	!0 class Singleton/Manager`1<class CameraManager>::inst
				7	0023	callvirt	instance class DebugCamera CameraManager::GetDebugCamera()
				8	0028	callvirt	instance bool DebugCamera::get_IsLocked()
				9	002D	brfalse	50 (00B6) ret
				... remaining code ...

				Remove all instructions up to 9, then insert:
				load self
				call hook

			So it becomes:
			call hook
			if hook returns false then jump to 50

			 */
		}
	}
}