Editting the assembly is done using a tool such as dnSpy

# Change private to internal
Change the following fields, properties and methods from `private` to `internal` (in dnSpy this level is called `assembly`)

## BlockLoader
- ManSpawn.AddBlockToDictionary(GameObject)
- BlockUnlockTable.m_CorpBlockList
- class BlockUnlockTable.CorpBlockData
- class BlockUnlockTable.GradeData
- TankBlock.m_BlockCategory

## Modules.Drill
- ModuleDrill.m_Spinning

## Modules.Energy
- ModuleEnergy.set_IsGenerating
- ModuleEnergy.m_OutputPerSecond
- ModuleEnergy.CheckOutputConditions()
- ModuleEnergy.m_OutputEnergyType
- ModuleEnergy.m_AnimatorController
- ModuleEnergy.m_GeneratingEnergyBool
- ModuleEnergy.m_OutputConditions
- ModuleEnergy.m_ThermalSourceInRange

## Modules.Hammer
- ModuleHammer.state

## Modules.Magnet
- ModuleItemHolderMagnet.m_Holder
- ModuleItemHolderMagnet.UnglueAllObjects()
- ModuleItemHolderMagnet.m_PickupDelayTimeout
- ModuleItemHolderMagnet.m_Picker
- ModuleItemHolderMagnet.UpdateItemMovement()
- ModuleItemHolderMagnet.m_SettleThresholdSqr
- ModuleItemHolderMagnet.m_SettlingSpeedThreshold

## Modules.Scoop
- ModuleScoop.lifted
- ModuleScoop.lift
- ModuleScoop.drop

## Modules.Weapon
- ModuleWeapon.m_TargetPosition

## SplashScreenHandler
- ManSplashScreen.m_MyCanvas
- ManSplashScreen.m_SplashScreenIndex

# Bake code into assembly
## dnSpy
1. Merge all the code into one big file. (important: using statements must be at the top of the file)
2. Right-click the "Assembly-CSharp.dll" module and choose "Add Class (C#)..."
3. Paste all the code into the dialog and compile it.

## One-at-a-time bake order:
1. CleanLogger
2. GameObjectExtensions
3. ObjImporter
4. ModConfig
5. SplashScreenHandler
6. <write assembly to disk>
7. BlockLoader
9. Mod + MagnetToggleKeyBehaviour + Modules

# Hook existing code
Change the methods of existing TerraTech code to redirect to the mod.

## dnSpy
Due to a technical limitation in dnSpy, you cannot pass 'this' if the target method expects an instance of the class being editted.
To solve this, you need to pass null, so the compiler can succesfully compile the code. Then, edit the IL-opcodes to pass 'this' instead.
1. When editting the method, pass 'null' where 'this' should be passed.
2. Edit the IL instructions
3. Change the `ldnull` IL-opcode to `ldarg.0`, if correct the decompiler should now show that `this` is being passed to the mod instead of `null`.

Hooked method | Target mod method | When to call hook
--- | --- | ---
ManUI.Start() | Maritaria.Mod.Init() | Before method body
ManSpawn.Start() | Maritaria.BlockLoader.Init() | After method body
BlockUnlockTable.Init() | Maritaria.BlockLoader.BlockUnlockTable_Init() | Before method body
ManLicenses.SetupLicenses() | Maritaria.BlockLoader.ManLicenses_SetupLicenses() | Before method body
ModuleDrill.ControlInput() | Maritaria.Modules.Drill.Input() | Replace method body [note]
ModuleEnergy.CheckOutputConditions() | Maritaria.Modules.Energy.CheckOutputConditions() | Replace method body
ModuleEnergy.OnUpdateSupplyEnergy() | Maritaria.Modules.Energy.OnUpdateSupplyEnergy() | Replace method body
ModuleHammer.ControlInput() | Maritaria.Modules.Hammer.Input() | Replace method body [note]
ModuleItemHolderMagnet.FixedUpdate() | Maritaria.Modules.Magnet.FixedUpdate() | Replace method body
ModuleScoop.ControlInput() | Maritaria.Modules.Scoop.Input() | Replace method body [note]
ModuleWeapon.ControlInputManual() | Maritaria.Modules.Weapon.Input() | Replace method body [note]

Note: to transform fire to bool use: ```(fire != 0)```

## Example: ManUI.Start() -> Maritaria.Mod.Init()
If you cannot manage to get the method to compile, try the following:
1. In dnSpy, right-click the method and choose "Edit IL instructions..."
2. Insert a `call` IL instruction at the beginning of the list and make it call `void Maritaria.Mod.Init()`

```
void ManUI.Start()
{
	Maritaria.Mod.Init();
	//Remaining ManUI.Start code
}
```