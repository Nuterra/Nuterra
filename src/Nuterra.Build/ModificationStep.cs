using System;

namespace Nuterra.Build
{
	public abstract class ModificationStep
	{
		public ModificationStep Next { get; private set; }

		public ModificationStep Previous { get; private set; }

		protected abstract void Perform(ModificationInfo info);

		public ModificationStep SetNext(ModificationStep next)
		{
			if (next == null) throw new ArgumentNullException(nameof(next));
			Next = next;
			next.Previous = this;
			return next;
		}

		private void PerformInternal(ModificationInfo info)
		{
			Perform(info);
			if (Next != null)
			{
				Next.PerformInternal(info);
			}
		}

		public bool ExecuteSteps(ModificationInfo info)
		{
			try
			{
				PerformInternal(info);
				return true;
			}
			catch (ModificationException ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}