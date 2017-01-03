using System;
using System.IO;
using UnityEngine;

namespace Maritaria
{
	public class ProductionStoppedSign : MonoBehaviour
	{
		private static readonly string StopSignFile = Path.Combine(Mod.DataDirectory, "stop_sign.png");
		private static Texture2D _stopSign;
		private TankBlock _block;
		
		private void Start()
		{
			_stopSign = new SpriteFactory().CreateTexture(StopSignFile);
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
				screenPos.y = Screen.height - screenPos.y - (_stopSign.height / 2);
				screenPos.x -= _stopSign.width / 2;
				GUI.DrawTexture(new Rect(screenPos, new Vector2(32, 32)), _stopSign); 
				
			}
		}
	}
}