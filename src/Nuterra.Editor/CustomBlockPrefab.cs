using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nuterra.Editor
{
	[Serializable]
	public class CustomBlockPrefab : MonoBehaviour
	{
		[Range(2000, 32000)]
		public int BlockID;
		public string Name;
		[Multiline]
		public string Description;
		[Range(0, 100 * 1000)]
		public int Price;
		public Vector3 Dimensions;
		public Sprite DisplaySprite;
		public Vector3[] AttachmentPoints;
		public MyCoolType CoolThing;
	}
	[Serializable]
	public class MyCoolType : MonoBehaviour
	{
		public int TestA;
		public int TestB;
	}
}