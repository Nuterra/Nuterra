using System;
using UnityEngine;

namespace Maritaria.Cursor
{
	internal struct GameCursor
	{
		public Texture2D Texture { get; set; }
		public Vector2 Hotspot { get; set; }

		public GameCursor(Texture2D texture, Vector2 hotspot)
		{
			Texture = texture;
			Hotspot = hotspot;
		}
	}
}