using System;

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