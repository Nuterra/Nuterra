using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public class SpriteFactory
	{
		private Dictionary<string, Texture2D> _loadedImages = new Dictionary<string, Texture2D>();
		
		public Sprite CreateSprite(string imageName)
		{
			Texture2D texture = FindTexture(imageName);
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		}
		private Texture2D FindTexture(string imageName)
		{
			Texture2D texture;
			if (!_loadedImages.TryGetValue(imageName, out texture))
			{
				texture = LoadTexture(imageName);
				_loadedImages.Add(imageName, texture);
			}
			return texture;
		}
		
		private Texture2D LoadTexture(string imageName)
		{
			byte[] imageData = File.ReadAllBytes(imageName);
			Texture2D texture = new Texture2D(1, 1);
			texture.LoadImage(imageData);
			return texture;
		}
		
	}
}