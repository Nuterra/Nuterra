using System;

namespace Nuterra.Build.ModuleImport
{
	/// <summary>
	/// Line location
	/// </summary>
	public struct LineLocation
	{
		/// <summary>
		/// Line, 0-based
		/// </summary>
		public int Line { get; }

		/// <summary>
		/// Column, 0-based
		/// </summary>
		public int Character { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="line">Line, 0-based</param>
		/// <param name="character">Column, 0-based</param>
		public LineLocation(int line, int character)
		{
			if (line < 0)
				throw new ArgumentOutOfRangeException(nameof(line));
			if (character < 0)
				throw new ArgumentOutOfRangeException(nameof(line));
			Line = line;
			Character = character;
		}

		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"({Line + 1},{Character + 1})";
	}
}