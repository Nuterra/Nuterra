using System;
using UnityEngine;

namespace Maritaria.FirstPerson
{
	internal sealed class ModuleFirstPerson : Module
	{
		private GameObject _anchor;

		public GameObject FirstPersonAnchor
		{
			get
			{
				if (!_anchor)
				{
					_anchor = gameObject.FindChildGameObject("FirstPersonAnchor");
				}
				return _anchor;
			}
		}
	}
}