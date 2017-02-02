using System;
using System.Linq;
using Nuterra;

namespace Maritaria
{
	[Mod]
	public sealed class CustomCursor : TerraTechMod
	{
		public override string Name => "Custom Cursor";
		public override string Description => "Allows the cursor to be changed using a png image";

		public override void Load()
		{
			base.Load();
			var objects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
			var mousePointers = objects.SelectMany(o => o.GetComponentsInChildren<MousePointer>());
			foreach (MousePointer pointer in mousePointers)
			{
				Console.WriteLine($"Pointer: {pointer} {pointer.GetHashCode()}");
			}
		}
	}
}