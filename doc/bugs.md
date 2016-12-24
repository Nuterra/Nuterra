# Potential bugs in Mod

# Potential bugs in TerraTech

## ManSplashScreen.get_CanvasTrans()
Can throw NullReferenceException if m_MyCanvas is null

## ManSpawn.AddBlocksToTech(ref Tank, TechData, int[])
No try-catch that ensures restore of `ModuleAnchor.AnchorOnAttach` and `ModuleItemConveyor.LinkOnAttach`.
