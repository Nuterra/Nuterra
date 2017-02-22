using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nuterra.Build
{
	public sealed class ModificationException : Exception
	{
		private ModificationException()
		{
		}

		public ModificationException(string message) : base(message)
		{
		}

		public ModificationException(string message, Exception innerException) : base(message, innerException)
		{
		}

	}
}
