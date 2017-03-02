using System;

namespace Nuterra.HelloWorld
{
	public sealed class HelloWorldMod : TerraTechMod
	{
		public override string Name => nameof(HelloWorldMod);
		public override string Description => "Minimalistic mod that verifies your install of Nuterra works and mods load correctly";

		public override void Load()
		{
			base.Load();
			Console.WriteLine("Hello world");
		}
	}
}