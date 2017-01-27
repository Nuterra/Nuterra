using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuterra.Installer.ModuleImport
{
	/// <summary>
	/// PDB file
	/// </summary>
	public struct DebugFileResult
	{
		/// <summary>
		/// PDB file or null if none
		/// </summary>
		public byte[] RawFile { get; }

		/// <summary>
		/// Debug file format
		/// </summary>
		public DebugFileFormat Format { get; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="format">Debug file format</param>
		/// <param name="rawFile">Debug file data</param>
		public DebugFileResult(DebugFileFormat format, byte[] rawFile)
		{
			Format = format;
			RawFile = rawFile;
		}
	}
}
