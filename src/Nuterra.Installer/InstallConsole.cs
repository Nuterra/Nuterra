using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Nuterra.Installer
{
	public partial class InstallConsole : UserControl
	{
		private RedirectingWriter _writer;

		public InstallConsole()
		{
			InitializeComponent();
			_writer = new RedirectingWriter(this, Console.Out);
			Console.SetOut(_writer);
		}

		~InstallConsole()
		{
			if (_writer != null)
			{
				Console.SetOut(_writer.InnerWriter);
			}
		}

		private void Write(string value)
		{
			this.BeginInvoke(new Action(() => { output.AppendText(value); }));
		}

		private void Write(char value)
		{
			this.BeginInvoke(new Action(() => { output.AppendText(value.ToString()); }));
		}

		private class RedirectingWriter : TextWriter
		{
			public TextWriter InnerWriter { get; }
			public WeakReference<InstallConsole> Console { get; }

			public RedirectingWriter(InstallConsole console, TextWriter inner)
			{
				if (console == null) throw new ArgumentNullException(nameof(console));
				if (inner == null) throw new ArgumentNullException(nameof(inner));
				InnerWriter = inner;
				Console = new WeakReference<InstallConsole>(console);
			}

			public override Encoding Encoding => InnerWriter.Encoding;

			public override void Write(char value)
			{
				try
				{
					InstallConsole console;
					if (Console.TryGetTarget(out console))
					{
						console.Write(value);
					}
				}
				finally
				{
					InnerWriter.Write(value);
				}
			}

			public override void Write(string value)
			{
				try
				{
					InstallConsole console;
					if (Console.TryGetTarget(out console))
					{
						console.Write(value);
					}
				}
				finally
				{
					InnerWriter.Write(value);
				}
			}
		}
	}
}