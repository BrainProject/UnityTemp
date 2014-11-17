using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class NewronGui : MonoBehaviour {
		public Texture normal;
		public Texture hover;
		
		public Animation DeskAnimation;
		public GameObject Images;
		public LevelManagerColoring thisLevelManager;

		void Start()
		{
			this.guiTexture.pixelInset = new Rect (50, Screen.height - Screen.height/9*2 - 50, Screen.width / 16 * 2, Screen.height / 9 * 2);
		}


		void OnMouseEnter()
		{
			this.guiTexture.texture = hover;
		}

		void OnMouseExit()
		{
			this.guiTexture.texture = normal;
		}

		void OnMouseDown()
		{
			if(!DeskAnimation.IsPlaying("deskRotation") && !DeskAnimation.IsPlaying("deskRotation2") && Images.activeSelf)
			{
				DeskAnimation.CrossFade("deskRotation");
				thisLevelManager.painting = false;
				MGC.Instance.ShowCustomCursor(true);
			}
		}
	}
}