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

		public Dictionary<CursorType, GameCursor> Cursors { get; set; }

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
			Cursors = new Dictionary<CursorType, GameCursor>();


			Cursors.Add(CursorType.Default, new GameCursor(GetOriginalCursor(_original, "m_DefaultPointer"), new Vector2(16f, 16f)));
			Cursors.Add(CursorType.Hover, new GameCursor(GetOriginalCursor(_original, "m_OverGrabbableCursor"), new Vector2(16f, 16f)));
			Cursors.Add(CursorType.Pressed, new GameCursor(GetOriginalCursor(_original, "m_HoldingGrabbableCursor"), new Vector2(16f, 16f)));
			Cursors.Add(CursorType.Painting, new GameCursor(GetOriginalCursor(_original, "m_PaintingCursor"), new Vector2(16f, 16f)));
		}

		private static Texture2D GetOriginalCursor(MousePointer pointer, string name)
		{
			return (Texture2D)typeof(MousePointer).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(pointer);
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
			UnityEngine.Cursor.SetCursor(cursor.Texture, cursor.Hotspot, CursorMode.Auto);
		}
	}
}