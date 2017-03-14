using System;

namespace Nuterra.Build.ModuleImport
{
	/// <summary>
	/// Debug file format
	/// </summary>
	public enum DebugFileFormat
	{
		/// <summary>
		/// None
		/// </summary>
		None,

		/// <summary>
		/// PDB
		/// </summary>
		Pdb,

		/// <summary>
		/// Portable PDB
		/// </summary>
		PortablePdb,

		/// <summary>
		/// <see cref="PortablePdb"/> embedded in metadata
		/// </summary>
		Embedded,
	}
}