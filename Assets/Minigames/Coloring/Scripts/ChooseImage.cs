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
			Image.SetActive(true);

			if(!DeskAnimation.IsPlaying("deskRotation") && !DeskAnimation.IsPlaying("deskRotation2"))
			{
				DeskAnimation.Play("deskRotation2");
				MGC.Instance.minigameStates.SetPlayed(Application.loadedLevelName);
				thisLevelManager.CursorType(true);
			}
		}
	}
}