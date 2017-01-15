using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuterra.Installer.ModuleImport
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
