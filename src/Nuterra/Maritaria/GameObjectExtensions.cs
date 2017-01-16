using System;
using UnityEngine;

namespace Maritaria
{
	public static class GameObjectExtensions
	{
		public static T EnsureComponent<T>(this GameObject obj) where T : Component
		{
			return obj.GetComponent<T>() ?? obj.AddComponent<T>();
		}
		
		public static GameObject FindChildGameObject(this GameObject root, string targetName)
		{
			Transform[] ts = root.transform.GetComponentsInChildren<Transform>();
			foreach (Transform t in ts)
			{
				if (t.gameObject.name == targetName)
				{
					return t.gameObject;
				}
			}
			return null;
		}
	}
}