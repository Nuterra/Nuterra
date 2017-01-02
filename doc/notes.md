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

## Filled blocks
- `TankBlock.filledCells' is a `Vector3[]` containing the voxels that are occupied by a given block.
- Simple 1x1x1 blocks will only list 0 0 0, but the GeoCorp 2x2x2 standard block lists the following:

## GameObject graph
The following two graphs show the layout of `GameObject`s used as prefabs for the GSO 1x1x1 block and GC 2x2x2 block.
```
GameObject name: (GSOBlock(111)) pos: (0.0, 0.0, 0.0)
	activeSelf: True activeInHierarchy: False
	layer: (8) tag: (TankBlock)
	Components:
	UnityEngine.Transform 
	TankBlock 
		attachPoints:
			- (0.5, 0.0, 0.0)
			- (0.0, 0.0, -0.5)
			- (0.0, 0.0, 0.5)
			- (-0.5, 0.0, 0.0)
			- (0.0, -0.5, 0.0)
			- (0.0, 0.5, 0.0)
		filledCells:
			- (0.0, 0.0, 0.0)
		partialCells:
		apGroups:
	Visible Bitfield`1[Visible+StateFlags]
	AutoSpriteRenderer 
	ModuleDamage 
	Damageable 
	GameObjects:
	GameObject name: (GSO_Block_111) pos: (0.0, 0.0, 0.0)
		activeSelf: True activeInHierarchy: False
		layer: (8) tag: (Untagged)
		Components:
		UnityEngine.Transform 
		UnityEngine.MeshFilter m_GSO_Block_111 Instance
		UnityEngine.MeshRenderer 
		UnityEngine.BoxCollider (0.0, 0.0, 0.0) (1.0, 1.0, 1.0)
		GameObjects:
		GameObject name: (m_GSO_Block_111_APs) pos: (0.0, 0.0, 0.0)
			activeSelf: True activeInHierarchy: False
			layer: (8) tag: (Untagged)
			Components:
			UnityEngine.Transform 
			UnityEngine.MeshFilter m_GSO_Block_111_APs Instance
			UnityEngine.MeshRenderer 
			GameObjects:
```

```
GameObject name: (GCBlock(222)) pos: (0.0, 0.0, 0.0)
	activeSelf: True activeInHierarchy: False
	layer: (8) tag: (TankBlock)
	Components:
	UnityEngine.Transform 
	TankBlock 
		attachPoints:
			- (0.0, 0.0, -0.5)
			- (-0.5, 0.0, 0.0)
			- (0.0, -0.5, 0.0)
			- (1.0, 0.0, -0.5)
			- (1.0, 1.0, -0.5)
			- (0.0, 1.0, -0.5)
			- (1.5, 1.0, 0.0)
			- (1.5, 1.0, 1.0)
			- (1.5, 0.0, 1.0)
			- (1.5, 0.0, 0.0)
			- (1.0, -0.5, 0.0)
			- (1.0, -0.5, 1.0)
			- (0.0, -0.5, 1.0)
			- (1.0, 1.0, 1.5)
			- (0.0, 1.0, 1.5)
			- (1.0, 0.0, 1.5)
			- (0.0, 0.0, 1.5)
			- (-0.5, 0.0, 1.0)
			- (-0.5, 1.0, 1.0)
			- (-0.5, 1.0, 0.0)
			- (0.0, 1.5, 0.0)
			- (0.0, 1.5, 1.0)
			- (1.0, 1.5, 1.0)
			- (1.0, 1.5, 0.0)
		filledCells:
			- (0.0, 0.0, 0.0)
			- (1.0, 0.0, 0.0)
			- (1.0, 0.0, 1.0)
			- (0.0, 0.0, 1.0)
			- (1.0, 1.0, 1.0)
			- (0.0, 1.0, 1.0)
			- (0.0, 1.0, 0.0)
			- (1.0, 1.0, 0.0)
		partialCells:
		apGroups:
	Visible Bitfield`1[Visible+StateFlags]
	AutoSpriteRenderer 
	ModuleDamage 
	Damageable 
	GameObjects:
	GameObject name: (GeoCorp_Big_block_222) pos: (0.0, 0.0, 0.0)
		activeSelf: True activeInHierarchy: False
		layer: (8) tag: (Untagged)
		Components:
		UnityEngine.Transform 
		UnityEngine.MeshFilter m_GeoCorp_Big_block_222 Instance
		UnityEngine.MeshRenderer 
		UnityEngine.BoxCollider (0.5, 0.5, 0.5) (2.0, 2.0, 2.0)
		GameObjects:
		GameObject name: (m_GeoCorp_Big_block_222_APs) pos: (0.0, 0.0, 0.0)
			activeSelf: True activeInHierarchy: False
			layer: (8) tag: (Untagged)
			Components:
			UnityEngine.Transform 
			UnityEngine.MeshFilter m_GeoCorp_Big_block_222_APs Instance
			UnityEngine.MeshRenderer 
			GameObjects:
```
