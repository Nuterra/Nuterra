using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nuterra
{
	public sealed class BlockPrefabBuilder
	{
		private bool _finished = false;
		private GameObject _prefab;
		private TankBlock _block;
		private Visible _visible;
		private Damageable _damageable;
		private ModuleDamage _moduleDamage;
		private AutoSpriteRenderer _spriteRenderer;
		private GameObject _renderObject;

		public BlockPrefabBuilder()
		{
			_prefab = new GameObject();
			GameObject.DontDestroyOnLoad(_prefab);

			_prefab.tag = "TankBlock";
			_prefab.layer = Globals.inst.layerTank;

			_visible = _prefab.EnsureComponent<Visible>();
			_damageable = _prefab.EnsureComponent<Damageable>();
			_moduleDamage = _prefab.EnsureComponent<ModuleDamage>();
			_spriteRenderer = _prefab.EnsureComponent<AutoSpriteRenderer>();

			_block = _prefab.EnsureComponent<TankBlock>();
			_block.attachPoints = new Vector3[] { };
			_block.filledCells = new Vector3[] { new Vector3(0, 0, 0) };
			_block.partialCells = new Vector3[] { };
		}

		public BlockPrefabBuilder SetName(string blockName)
		{
			ThrowIfFinished();
			_prefab.name = blockName;
			return this;
		}

		public BlockPrefabBuilder SetSize(Vector3I size)
		{
			ThrowIfFinished();
			List<Vector3> cells = new List<Vector3>();
			List<Vector3> aps = new List<Vector3>();
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					for (int z = 0; z < size.z; z++)
					{
						cells.Add(new Vector3(x, y, z));
						if (y == 0)
						{
							aps.Add(new Vector3(x, -0.5f, z));
						}
					}
				}
			}
			_block.filledCells = cells.ToArray();
			_block.attachPoints = aps.ToArray();
			return this;
		}

		public BlockPrefabBuilder SetBlockID(int id)
		{
			ThrowIfFinished();
			_visible.m_ItemType = new ItemTypeInfo(ObjectTypes.Block, id);
			return this;
		}

		public BlockPrefabBuilder SetCategory(BlockCategories category)
		{
			ThrowIfFinished();
			_block.m_BlockCategory = category;
			return this;
		}

		public BlockPrefabBuilder SetModel(string path)
		{
			ThrowIfFinished();
			if (_renderObject)
			{
				GameObject.DestroyImmediate(_renderObject);
			}
			_renderObject = GameObject.Instantiate(AssetBundleImport.Load<GameObject>(path));
			_renderObject.transform.parent = _prefab.transform;
			_renderObject.name = $"RenderObject";
			_renderObject.layer = Globals.inst.layerTank;

			foreach (GameObject child in _renderObject.EnumerateHierarchy(false, false))
			{
				child.layer = _renderObject.layer;
			}


			return this;
		}

		private void ThrowIfFinished()
		{
			if (_finished)
			{
				throw new InvalidOperationException("Build() already called");
			}
		}

		public GameObject Build()
		{
			_finished = true;
			return _prefab;
		}
	}
}