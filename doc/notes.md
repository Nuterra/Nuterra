# Custom blocks
## Block creation
- `ManSpawn.AddBlocksToTech` uses `m_BlockAliasing` to translate obsolete `BlockTypes` values to new ones.
- `ManSpawn.SpawnBlock` uses `m_BlockPrefabs` to create new block instances.

## Block shop population
- Block shop: `UIShopBlockSelect`
- Available blocks are shown by `UIBlockSelectGrid`, which uses `ManLicense` to determine the blocks that are available.
- The list for all known blocks in the shop originates from `ManLicenses.GetAllBlocksForFaction(FactionSubTypes)`.

## Attachment points
- `TankBlock.attachPoints` is a `Vector3[]` containing the points the block can be connected on.
