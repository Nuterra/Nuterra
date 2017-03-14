namespace Nuterra.Build.ModuleImport
{
	/// <summary>
	/// Line location span
	/// </summary>
	public struct LineLocationSpan
	{
		/// <summary>
		/// Start line position
		/// </summary>
		public LineLocation StartLinePosition { get; }

		/// <summary>
		/// End line position
		/// </summary>
		public LineLocation EndLinePosition { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="startLinePosition">Start line position</param>
		/// <param name="endLinePosition">End line position</param>
		public LineLocationSpan(LineLocation startLinePosition, LineLocation endLinePosition)
		{
			StartLinePosition = startLinePosition;
			EndLinePosition = endLinePosition;
		}

		/// <summary>
		/// ToString()
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"{StartLinePosition}-{EndLinePosition}";
	}
}