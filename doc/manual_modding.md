Editting the assembly is done using a tool such as dnSpy

# Change private to internal
Change the following fields, properties and methods from `private` to `internal` (in dnSpy this level is called `assembly`)

- ModuleDrill.m_Spinning
- ModuleEnergy.m_OutputConditions
- ModuleEnergy.m_ThermalSourceInRange
- ModuleEnergy.m_OutputPerSecond
- ModuleEnergy.m_OutputEnergyType
- ModuleEnergy.m_AnimatorController
- ModuleEnergy.m_GeneratingEnergyBool
- ModuleEnergy.CheckOutputConditions()
- ModuleEnergy.set_IsGenerating
- ModuleHammer.state
- ModuleItemHolderMagnet.m_Holder
- ModuleItemHolderMagnet.m_Picker
- ModuleItemHolderMagnet.m_SettlingSpeedThreshold
- ModuleItemHolderMagnet.m_PickupDelayTimeout
- ModuleItemHolderMagnet.m_SettleThresholdSqr
- ModuleItemHolderMagnet.UnglueAllObjects()
- ModuleItemHolderMagnet.UpdateItemMovement()
- ModuleScoop.lifted
- ModuleScoop.lift
- ModuleScoop.drop
- ModuleWeapon.m_TargetPosition
- ManSplashScreen.m_SplashScreenIndex
- ManSplashScreen.m_MyCanvas

# Bake code into assembly
## dnSpy
1. Merge all the code into one big file. (important: using statements must be at the top of the file)
2. Right-click the "Assembly-CSharp.dll" module and choose "Add Class (C#)..."
3. Paste all the code into the dialog and compile it.

# Hook ManUI.Start to bootstrap mod
Change the `ManUI.Start()` method to become:
```
void ManUI.Start()
{
	Maritaria.Mod.Init();
	//Remaining ManUI.Start code
}
```
## dnSpy
1. In dnSpy, right-click the method and choose "Edit IL instructions..."
2. Insert a `call` IL instruction at the beginning of the list and make it call `void Maritaria.Mod.Init()`

# Hook module code
For each of the module overrides in the `Maritaria.Modules` class, the original methods of the game have to be redirected to the modded ones. This is done by finding the original method, clearing it's method body and calling the modded method, returning it's result if required. For the `InputControl` overrides, fire has to be changed to a boolean using the following expression: `(fire != 0)`.

## dnSpy
1. Find the original method, right-click and choose "Edit Method (C#)..."
2. Replace the method body with a call to the static method in the mod code.
3. Pass `null` when required to pass the current instance. This is a required workaround because of compiler issues.
4. Edit the IL instructions and change the `ldnull` IL OpCode to `ldarg.0`. If correct the decompiler should now show that `this` is being passed to the mod instead of `null`.
