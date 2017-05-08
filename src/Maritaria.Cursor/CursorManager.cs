using System;
using System.Collections.Generic;
using System.Reflection;
using Nuterra;
using UnityEngine;

namespace Maritaria.Cursor
{
	internal sealed class CursorManager : MonoBehaviour
	{
		private MousePointer _original;

		public Dictionary<CursorType, MousePointer.CursorData> Cursors { get; set; }

		public static CursorManager Install(MousePointer original)
		{
			GameObject owner = original.gameObject;
			var manager = owner.EnsureComponent<CursorManager>();
			GameObject.Destroy(original);
			manager.Setup(original);
			return manager;
		}

		private void Setup(MousePointer original)
		{
			_original = original;
			RestoreFromOriginalMousePointer();
		}

		public void RestoreFromOriginalMousePointer()
		{
			Cursors = new Dictionary<CursorType, MousePointer.CursorData>();

			Cursors.Add(CursorType.Default, _original.DefaultPointer);
			Cursors.Add(CursorType.Hover, _original.OverGrabbableCursor);
			Cursors.Add(CursorType.Pressed, _original.HoldingGrabbableCursor);
			Cursors.Add(CursorType.Painting, _original.PaintingCursor);
		}

		private static MousePointer.CursorData GetOriginalCursor(MousePointer pointer, string name)
		{
			return (MousePointer.CursorData)typeof(MousePointer).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(pointer);
		}

		private void Update()
		{
			var cursor = Cursors[CursorType.Default];
			ManPointer inst = Singleton.Manager<ManPointer>.inst;
			if (inst != null)
			{
				if (inst.DraggingItem != null)
				{
					cursor = ((inst.BuildMode != ManPointer.BuildingMode.PaintBlock) ? this.Cursors[CursorType.Pressed] : this.Cursors[CursorType.Painting]);
				}
				else if (inst.targetVisible && Singleton.Manager<ManPointer>.inst.ItemIsGrabbable(Singleton.Manager<ManPointer>.inst.targetVisible))
				{
					cursor = this.Cursors[CursorType.Hover];
				}
			}
			UnityEngine.Cursor.SetCursor(cursor.m_Texture, cursor.m_Hotspot, CursorMode.Auto);
		}
	}
}