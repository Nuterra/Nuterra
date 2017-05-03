using System;
using System.Collections.Generic;
using System.Linq;
using Nuterra;
using UnityEngine;

namespace Maritaria.WorldBuilder
{
	internal sealed class EditorHotkey : MonoBehaviour
	{
		public WorldBuilderMod Mod { get; set; }
		public bool EditorEnabled { get; set; }
		private TerrainObject SelectedPrefab => _prefabs[_selectedIndex];

		private HashSet<string> _prefabWhitelist = new HashSet<string>() {
			"DesertRock1",
			"RockGrassland1",
			"MountainRock1",
			"RockSaltFlats5",
			"RockSaltFlats6",
			"RockSaltFlats7",
			"CelestiteOutcrop2",
			"CelestiteOutcrop3",
			"CelestiteOutcrop4",
			"EruditeOutcrop5",
			"EruditeOutcrop6",
			"EruditeOutcrop7",
			"IgniteOutcrop5",
			"IgniteOutcrop6",
			"IgniteOutcrop9",
			"LuxiteOutcrop2",
			"LuxiteOutcrop3",
			"DesertTree1",
			"DesertTree2",
			"DesertTree4",
			"GrasslandTree1",
			"GrasslandTree2",
			"GrasslandTree3",
			"GrasslandTree4",
			"GrasslandTree5",
			"GrasslandTree6",
			"TreeShroom1",
			"TreeShroom3",
			"TreeShroom4",
			"MountainTree1",
			"MountainTree3",
			"MountainTree4",
		};
		private List<TerrainObject> _prefabs;
		private int _selectedIndex;
		private GameObject _ghost;

		private void Start()
		{
			_prefabs = new List<TerrainObject>(ManSpawn.inst.GetVanillaTerrainPrefabs().Where(a => _prefabWhitelist.Contains(a.name)));
			foreach (TerrainObject obj in _prefabs)
			{
				Console.WriteLine(obj.name);
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.L))
			{
				EditorEnabled = !EditorEnabled;
				if (EditorEnabled)
				{
					if (!_ghost)
					{
						_ghost = CreateGhost(SelectedPrefab, Vector3.zero);
					}
				}
				else
				{
					Destroy(_ghost);
				}
			}
			if (EditorEnabled)
			{
				Update_Editor();
			}
		}

		private void OnGUI()
		{
			GUI.Label(new Rect(0, 0, 200, 100), $"Selection: {SelectedPrefab.name}");
		}

		private void Update_Editor()
		{
			var mousePos = Input.mousePosition;
			RaycastHit ray;
			bool hit = Physics.Raycast(Singleton.camera.ScreenPointToRay(mousePos), out ray, float.MaxValue, Globals.inst.layerTerrain.mask | Globals.inst.layerScenery.mask, QueryTriggerInteraction.Ignore);
			//LMB
			if (Input.GetMouseButtonDown(0))
			{
				PlaceRock(hit, ray);
			}
			//RMB
			if (Input.GetMouseButtonDown(1))
			{
				_mouseDownPos = mousePos;
			}
			else if (Input.GetMouseButtonUp(1) && (_mouseDownPos == mousePos))
			{
				SelectNextPrefab(hit, ray);
			}

			UpdateGhost(ray);
		}

		private Vector3 _mouseDownPos;

		private void SelectNextPrefab(bool hit, RaycastHit ray)
		{
			_selectedIndex++;
			if (_selectedIndex >= _prefabs.Count - 1)
			{
				_selectedIndex = 0;
			}
			Destroy(_ghost);
			_ghost = CreateGhost(SelectedPrefab, ray.point);
		}

		private void UpdateGhost(RaycastHit ray)
		{
			if (_ghost)
			{
				_ghost.transform.position = ray.point;
				_ghost.transform.rotation = Quaternion.FromToRotation(Vector3.up, ray.normal);
			}
		}

		private GameObject CreateGhost(TerrainObject selectedPrefab, Vector3 pos)
		{
			UnityGraph.LogGameObject(selectedPrefab.gameObject);
			ResourceDispenser dispenser = selectedPrefab.GetComponent<ResourceDispenser>();
			GameObject obj = GameObject.Instantiate(dispenser.AnimatedTransform.GetChild(0).gameObject);
			obj.layer = Globals.inst.layerTrigger;
			foreach (var collider in obj.GetComponentsInChildren<MeshCollider>())
			{
				collider.enabled = false;
			}
			return obj;
		}

		private void PlaceRock(bool hit, RaycastHit ray)
		{
			if (hit && (ray.transform.gameObject.layer == Globals.inst.layerTerrain || ray.transform.gameObject.layer == Globals.inst.layerScenery))
			{
				PlaceRock(ray.point, Quaternion.FromToRotation(Vector3.up, ray.normal));
			}
		}

		private void PlaceRock(Vector3 point, Quaternion direction)
		{
			SelectedPrefab.SpawnFromPrefabAndAddToSaveData(point, direction);
		}
	}
}