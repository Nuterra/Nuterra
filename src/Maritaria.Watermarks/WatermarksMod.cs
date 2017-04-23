using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nuterra;
using Nuterra.Internal;
using UnityEngine;

namespace Maritaria.Watermarks
{
	public sealed class WatermarksMod : TerraTechMod
	{
		public override string Name => nameof(WatermarksMod);
		public override string Description => "Adds watermarks to your screengrabs of techs";

		private Texture2D _watermark;

		public override void Load()
		{
			base.Load();
			string watermarkFile = Path.Combine(FolderStructure.DataFolder, "watermark.png");
			if (!File.Exists(watermarkFile))
			{
				Console.WriteLine($"Missing file '{watermarkFile}' mod disabled");
				return;
			}
			Hooks.Managers.Screenshot.BeforeEncodeScreenshot += Screenshot_BeforeEncodeScreenshot;
			_watermark = new Texture2D(0, 0);
			_watermark.LoadImage(File.ReadAllBytes(watermarkFile));
		}

		private void Screenshot_BeforeEncodeScreenshot(ScreenshotEvent info)
		{
			int startx = info.Texture.width - _watermark.width;
			int starty = info.Texture.height - _watermark.height;
			for (int x = 0; x < _watermark.width; x++)
			{
				for (int y = 0; y < _watermark.height; y++)
				{
					Color watermark = _watermark.GetPixel(x, y);
					if (watermark.a < 1)
					{
						Color original = info.Texture.GetPixel(startx + x, starty + y);
						watermark = Color.Lerp(original, watermark, watermark.a);
					}
					info.Texture.SetPixel(startx + x, starty + y, watermark);
				}
			}
		}
	}
}
