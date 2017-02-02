using System;

namespace Maritaria.Cursor
{
	public enum CursorType
	{
		/// <summary>
		/// MousePointer.m_DefaultPointer
		/// </summary>
		Default,

		/// <summary>
		/// MousePointer.m_OverGrabbableCursor
		/// </summary>
		Hover,

		/// <summary>
		/// MousePointer.m_HoldingGrabbableCursor
		/// </summary>
		Pressed,

		/// <summary>
		/// MousePointer.m_PaintingCursor
		/// </summary>
		Painting,
	}
}