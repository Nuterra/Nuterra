using Nuterra;
using System;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public class ProductionStoppedSign : MonoBehaviour
	{
		private static readonly string StopSignFile = @"Assets/Images/stop_sign.png";
		private static Texture2D _stopSign;
		private TankBlock _block;
		private static readonly Rect _signDrawSize = new Rect(0, 0, 32, 32);

		private void Start()
		{
			_stopSign = AssetBundleImport.Load<Texture2D>(StopSignFile);
		}

		private void Awake()
		{
			_block = gameObject.GetComponent<TankBlock>();
		}

		private void OnGUI()
		{
			if (Singleton.Manager<ProductionToggleKeyBehaviour>.inst.ProductionActive)
			{
				return;
			}
			Vector3 blockPos = _block.centreOfMassWorld;
			blockPos.y += 2f;
			Vector3 userPosition = (Singleton.playerTank) ? Singleton.playerTank.boundsCentreWorld : Singleton.cameraTrans.position;
			float distanceToCamera = Vector3.Distance(blockPos, userPosition);
			if (distanceToCamera < 40)
			{
				//Draw only nearby stopsigns
				Vector3 screenPos = Singleton.camera.WorldToScreenPoint(blockPos);
				screenPos.x -= _signDrawSize.width / 2;
				screenPos.y = Screen.height - screenPos.y - (_signDrawSize.height / 2);
				GUI.DrawTexture(new Rect(screenPos, _signDrawSize.size), _stopSign);

			}
		}
	}
}