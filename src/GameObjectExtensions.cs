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
	}
}