Editting the assembly is done using a tool such as dnSpy

1. Make internal:
	ModuleDrill.m_Spinning
	ModuleEnergy.m_OutputConditions
	ModuleEnergy.m_ThermalSourceInRange
	ModuleEnergy.m_OutputPerSecond
	ModuleEnergy.m_OutputEnergyType
	ModuleEnergy.m_AnimatorController
	ModuleEnergy.m_GeneratingEnergyBool
	ModuleEnergy.CheckOutputConditions()
	ModuleEnergy.set_IsGenerating
	ModuleHammer.state
	ModuleItemHolderMagnet.m_Holder
	ModuleItemHolderMagnet.m_Picker
	ModuleItemHolderMagnet.m_SettlingSpeedThreshold
	ModuleItemHolderMagnet.m_PickupDelayTimeout
	ModuleItemHolderMagnet.m_SettleThresholdSqr
	ModuleItemHolderMagnet.UnglueAllObjects()
	ModuleItemHolderMagnet.UpdateItemMovement()
	ModuleScoop.lifted
	ModuleScoop.lift
	ModuleScoop.drop
	ModuleWeapon.m_TargetPosition
	ManSplashScreen.m_SplashScreenIndex
	ManSplashScreen.m_MyCanvas

2. Bake code into assembly
	
	dnSpy:
		Merge all the code into one big file. (important: using statements must be at the top of the file)
		Right-click the "Assembly-CSharp.dll" module and choose "Add Class (C#)..."
		Paste all the code into the dialog and compile it.

3. Hook ManUI.Start to bootstrap mod
void ManUI.Start()
{
	Maritaria.Mod.Init();
	//Remaining ManUI.Start code
}

4. Hook Module code
	Naming
	The modules that need to be hooked are located inside the "Maritaria.Modules" class.
	Inside this class are nested static classes that contain the overrides that need to be called.
	The mapping of methods is based on their names, the name of an overriding method is the same as in the original module.
	
	The type that has to be hooked is indicated for each module in the mod files.
	
	

5. Done