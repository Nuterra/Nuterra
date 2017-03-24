using System;
using System.Collections.Generic;
using Nuterra.Editor;
using UnityEngine;

namespace Nuterra
{
	public sealed class BlockPrefabBuilder
	{
		private bool _finished = false;
		private TankBlock _block;
		private Visible _visible;
		private Damageable _damageable;
		private ModuleDamage _moduleDamage;
		private AutoSpriteRenderer _spriteRenderer;
		private GameObject _renderObject;
		private CustomBlock _customBlock;

		public BlockPrefabBuilder()
		{
			_customBlock = new CustomBlock();
			_customBlock.Prefab = new GameObject();
			GameObject.DontDestroyOnLoad(_customBlock.Prefab);

			_customBlock.Prefab.tag = "TankBlock";
			_customBlock.Prefab.layer = Globals.inst.layerTank;

			_visible = _customBlock.Prefab.EnsureComponent<Visible>();
			_damageable = _customBlock.Prefab.EnsureComponent<Damageable>();
			_moduleDamage = _customBlock.Prefab.EnsureComponent<ModuleDamage>();
			_spriteRenderer = _customBlock.Prefab.EnsureComponent<AutoSpriteRenderer>();

			_block = _customBlock.Prefab.EnsureComponent<TankBlock>();
			_block.attachPoints = new Vector3[] { };
			_block.filledCells = new Vector3[] { new Vector3(0, 0, 0) };
			_block.partialCells = new Vector3[] { };
		}

		public BlockPrefabBuilder FromAsset(GameObject prefab)
		{
			var prefabInfo = prefab.GetComponent<CustomBlockPrefab>();
			SetBlockID(prefabInfo.BlockID);
			SetName(prefabInfo.Name);
			SetDescription(prefabInfo.Description);
			SetPrice(prefabInfo.Price);
			SetFaction(FactionSubTypes.GSO);
			SetCategory(BlockCategories.Accessories);
			SetSize(new Vector3I(prefabInfo.Dimensions));
			SetModel(prefab);
			SetIcon(prefabInfo.DisplaySprite);
			return this;
		}

		public void Register()
		{
			ThrowIfFinished();
			_finished = true;
			BlockLoader.Register(_customBlock);
		}

		public BlockPrefabBuilder SetName(string blockName)
		{
			ThrowIfFinished();
			_customBlock.Name = blockName;
			_customBlock.Prefab.name = blockName;
			return this;
		}

		public BlockPrefabBuilder SetBlockID(int id)
		{
			ThrowIfFinished();
			_customBlock.BlockID = id;
			_visible.m_ItemType = new ItemTypeInfo(ObjectTypes.Block, id);
			return this;
		}

		public BlockPrefabBuilder SetDescription(string description)
		{
			ThrowIfFinished();
			_customBlock.Description = description;
			return this;
		}

		public BlockPrefabBuilder SetPrice(int price)
		{
			ThrowIfFinished();
			_customBlock.Price = price;
			return this;
		}

		public BlockPrefabBuilder SetFaction(FactionSubTypes faction)
		{
			ThrowIfFinished();
			_customBlock.Faction = faction;
			return this;
		}

		public BlockPrefabBuilder SetCategory(BlockCategories category)
		{
			ThrowIfFinished();
			_customBlock.Category = category;
			_block.m_BlockCategory = category;
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

		public BlockPrefabBuilder SetMass(float mass)
		{
			ThrowIfFinished();
			if (mass <= 0f) throw new ArgumentOutOfRangeException(nameof(mass), "Cannot be lower or equal to zero");
			_block.m_DefaultMass = mass;
			return this;
		}

		public BlockPrefabBuilder SetModel(string path)
		{
			ThrowIfFinished();
			SetModel(AssetBundleImport.Load<GameObject>(path));
			return this;
		}

		private BlockPrefabBuilder SetModel(GameObject renderObject)
		{
			ThrowIfFinished();
			if (_renderObject)
			{
				GameObject.DestroyImmediate(_renderObject);
			}

			_renderObject = GameObject.Instantiate(renderObject);
			_renderObject.transform.parent = _customBlock.Prefab.transform;
			_renderObject.name = $"RenderObject";
			_renderObject.layer = Globals.inst.layerTank;

			foreach (GameObject child in _renderObject.EnumerateHierarchy(false, false))
			{
				child.layer = _renderObject.layer;
			}

			return this;
		}

		public BlockPrefabBuilder SetIcon(string spriteFile)
		{
			ThrowIfFinished();
			_customBlock.DisplaySprite = AssetBundleImport.Load<Sprite>(spriteFile);
			return this;
		}

		private BlockPrefabBuilder SetIcon(Sprite displaySprite)
		{
			ThrowIfFinished();
			_customBlock.DisplaySprite = displaySprite;
			return this;
		}

		public BlockPrefabBuilder AddComponent<TBehaviour>(Action<TBehaviour> preparer) where TBehaviour : MonoBehaviour
		{
			ThrowIfFinished();
			var component = _customBlock.Prefab.AddComponent<TBehaviour>();
			preparer?.Invoke(component);
			return this;
		}

		public BlockPrefabBuilder AddComponent<TBehaviour>() where TBehaviour : MonoBehaviour
		{
#warning TODO: Make extension method
			return AddComponent<TBehaviour>(null);
		}

		private void ThrowIfFinished()
		{
			if (_finished)
			{
				throw new InvalidOperationException("Build() already called");
			}
		}

		public CustomBlock Build()
		{
			_finished = true;
			return _customBlock;
		}
	}
}