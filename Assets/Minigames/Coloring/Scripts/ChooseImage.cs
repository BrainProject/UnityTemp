using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class ChooseImage : MonoBehaviour
	{
		public GameObject Image;
		public Animation DeskAnimation;
		public LevelManagerColoring thisLevelManager;

		void OnMouseDown(){

			if(!DeskAnimation.IsPlaying("deskRotation") && !DeskAnimation.IsPlaying("deskRotation2"))
			{
				Image.SetActive(true);
				DeskAnimation.Play("deskRotation2");
				MGC.Instance.minigameStates.SetPlayed(Application.loadedLevelName);
				thisLevelManager.painting = true;
				thisLevelManager.ShowColoringGUI(true);
				if(!thisLevelManager.hiddenGUIwhilePainting)
					MGC.Instance.ShowCustomCursor(false);
			}
		}
	}
}