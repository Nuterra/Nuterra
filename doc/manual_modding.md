Editting the assembly is done using a tool such as dnSpy

# Inserting code
First compile all the existing code into the assembly. Then write the assembly to disk so the new types can be used from existing TerraTech code.

# Changing accessors
Some things need to have their accessors modified in order to give access to the newly compiled code.
Unfortunatly the list of things to change frequently changes and the best way to handle this step is to go through the code and check the accessor on all properties accessed.

# Redirect TerraTech to mod code
TerraTech code is changed by creating methods in the 

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
ModuleDrill.ControlInput() | Maritaria.Modules.Drill.Input() | Replace method body [#1]
ModuleEnergy.CheckOutputConditions() | Maritaria.Modules.Energy.CheckOutputConditions() | Replace method body
ModuleEnergy.OnUpdateSupplyEnergy() | Maritaria.Modules.Energy.OnUpdateSupplyEnergy() | Replace method body
ModuleHammer.ControlInput() | Maritaria.Modules.Hammer.Input() | Replace method body [#1]
ModuleItemHolderMagnet.FixedUpdate() | Maritaria.Modules.Magnet.FixedUpdate() | Replace method body
ModuleScoop.ControlInput() | Maritaria.Modules.Scoop.Input() | Replace method body [#1]
ModuleWeapon.ControlInputManual() | Maritaria.Modules.Weapon.Input() | Replace method body [#1]

Note #1: to transform fire to bool use: ```(fire != 0)```

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